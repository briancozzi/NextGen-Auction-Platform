using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.AppAccountEvent.Dto;
using NextGen.BiddingPlatform.Core.AppAccountEvents;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using NextGen.BiddingPlatform.Authorization;
using Abp.Authorization;
using Abp.UI;
using NextGen.BiddingPlatform.Authorization.Users;
using System.Collections.Generic;
using Abp.Webhooks;
using NextGen.BiddingPlatform.WebHooks;
using NextGen.BiddingPlatform.Helpers;
using System.Net;

namespace NextGen.BiddingPlatform.AppAccountEvent
{
    public class AccountEventAppService : BiddingPlatformAppServiceBase, IAccountEventAppService
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<Core.Auctions.Auction> _auctionRepository;
        private readonly IRepository<Core.AuctionItems.AuctionItem> _auctionItemRepository;

        private readonly IRepository<Core.AppAccounts.AppAccount> _accountRepository;
        private readonly IRepository<Core.State.State> _stateRepository;
        private readonly IRepository<Country.Country> _countryRepository;
        private readonly IUserAppService _userAppService;
        private readonly IWebhookPublisher _webHookPublisher;
        public AccountEventAppService(IRepository<Event> eventRepository,
                                      IRepository<Core.AppAccounts.AppAccount> accountRepository,
                                      IRepository<Core.State.State> stateRepository,
                                      IRepository<Country.Country> countryRepository,
                                      IUserAppService userAppService,
                                      IRepository<Core.Auctions.Auction> auctionRepository,
                                      IRepository<Core.AuctionItems.AuctionItem> auctionItemRepository,
                                       IWebhookPublisher webhookPublisher)
        {
            _eventRepository = eventRepository;
            _accountRepository = accountRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _userAppService = userAppService;
            _auctionRepository = auctionRepository;
            _auctionItemRepository = auctionItemRepository;
            _webHookPublisher = webhookPublisher;
        }
        [AbpAllowAnonymous]
        public async Task<ApiResponse<List<AccountEventListDto>>> GetAllAnnonymousAccountEvents(bool includeClosed = false)
        {
            List<AccountEventListDto> eventsData = new List<AccountEventListDto>();
            try
            {
                var query = await _eventRepository.GetAll().AsNoTracking()
                .Include(s => s.AppAccount)
                .Include(s => s.EventAuctions)
                .Include($"{nameof(Event.EventAuctions)}.{nameof(Core.Auctions.Auction.AuctionItems)}")
                .Include($"{nameof(Event.EventAuctions)}.{nameof(Core.Auctions.Auction.AuctionItems)}.{nameof(Core.AuctionItems.AuctionItem.Item)}")
                .Where(x => x.EventAuctions.Any(c => c.AuctionItems.Any())).ToListAsync();

                var tenantOd = AbpSession.TenantId;
                foreach (var x in query)
                {
                    if (!includeClosed)
                    {
                        if (x.EventAuctions.Any(s => s.AuctionEndDateTime >= DateTime.UtcNow))
                        {
                            var itemCount = x.EventAuctions.Select(s => s.AuctionItems.Where(x => x.Item.IsShow).Count()).Sum();
                            if (itemCount > 0)
                            {
                                eventsData.Add(new AccountEventListDto
                                {
                                    Id = x.Id,
                                    AppAccountUniqueId = x.AppAccount.UniqueId,
                                    EventEndDateTime = x.EventEndDateTime,//.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone),
                                    EventStartDateTime = x.EventStartDateTime,//.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone),
                                    EventName = x.EventName,
                                    EventUrl = x.EventUrl,
                                    TimeZone = x.TimeZone,
                                    UniqueId = x.UniqueId,
                                    ItemCount = itemCount,
                                    IsEventClosedOrExpired = false
                                });
                            }
                        }
                    }
                    else
                    {
                        var itemCount = x.EventAuctions.Select(s => s.AuctionItems.Where(x => x.Item.IsShow).Count()).Sum();
                        if (itemCount > 0)
                        {
                            eventsData.Add(new AccountEventListDto
                            {
                                Id = x.Id,
                                AppAccountUniqueId = x.AppAccount.UniqueId,
                                EventEndDateTime = x.EventEndDateTime,//.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone),
                                EventStartDateTime = x.EventStartDateTime,//.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone),
                                EventName = x.EventName,
                                EventUrl = x.EventUrl,
                                TimeZone = x.TimeZone,
                                UniqueId = x.UniqueId,
                                ItemCount = itemCount,
                                IsEventClosedOrExpired = x.EventAuctions.Any(c => c.AuctionItems.Any(d => d.IsBiddingClosed))
                            });
                        }
                    }

                }

                return new ApiResponse<List<AccountEventListDto>>
                {
                    Data = eventsData,
                    //Status = true,
                    //Message = "Successfully fetch the data.",
                    //StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<AccountEventListDto>>
                {
                    Data = eventsData,
                    //Status = false,
                    //Message = "Error occured while fetching the data.",
                    //StatusCode = HttpStatusCode.InternalServerError
                };
            }

        }

        [AbpAuthorize]
        public async Task<EventWithAuctionItems> GetEventById(int id)
        {
            try
            {
                var currEvent = await _eventRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

                var auctionIds = await _auctionRepository.GetAll()
                                                    .AsNoTracking()
                                                    .Where(s => s.EventId == currEvent.Id).Select(s => s.Id).ToListAsync();

                var auctionItems = await _auctionItemRepository.GetAll()
                                                .Include(s => s.Item)
                                                .Include(s => s.Auction)
                                                .Include(s => s.AuctionHistories)
                                                .AsNoTracking().Where(s => auctionIds.Contains(s.AuctionId)).Select(x => new AuctionItem.Dto.AuctionItemListDto
                                                {
                                                    AuctionItemId = x.UniqueId,
                                                    ItemName = x.Item.ItemName,
                                                    ItemId = x.Item.UniqueId,
                                                    ItemNumber = x.Item.ItemNumber,
                                                    ItemStatus = x.Item.ItemStatus,
                                                    ActualItemId = x.ItemId,
                                                    LastBidAmount = x.AuctionHistories.OrderByDescending(x => x.CreationTime).FirstOrDefault().BidAmount,
                                                    AuctionId = x.Auction.UniqueId


                                                }).ToListAsync();

                var result = new EventWithAuctionItems
                {
                    EventName = currEvent.EventName,
                    EventStartDate = currEvent.EventStartDateTime,
                    EventEndDate = currEvent.EventEndDateTime,
                    AuctionItems = auctionItems
                };

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Event)]
        public async Task<PagedResultDto<AccountEventListDto>> GetAccountEventsWithFilter(AccountEventFilter input)
        {
            //for get current user timezzone from MySetting
            //var currentUserTimeZone = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);
            var currUser = _userAppService.GetCurrUser();
            var query = _eventRepository.GetAllIncluding(x => x.AppAccount);
            if (currUser.AppAccountId.HasValue)
                query = query.Where(x => x.AppAccountId == currUser.AppAccountId.Value);

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

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Event)]
        public async Task<ListResultDto<AccountEventListDto>> GetAllAccountEvents()
        {
            //get current user timezone which is added through MySettings
            //var currentUserTimeZone = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);
            var currUser = _userAppService.GetCurrUser();
            var query = _eventRepository.GetAllIncluding(x => x.AppAccount);
            if (currUser.AppAccountId.HasValue)
                query = query.Where(x => x.AppAccountId == currUser.AppAccountId.Value);

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

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Event, AppPermissions.Pages_Administration_Tenant_Event_Create, RequireAllPermissions = true)]
        public async Task<CreateAccountEventDto> Create(CreateAccountEventDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
            if (country == null)
                throw new UserFriendlyException("Country not found for given id");

            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (state == null)
                throw new UserFriendlyException("State not found for given id");

            var account = await _accountRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AppAccountUniqueId);
            if (account == null)
                throw new UserFriendlyException("Account not found for given id");

            if (!AbpSession.TenantId.HasValue)
                throw new UserFriendlyException("You are not Authorized user.");

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

            events.TenantId = AbpSession.TenantId.Value;

            await _eventRepository.InsertAsync(events);
            input.EventUniqueId = events.UniqueId;
            return input;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Event, AppPermissions.Pages_Administration_Tenant_Event_Edit, RequireAllPermissions = true)]
        public async Task<UpdateAccountEventDto> Update(UpdateAccountEventDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
            if (country == null)
                throw new UserFriendlyException("Country not found for given id");

            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (state == null)
                throw new UserFriendlyException("State not found for given id");

            var events = await _eventRepository.GetAllIncluding(x => x.Address, x => x.EventPermissions).FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (events == null)
                throw new UserFriendlyException("Event not found for given id");

            var account = await _accountRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AppAccountUniqueId);
            if (account == null)
                throw new UserFriendlyException("Account not found for given id");


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

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Event, AppPermissions.Pages_Administration_Tenant_Event_Delete, RequireAllPermissions = true)]
        public async Task Delete(EntityDto<Guid> input)
        {
            var events = await _eventRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Id);
            if (events == null)
                throw new UserFriendlyException("Event not found for given id");

            await _eventRepository.DeleteAsync(events);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Event)]
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



            var mappedEvent = ObjectMapper.Map<AccountEventDto>(Event);
            mappedEvent.Users = Event.EventPermissions.Select(x => x.UserId).ToList();
            mappedEvent.EventEndDateTime = mappedEvent.EventEndDateTime;//.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone);
            mappedEvent.EventStartDateTime = mappedEvent.EventStartDateTime;//.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone);
            return mappedEvent;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Event)]
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

        //private async Task<List<int>> GetAccessibleIds(AccessType accessType, int? eventId)
        //{
        //    List<int> eventIds = null;
        //    var userPermissions = await _commonPermissionService.GetUserPermissions();

        //    if (userPermissions.Contains(AppPermissions.Pages_Administration_Tenant_Event_All))
        //    {
        //        if (accessType == AccessType.List)
        //            return null;

        //        eventIds = new List<int>();
        //        eventIds.Add(eventId ?? 0);
        //        return eventIds;
        //    }

        //    eventIds = new List<int>();
        //    if (userPermissions.Contains(AppPermissions.Pages_Administration_Tenant_Event_Assign))
        //    {
        //        if (accessType == AccessType.List)
        //            eventIds = await GetAssignedEvents(AbpSession.UserId.Value);
        //        else if (accessType == AccessType.Get || accessType == AccessType.Edit || accessType == AccessType.Delete)
        //        {
        //            var hasAccess = await IsEventAccessible(AbpSession.UserId.Value, eventId.Value);
        //            if (hasAccess)
        //                eventIds.Add(eventId.Value);
        //        }
        //        return eventIds;
        //    }

        //    return eventIds;
        //}

        //private async Task<List<int>> GetAssignedEvents(long userId)
        //{
        //    var user = await UserManager.GetUserByIdAsync(userId);
        //    if (user == null)
        //        throw new Exception("User not found for given id");

        //    var selfEvents = await _accountRepository.GetAllListAsync(x => x.CreatorUserId == userId);

        //    var filterEvent = await _eventPermissionRepo.GetAllListAsync(x => x.UserId == userId);

        //    var selfEventIds = selfEvents.Select(x => x.Id);
        //    var filterIds = filterEvent.Select(x => x.EventId);

        //    selfEventIds = selfEventIds.Union(filterIds);

        //    return selfEventIds.ToList();
        //}

        //private async Task<bool> IsEventAccessible(long userId, int eventId)
        //{
        //    var selfEvents = await _eventRepository.FirstOrDefaultAsync(x => x.CreatorUserId == userId && x.Id == eventId);

        //    var filterEvent = await _eventPermissionRepo.FirstOrDefaultAsync(x => x.UserId == userId && x.EventId == eventId);

        //    return selfEvents != null || filterEvent != null;
        //}


        public async Task CloseBiddingOnEvent(Guid eventId)
        {
            var @event = await _eventRepository.GetAll().AsNoTracking()
                                .Include(s => s.EventAuctions).FirstOrDefaultAsync(s => s.UniqueId == eventId);

            if (@event == null)
                throw new UserFriendlyException("Event not found!!");

            List<int> auctionIds = new List<int>();
            foreach (var s in @event.EventAuctions)
            {
                if ((s.AuctionEndDateTime - DateTime.UtcNow).TotalHours >= 0)
                {
                    auctionIds.Add(s.Id);
                }
            }

            var auctionItems = await _auctionItemRepository.GetAll().AsNoTracking()
                                        .Where(s => auctionIds.Contains(s.AuctionId)).ToListAsync();

            List<Guid> auctionItemIds = new List<Guid>();
            foreach (var auctionItem in auctionItems)
            {
                auctionItem.IsBiddingClosed = true;
                await _auctionItemRepository.UpdateAsync(auctionItem);
                auctionItemIds.Add(auctionItem.UniqueId);
            }


            await _webHookPublisher.PublishAsync(AppWebHookNames.CloseBiddingOnEventOrItem,
                new CloseEventOrItemDto
                {
                    AuctionItemIds = auctionItemIds,
                    TenantId = AbpSession.TenantId,
                    FromService = "Events"
                }, AbpSession.TenantId);
        }

    }
}
