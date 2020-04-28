using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextGen.BiddingPlatform.AppAccount.Dto;
using NextGen.BiddingPlatform.Country;
using Abp.Runtime.Session;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Extensions;
using System.Linq.Dynamic.Core;
using IdentityServer4.Extensions;
using Abp.Linq.Extensions;
using NextGen.BiddingPlatform.DashboardCustomization.Dto;
using Abp.Authorization;
using NextGen.BiddingPlatform.Authorization;

namespace NextGen.BiddingPlatform.AppAccount
{
    [AbpAuthorize(AppPermissions.Pages_Tenant_AppAccount)]
    public class AppAccountAppService : BiddingPlatformDomainServiceBase, IAppAccountAppService
    {
        private readonly IRepository<Core.AppAccounts.AppAccount> _accountRepository;
        private readonly IRepository<Country.Country> _countryRepository;
        private readonly IRepository<Core.State.State> _stateRepository;
        private readonly IAbpSession AbpSession;
        public AppAccountAppService(IRepository<Core.AppAccounts.AppAccount> accountRepository,
                                    IRepository<Country.Country> countryRepository,
                                    IRepository<Core.State.State> stateRepository,
                                    IAbpSession abpSession)
        {
            _accountRepository = accountRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            AbpSession = abpSession;
        }

        public async Task<PagedResultDto<AppAccountListDto>> GetAllAccountFilter(AppAccountFilter input)
        {
            var query = _accountRepository.GetAll()
                                                    .WhereIf(!input.SearchName.IsNullOrWhiteSpace(), x => x.FirstName.ToLower().IndexOf(input.SearchName.ToLower()) > -1 || x.LastName.ToLower().IndexOf(input.SearchName.ToLower()) > -1)
                                                    .Select(x => new AppAccountListDto
                                                    {
                                                        UniqueId = x.UniqueId,
                                                        Email = x.Email,
                                                        FirstName = x.FirstName,
                                                        LastName = x.LastName,
                                                        PhoneNo = x.PhoneNo,
                                                        Logo = x.Logo
                                                    });

            var resultCount = await query.CountAsync();

            //FirstName ASC OR DESC
            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);

            query = query.PageBy(input);

            return new PagedResultDto<AppAccountListDto>(resultCount, await query.ToListAsync());
        }

        public async Task<CreateAppAccountDto> Create(CreateAppAccountDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (country == null || state == null)
                throw new Exception("Country or State not found");

            if (!AbpSession.TenantId.HasValue)
                throw new Exception("You are not Authorized user.");

            var account = ObjectMapper.Map<Core.AppAccounts.AppAccount>(input);
            account.UniqueId = Guid.NewGuid();
            account.Address.UniqueId = Guid.NewGuid();
            account.Address.StateId = state.Id;
            account.Address.CountryId = country.Id;
            account.TenantId = AbpSession.TenantId.Value;
            await _accountRepository.InsertAsync(account);
            return input;
        }

        public async Task<UpdateAppAccountDto> Update(UpdateAppAccountDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (country == null || state == null)
                throw new Exception("Country or State not found");

            var account = await _accountRepository.GetAllIncluding(x => x.Address).FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (account == null)
                throw new Exception("AppAccount not found for given Id");

            //AppAccount Properties
            account.FirstName = input.FirstName;
            account.LastName = input.LastName;
            account.Email = input.Email;
            account.PhoneNo = input.PhoneNo;
            account.Logo = input.Logo;
            account.IsActive = input.IsActive;
            //Address Properties
            account.Address.Address1 = input.Address.Address1;
            account.Address.Address2 = input.Address.Address2;
            account.Address.City = input.Address.City;
            account.Address.ZipCode = input.Address.ZipCode;
            account.Address.StateId = state.Id;
            account.Address.CountryId = country.Id;
            await _accountRepository.UpdateAsync(account);
            return input;
        }

        public async Task Delete(Guid Id)
        {
            var appAccount = await _accountRepository.FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (appAccount == null)
                throw new Exception("AppAccount not found for given Id");

            await _accountRepository.DeleteAsync(appAccount);
        }

        public async Task<ListResultDto<AppAccountListDto>> GetAllAccount()
        {
            var accountsData = await _accountRepository.GetAll().ToListAsync();
            return new ListResultDto<AppAccountListDto>(ObjectMapper.Map<List<AppAccountListDto>>(accountsData));
        }

        public async Task<AppAccountDto> GetAccountById(Guid Id)
        {
            var account = await _accountRepository.GetAllIncluding(x => x.Address, x => x.Address.State, x => x.Address.Country).FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (account == null)
                throw new Exception("AppAccount not found for given Id");

            return ObjectMapper.Map<AppAccountDto>(account);
        }
    }
}
