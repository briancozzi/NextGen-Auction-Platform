using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.AppAccountEvent.Dto;
using NextGen.BiddingPlatform.Authorization.Accounts;
using NextGen.BiddingPlatform.Core.Addresses;
using NextGen.BiddingPlatform.Core.AppAccountEvents;
using NextGen.BiddingPlatform.Core.AppAccounts;
using NextGen.BiddingPlatform.Country;
using NextGen.BiddingPlatform.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Abp.Timing;
using NextGen.BiddingPlatform.ExtensionMethod;
using NextGen.BiddingPlatform.Timing;
using NextGen.BiddingPlatform.Authorization;
using Abp.Authorization;
using static NextGen.BiddingPlatform.CustomAuthorization.CustomEnum;
using NextGen.BiddingPlatform.Common;
using NextGen.BiddingPlatform.AppAccountEvents.EventPermission;
using Abp.UI;

namespace NextGen.BiddingPlatform.AppAccountEvent
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Event)]
    public class AccountEventAppService : BiddingPlatformAppServiceBase, IAccountEventAppService
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<Core.AppAccounts.AppAccount> _accountRepository;
        private readonly IRepository<Core.State.State> _stateRepository;
        private readonly IRepository<Country.Country> _countryRepository;
        private readonly ICommonPermissionAppService _commonPermissionService;
        private readonly IRepository<EventPermission> _eventPermissionRepo;

        public AccountEventAppService(IRepository<Event> eventRepository,
                                      IRepository<Core.AppAccounts.AppAccount> accountRepository,
                                      IAbpSession abpSession,
                                      IRepository<Core.State.State> stateRepository,
                                      IRepository<Country.Country> countryRepository,
                                      ICommonPermissionAppService commonPermissionService,
                                      IRepository<EventPermission> eventPermissionRepo)
        {
            _eventRepository = eventRepository;
            _accountRepository = accountRepository;
            _abpSession = abpSession;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _commonPermissionService = commonPermissionService;
            _eventPermissionRepo = eventPermissionRepo;
        }

        public async Task<PagedResultDto<AccountEventListDto>> GetAccountEventsWithFilter(AccountEventFilter input)
        {
            //for get current user timezzone from MySetting
            //var currentUserTimeZone = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);
            var query = _eventRepository.GetAllIncluding(x => x.AppAccount);
            var eventIds = await GetAccessibleIds(AccessType.List, null);
            if (eventIds != null)
                query = query.Where(x => eventIds.Contains(x.Id));

            var resultQuery = query.WhereIf(!input.Search.IsNullOrWhiteSpace(), x => x.EventName.ToLower().IndexOf(input.Search.ToLower()) > -1)
                                         .Select(x => new AccountEventListDto
                                         {
                                             AppAccountUniqueId = x.AppAccount.UniqueId,
                                             EventEndDateTime = x.EventEndDateTime,//.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone),
                                             EventStartDateTime = x.EventStartDateTime,//.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone),
                                             EventName = x.EventName,
                                             EventUrl = x.EventUrl,
                                             TimeZone = x.TimeZone,
                                             UniqueId = x.UniqueId
                                         });

            var resultCount = await resultQuery.CountAsync();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
                resultQuery = resultQuery.OrderBy(input.Sorting);

            resultQuery = resultQuery.PageBy(input);

            return new PagedResultDto<AccountEventListDto>(resultCount, resultQuery.ToList());
        }

        public async Task<ListResultDto<AccountEventListDto>> GetAllAccountEvents()
        {
            //get current user timezone which is added through MySettings
            //var currentUserTimeZone = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);
            var query = _eventRepository.GetAllIncluding(x => x.AppAccount);

            var eventIds = await GetAccessibleIds(AccessType.List, null);
            if (eventIds != null)
                query = query.Where(x => eventIds.Contains(x.Id));

            var eventsData = await query.Select(x => new AccountEventListDto
            {
                AppAccountUniqueId = x.AppAccount.UniqueId,
                EventEndDateTime = x.EventEndDateTime,//.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone),
                EventStartDateTime = x.EventStartDateTime,//.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone),
                EventName = x.EventName,
                EventUrl = x.EventUrl,
                TimeZone = x.TimeZone,
                UniqueId = x.UniqueId
            }).ToListAsync();

            return new ListResultDto<AccountEventListDto>(eventsData);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Event_Create)]
        public async Task<CreateAccountEventDto> Create(CreateAccountEventDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
            if (country == null)
                throw new Exception("Country not found for given id");
            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (state == null)
                throw new Exception("State not found for given id");
            var account = await _accountRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AppAccountUniqueId);
            if (account == null)
                throw new Exception("Account not found for given id");
            if (!_abpSession.TenantId.HasValue)
                throw new Exception("You are not Authorized user.");
            //convert dates to Utc time
            input.EventEndDateTime = input.EventEndDateTime;
            input.EventStartDateTime = input.EventStartDateTime;

            var events = ObjectMapper.Map<Event>(input);
            //events.EventDate = input.EventDate.Date;
            events.UniqueId = Guid.NewGuid();
            events.Address.UniqueId = Guid.NewGuid();
            events.AppAccountId = account.Id;
            events.Address.CountryId = country.Id;
            events.Address.StateId = state.Id;

            events.TenantId = _abpSession.TenantId.Value;
            foreach (var item in input.Users)
            {
                events.EventPermissions.Add(new EventPermission
                {
                    UserId = item
                });
            }
            await _eventRepository.InsertAsync(events);
            return input;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Event_Edit)]
        public async Task<UpdateAccountEventDto> Update(UpdateAccountEventDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);

            if (country == null)
                throw new Exception("Country not found for given id");
            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (state == null)
                throw new Exception("State not found for given id");

            var events = await _eventRepository.GetAllIncluding(x => x.Address, x => x.EventPermissions).FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (events == null)
                throw new Exception("Event not found for given id");

            var accessIds = await GetAccessibleIds(AccessType.Edit, events.Id);
            if (accessIds.Count == 0)
                throw new UserFriendlyException("You are not authorie to update this record");

            var account = await _accountRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AppAccountUniqueId);
            if (account == null)
                throw new Exception("Account not found for given id");

            if (events.EventPermissions.Count > 0)
                await _eventPermissionRepo.DeleteAsync(x => x.EventId == events.Id);

            foreach (var item in input.Users)
            {
                events.EventPermissions.Add(new EventPermission
                {
                    UserId = item
                });
            }

            //Event Properties
            events.Email = input.Email;
            events.EventName = input.EventName;

            //events.EventDate = input.EventDate;
            events.EventUrl = input.EventUrl;
            events.EventStartDateTime = input.EventStartDateTime;
            events.EventEndDateTime = input.EventEndDateTime;
            events.MobileNo = input.MobileNo;
            events.TimeZone = input.TimeZone;
            events.IsActive = input.IsActive;
            //Address Properties
            events.Address.Address1 = input.Address.Address1;
            events.Address.Address2 = input.Address.Address2;
            events.Address.City = input.Address.City;
            events.Address.ZipCode = input.Address.ZipCode;
            events.Address.StateId = state.Id;
            events.Address.CountryId = country.Id;
            events.AppAccountId = account.Id;
            await _eventRepository.UpdateAsync(events);
            return input;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Event_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            var events = await _eventRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Id);
            if (events == null)
                throw new Exception("Event not found for given id");

            var accessIds = await GetAccessibleIds(AccessType.Delete, events.Id);
            if (accessIds.Count == 0)
                throw new UserFriendlyException("You are not authorie to delete this record");

            await _eventRepository.DeleteAsync(events);
        }

        public async Task<AccountEventDto> GetAccountEventById(Guid Id)
        {
            //var currentUserTimeZone = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);
            var Event = await _eventRepository.GetAllIncluding(x =>
                                                               x.AppAccount,
                                                               x => x.Address,
                                                               x => x.Address.State,
                                                               x => x.Address.Country,
                                                               x => x.EventPermissions)
                                              .FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (Event == null)
                throw new Exception("Event not found for given id");

            var accessIds = await GetAccessibleIds(AccessType.Edit, Event.Id);
            if (accessIds.Count == 0)
                throw new UserFriendlyException("You are not authorie to update this record");

            var mappedEvent = ObjectMapper.Map<AccountEventDto>(Event);
            mappedEvent.Users = Event.EventPermissions.Select(x => x.UserId).ToList();
            mappedEvent.EventEndDateTime = mappedEvent.EventEndDateTime;//.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone);
            mappedEvent.EventStartDateTime = mappedEvent.EventStartDateTime;//.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone);
            return mappedEvent;
        }

        public async Task<AccountEventListDto> GetEventDateTimeByEventId(Guid Id)
        {
            var existingEvent = await _eventRepository.FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (existingEvent == null)
                throw new UserFriendlyException("Event not found for given id");

            var @event = new AccountEventListDto
            {
                EventEndDateTime = existingEvent.EventEndDateTime,
                EventStartDateTime = existingEvent.EventStartDateTime,
                TimeZone = existingEvent.TimeZone,
                EventName = existingEvent.EventName,
                EventUrl = existingEvent.EventUrl,
                UniqueId = existingEvent.UniqueId
            };

            return @event;
        }

        private async Task<List<int>> GetAccessibleIds(AccessType accessType, int? eventId)
        {
            List<int> eventIds = null;
            var userPermissions = await _commonPermissionService.GetUserPermissions();

            if (userPermissions.Contains(AppPermissions.Pages_Administration_Tenant_Event_All))
            {
                if (accessType == AccessType.List)
                    return null;

                eventIds = new List<int>();
                eventIds.Add(eventId ?? 0);
                return eventIds;
            }

            eventIds = new List<int>();
            if (userPermissions.Contains(AppPermissions.Pages_Administration_Tenant_Event_Assign))
            {
                if (accessType == AccessType.List)
                    eventIds = await GetAssignedEvents(AbpSession.UserId.Value);
                else if (accessType == AccessType.Get || accessType == AccessType.Edit || accessType == AccessType.Delete)
                {
                    var hasAccess = await IsEventAccessible(AbpSession.UserId.Value, eventId.Value);
                    if (hasAccess)
                        eventIds.Add(eventId.Value);
                }
                return eventIds;
            }

            return eventIds;
        }

        private async Task<List<int>> GetAssignedEvents(long userId)
        {
            var user = await UserManager.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found for given id");

            var selfEvents = await _accountRepository.GetAllListAsync(x => x.CreatorUserId == userId);

            var filterEvent = await _eventPermissionRepo.GetAllListAsync(x => x.UserId == userId);

            var selfEventIds = selfEvents.Select(x => x.Id);
            var filterIds = filterEvent.Select(x => x.EventId);

            selfEventIds = selfEventIds.Union(filterIds);

            return selfEventIds.ToList();
        }

        private async Task<bool> IsEventAccessible(long userId, int eventId)
        {
            var selfEvents = await _eventRepository.FirstOrDefaultAsync(x => x.CreatorUserId == userId && x.Id == eventId);

            var filterEvent = await _eventPermissionRepo.FirstOrDefaultAsync(x => x.UserId == userId && x.EventId == eventId);

            return selfEvents != null || filterEvent != null;
        }
    }
}
