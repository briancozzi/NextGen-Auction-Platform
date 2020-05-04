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

namespace NextGen.BiddingPlatform.AppAccountEvent
{
    public class AccountEventAppService : BiddingPlatformAppServiceBase, IAccountEventAppService
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<Core.AppAccounts.AppAccount> _accountRepository;
        private readonly IRepository<Core.State.State> _stateRepository;
        private readonly IRepository<Country.Country> _countryRepository;

        public AccountEventAppService(IRepository<Event> eventRepository,
                                      IRepository<Core.AppAccounts.AppAccount> accountRepository,
                                      IAbpSession abpSession,
                                      IRepository<Core.State.State> stateRepository,
                                      IRepository<Country.Country> countryRepository)
        {
            _eventRepository = eventRepository;
            _accountRepository = accountRepository;
            _abpSession = abpSession;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
        }

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
            input.EventEndDateTime = input.EventEndDateTime.ConverUserTimeZoneToUtcTime(input.TimeZone);
            input.EventStartDateTime = input.EventStartDateTime.ConverUserTimeZoneToUtcTime(input.TimeZone);

            var events = ObjectMapper.Map<Event>(input);
            //events.EventDate = input.EventDate.Date;
            events.UniqueId = Guid.NewGuid();
            events.Address.UniqueId = Guid.NewGuid();
            events.AppAccountId = account.Id;
            events.Address.CountryId = country.Id;
            events.Address.StateId = state.Id;

            account.TenantId = _abpSession.TenantId.Value;
            await _eventRepository.InsertAsync(events);
            return input;
        }

        public async Task<UpdateAccountEventDto> Update(UpdateAccountEventDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
            if (country == null)
                throw new Exception("Country not found for given id");
            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (state == null)
                throw new Exception("State not found for given id");

            var events = await _eventRepository.GetAllIncluding(x => x.Address).FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (events == null)
                throw new Exception("Event not found for given id");

            //Event Properties
            events.Email = input.Email;
            events.EventName = input.EventName;
            //events.EventDate = input.EventDate;
            events.EventUrl = input.EventUrl;
            events.EventStartDateTime = input.EventStartDateTime.ConverUserTimeZoneToUtcTime(input.TimeZone);
            events.EventEndDateTime = input.EventEndDateTime.ConverUserTimeZoneToUtcTime(input.TimeZone); ;
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
            await _eventRepository.UpdateAsync(events);
            return input;
        }

        public async Task Delete(EntityDto<Guid> input)
        {
            var events = await _eventRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Id);
            if (events == null)
                throw new Exception("Event not found for given id");

            await _eventRepository.DeleteAsync(events);
        }
        public async Task<string> Test()
        {
            return await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);

        }
        public async Task<ListResultDto<AccountEventListDto>> GetAllAccountEvents()
        {
            var currentUserTimeZone = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);
            var eventsData = await _eventRepository.GetAllIncluding(x => x.AppAccount)
                                                    .Select(x => new AccountEventListDto
                                                    {
                                                        AccountUniqueId = x.AppAccount.UniqueId,
                                                        EventEndDateTime = x.EventEndDateTime.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone),
                                                        EventStartDateTime = x.EventStartDateTime.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone),
                                                        EventName = x.EventName,
                                                        EventUrl = x.EventUrl,
                                                        TimeZone = x.TimeZone,
                                                        UniqueId = x.UniqueId
                                                    })
                                                    .ToListAsync();

            return new ListResultDto<AccountEventListDto>(eventsData);
        }

        public async Task<PagedResultDto<AccountEventListDto>> GetAccountEventsWithFilter(AccountEventFilter input)
        {
            var currentUserTimeZone = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);
            var query = _eventRepository.GetAllIncluding(x => x.AppAccount)
                                         .WhereIf(!input.Search.IsNullOrWhiteSpace(), x => x.EventName.ToLower().IndexOf(input.Search.ToLower()) > -1)
                                         .Select(x => new AccountEventListDto
                                         {
                                             AccountUniqueId = x.AppAccount.UniqueId,
                                             EventEndDateTime = x.EventEndDateTime.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone),
                                             EventStartDateTime = x.EventStartDateTime.ConvertTimeFromUtcToUserTimeZone(currentUserTimeZone),
                                             EventName = x.EventName,
                                             EventUrl = x.EventUrl,
                                             TimeZone = x.TimeZone,
                                             UniqueId = x.UniqueId
                                         });

            var resultCount = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);

            query = query.PageBy(input);

            return new PagedResultDto<AccountEventListDto>(resultCount, await query.ToListAsync());
        }

        public async Task<AccountEventDto> GetAccountEventById(Guid Id)
        {
            var Event = await _eventRepository.GetAllIncluding(x =>
                                                               x.AppAccount,
                                                               x => x.Address,
                                                               x => x.Address.State,
                                                               x => x.Address.Country)
                                              .FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (Event == null)
                throw new Exception("Event not found for given id");

            var mappedEvent = ObjectMapper.Map<AccountEventDto>(Event);
            mappedEvent.EventEndDateTime = mappedEvent.EventEndDateTime.ConvertTimeFromUtcToUserTimeZone(mappedEvent.TimeZone);
            mappedEvent.EventStartDateTime = mappedEvent.EventStartDateTime.ConvertTimeFromUtcToUserTimeZone(mappedEvent.TimeZone);
            return mappedEvent;
        }
    }
}
