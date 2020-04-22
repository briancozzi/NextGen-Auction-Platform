using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Core.AppAccounts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NextGen.BiddingPlatform.AppAccount.Dto;
using NextGen.BiddingPlatform.Country;
using NextGen.BiddingPlatform.State;
using NextGen.BiddingPlatform.DashboardCustomization.Dto;

namespace NextGen.BiddingPlatform.AppAccount
{
    public class AppAccountAppService : BiddingPlatformDomainServiceBase, IAppAccountAppService
    {
        private readonly IRepository<Core.AppAccounts.AppAccount> _accountRepository;
        private readonly ICountryAppService _countryAppService;
        private readonly IStateAppService _stateAppService;
        private readonly Abp.Runtime.Session.IAbpSession AbpSession;
        public AppAccountAppService(IRepository<Core.AppAccounts.AppAccount> accountRepository,
                                    ICountryAppService countryAppService,
                                    IStateAppService stateAppService,
                                    Abp.Runtime.Session.IAbpSession abpSession)
        {
            _accountRepository = accountRepository;
            _countryAppService = countryAppService;
            _stateAppService = stateAppService;
            AbpSession = abpSession;
        }

        public async Task Create(CreateAppAccountDto input)
        {
            var country = await _countryAppService.GetCountryById(input.Address.CountryUniqueId);

            var state = await _stateAppService.GetStateById(input.Address.StateUniqueId);

            var account = ObjectMapper.Map<Core.AppAccounts.AppAccount>(input);
            account.UniqueId = Guid.NewGuid();
            account.Address.UniqueId = Guid.NewGuid();
            account.Address.StateId = state.Id;
            account.Address.CountryId = country.Id;
            if (!AbpSession.TenantId.HasValue)
                throw new Exception("You are not Authorized user.");

            account.TenantId = AbpSession.TenantId.Value;
            await _accountRepository.InsertAsync(account);
            /*ObjectMapper.Map<AppAccountDto>(output);*/
        }

        public async Task Delete(EntityDto<Guid> input)
        {
            var state = await _accountRepository.GetAll().FirstOrDefaultAsync(x => x.UniqueId == input.Id);
            if (state == null)
                throw new Exception("No data found");

            await _accountRepository.DeleteAsync(state);
        }

        public async Task<List<AppAccountListDto>> GetAllAccount()
        {
            var accounts = await _accountRepository.GetAllIncluding(x => x.Address).ToListAsync();
            return ObjectMapper.Map<List<AppAccountListDto>>(accounts);
        }

        public async Task<AppAccountDto> GetAccountById(Guid Id)
        {
            var account = await _accountRepository.GetAllIncluding(x => x.Address).FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (account == null)
                throw new Exception("No data found");

            return ObjectMapper.Map<AppAccountDto>(account);
        }

        public async Task Update(UpdateAppAccountDto input)
        {
            var country = await _countryAppService.GetCountryById(input.Address.CountryUniqueId);
            if (country == null)
                throw new Exception("No data found");
            var state = await _stateAppService.GetStateById(input.Address.StateUniqueId);
            if (country == null)
                throw new Exception("No data found");
            var account = await _accountRepository.GetAllIncluding(x => x.Address).FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (account == null)
                throw new Exception("No data found");

            var output = ObjectMapper.Map<Core.AppAccounts.AppAccount>(input);
            output.TenantId = account.TenantId;
            output.UniqueId = account.UniqueId;
            output.Address.UniqueId = account.Address.UniqueId;
            output.Address.StateId = state.Id;
            output.Address.CountryId = country.Id;
            await _accountRepository.UpdateAsync(output);
        }

    }
}
