using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
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
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.AppAccountEvent
{
    public class AccountEventAppService : BiddingPlatformDomainServiceBase,IAccountEventAppService
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<Core.AppAccounts.AppAccount> _accountRepository;
        private readonly ICountryAppService _countryService;
        private readonly IStateAppService _stateService;
        public AccountEventAppService(IRepository<Event> eventRepository,
                                      IRepository<Core.AppAccounts.AppAccount> accountRepository,
                                      IAbpSession abpSession,
                                      ICountryAppService countryService,
                                      IStateAppService stateService)
        {
            _eventRepository = eventRepository;
            _accountRepository = accountRepository;
            _abpSession = abpSession;
            _countryService = countryService;
            _stateService = stateService;
        }

        public async Task<CreateAccountEventDto> Create(CreateAccountEventDto input)
        {
            var account = await _accountRepository.GetAll().FirstOrDefaultAsync(x => x.UniqueId == input.AppAccountUniqueId);
            var country = await _countryService.GetCountryById(input.Address.CountryUniqueId);
            var state = await _stateService.GetStateById(input.Address.StateUniqueId);
            if (account == null)
                throw new Exception("Country or State not found");

            var events = ObjectMapper.Map<Core.AppAccountEvents.Event>(input);
            events.UniqueId = Guid.NewGuid();
            events.Address.UniqueId = Guid.NewGuid();
            events.AppAccountId = account.Id;
            events.Address.CountryId = country.Id;
            events.Address.StateId = state.Id;
            if (!_abpSession.TenantId.HasValue)
                throw new Exception("You are not Authorized user.");

            account.TenantId = _abpSession.TenantId.Value;
            await _eventRepository.InsertAsync(events);
            return input;
        }

        public async Task<UpdateAccountEventDto> Update(UpdateAccountEventDto input)
        {
            var account = await _accountRepository.GetAll().FirstOrDefaultAsync(x => x.UniqueId == input.AppAccountUniqueId);
            var country = await _countryService.GetCountryById(input.Address.CountryUniqueId);
            var state = await _stateService.GetStateById(input.Address.StateUniqueId);
            if (account == null && country == null && state == null)
                throw new Exception("account, Country or State not found");

            var events = await _eventRepository.GetAllIncluding(x => x.Address).FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (events == null)
                throw new Exception("No data found");

            //AppAccount Properties
            events.Email = input.Email;
            events.EventName = input.EventName;
            events.EventDate = input.EventDate;
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
            //AppAccount
            events.AppAccount.Id = account.Id;
            await _eventRepository.UpdateAsync(events);
            return input;
        }

        public async Task Delete(EntityDto<Guid> input)
        {
            var events = await _eventRepository.GetAll().FirstOrDefaultAsync(x => x.UniqueId == input.Id);
            if (events == null)
                throw new Exception("No data found");

            await _eventRepository.DeleteAsync(events);
        }

        public async Task<ListResultDto<AccountEventListDto>> GetAllAccountEvents()
        {
            var eventsData = await _eventRepository
                    .GetAllIncluding(x => x.Address, x => x.Address.Country, x => x.Address.State)
                    .ToListAsync();
            if (eventsData == null)
                throw new Exception("No data found");

            return new ListResultDto<AccountEventListDto>(ObjectMapper.Map<List<AccountEventListDto>>(eventsData));
        }

        public async Task<AccountEventDto> GetAccountEventById(Guid Id)
        {
            var Event = await _eventRepository.GetAllIncluding(x => x.Address, x => x.Address.State, x => x.Address.Country)
                                              .FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (Event == null)
                throw new Exception("No data found");

            return ObjectMapper.Map<AccountEventDto>(Event);
        }


    }
}
