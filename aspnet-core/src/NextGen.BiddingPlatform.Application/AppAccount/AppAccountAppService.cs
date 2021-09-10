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
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.CustomAuthorization;
using NextGen.BiddingPlatform.Common;
using static NextGen.BiddingPlatform.CustomAuthorization.CustomEnum;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Abp.Authorization.Users;
using NextGen.BiddingPlatform.Authorization.Roles;

namespace NextGen.BiddingPlatform.AppAccount
{

    public class AppAccountAppService : BiddingPlatformAppServiceBase, IAppAccountAppService
    {
        private readonly IRepository<Core.AppAccounts.AppAccount> _accountRepository;
        private readonly IRepository<Country.Country> _countryRepository;
        private readonly IRepository<Core.State.State> _stateRepository;
        public readonly IUserAppService _userAppService;
        public AppAccountAppService(IRepository<Core.AppAccounts.AppAccount> accountRepository,
                                    IRepository<Country.Country> countryRepository,
                                    IRepository<Core.State.State> stateRepository,
                                    IUserAppService userAppService)
        {
            _accountRepository = accountRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _userAppService = userAppService;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_AppAccount)]
        public async Task<PagedResultDto<AppAccountListDto>> GetAllAccountFilter(AppAccountFilter input)
        {
            var query = _accountRepository.GetAll();


            var resultquery = query.WhereIf(!input.SearchName.IsNullOrWhiteSpace(), x => x.FirstName.ToLower().IndexOf(input.SearchName.ToLower()) > -1 || x.LastName.ToLower().IndexOf(input.SearchName.ToLower()) > -1)
                         .Select(x => new AppAccountListDto
                         {
                             UniqueId = x.UniqueId,
                             Email = x.Email,
                             FirstName = x.FirstName,
                             LastName = x.LastName,
                             PhoneNo = x.PhoneNo,
                             Logo = x.Logo,
                             ThumbnailImage = x.ThumbnailImage
                         });

            var resultCount = await resultquery.CountAsync();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
                resultquery = resultquery.OrderBy(input.Sorting);

            resultquery = resultquery.PageBy(input);

            return new PagedResultDto<AppAccountListDto>(resultCount, resultquery.ToList());
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_AppAccount, AppPermissions.Pages_Administration_Tenant_AppAccount_Create, RequireAllPermissions = true)]
        public async Task<CreateAppAccountDto> Create(CreateAppAccountDto input)
        {
            try
            {
                var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
                var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
                if (country == null || state == null)
                    throw new UserFriendlyException("Country or State not found");

                if (!AbpSession.TenantId.HasValue)
                    throw new UserFriendlyException("You are not Authorized user.");

                var account = ObjectMapper.Map<Core.AppAccounts.AppAccount>(input);
                account.UniqueId = Guid.NewGuid();
                account.Address.UniqueId = Guid.NewGuid();
                account.Address.StateId = state.Id;
                account.Address.CountryId = country.Id;
                account.TenantId = AbpSession.TenantId.Value;

                var appAccount = await _accountRepository.InsertAsync(account);

                await CurrentUnitOfWork.SaveChangesAsync();
                await _userAppService.CreateOrUpdateUser(new Authorization.Users.Dto.CreateOrUpdateUserInput
                {
                    User = new Authorization.Users.Dto.UserEditDto
                    {
                        EmailAddress = input.Email,
                        IsActive = true,
                        Name = input.FirstName,
                        PhoneNumber = input.PhoneNo,
                        Surname = input.LastName,
                        Password = "123qwe",
                        ShouldChangePasswordOnNextLogin = true,
                        UserName = input.Email,
                        AppAccountId = appAccount.Id
                    },
                    SetRandomPassword = false,
                    AssignedRoleNames = new List<string>() { StaticRoleNames.Tenants.AccountAdmin }.ToArray()
                });

                input.AppAccountUniqueId = account.UniqueId;
                return input;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_AppAccount, AppPermissions.Pages_Administration_Tenant_AppAccount_Edit, RequireAllPermissions = true)]
        public async Task<UpdateAppAccountDto> Update(UpdateAppAccountDto input)
        {
            var account = await _accountRepository.GetAllIncluding(x => x.Address, x => x.AccountPermissions).FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (account == null)
                throw new UserFriendlyException("AppAccount not found for given Id");

            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (country == null || state == null)
                throw new UserFriendlyException("Country or State not found");

            var accountUser = await UserManager.FindByNameOrEmailAsync(account.Email);

            //AppAccount Properties
            account.FirstName = input.FirstName;
            account.LastName = input.LastName;
            account.Email = input.Email;
            account.PhoneNo = input.PhoneNo;
            account.Logo = input.Logo;
            account.ThumbnailImage = input.ThumbnailImage;
            account.IsActive = input.IsActive;
            //Address Properties
            account.Address.Address1 = input.Address.Address1;
            account.Address.Address2 = input.Address.Address2;
            account.Address.City = input.Address.City;
            account.Address.ZipCode = input.Address.ZipCode;
            account.Address.StateId = state.Id;
            account.Address.CountryId = country.Id;

            await _accountRepository.UpdateAsync(account);
            var userRoles = await UserManager.GetRolesAsync(accountUser);
            await _userAppService.CreateOrUpdateUser(new Authorization.Users.Dto.CreateOrUpdateUserInput
            {
                User = new Authorization.Users.Dto.UserEditDto
                {
                    Id = accountUser.Id,
                    EmailAddress = input.Email,
                    AppAccountId = accountUser.AppAccountId,
                    IsActive = accountUser.IsActive,
                    IsLockoutEnabled = accountUser.IsLockoutEnabled,
                    IsTwoFactorEnabled = accountUser.IsTwoFactorEnabled,
                    Name = input.FirstName,
                    PhoneNumber = input.PhoneNo,
                    ShouldChangePasswordOnNextLogin = accountUser.ShouldChangePasswordOnNextLogin,
                    Surname = input.LastName,
                    UserName = accountUser.UserName
                },
                AssignedRoleNames = userRoles.ToArray()
            });
            return input;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_AppAccount, AppPermissions.Pages_Administration_Tenant_AppAccount_Delete, RequireAllPermissions = true)]
        public async Task Delete(Guid Id)
        {
            var appAccount = await _accountRepository.FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (appAccount == null)
                throw new Exception("AppAccount not found for given Id");

            await _accountRepository.DeleteAsync(appAccount);

            var user = await UserManager.Users.FirstOrDefaultAsync(s => s.AppAccountId == appAccount.Id);
            if (user != null)
            {
                user.IsDeleted = true;
                user.LastModificationTime = DateTime.Now;
                user.LastModifierUserId = AbpSession.UserId;
                await UserManager.UpdateAsync(user);
            }
        }

        public async Task<ListResultDto<AppAccountListDto>> GetAllAccount()
        {
            //var accountIds = await GetAccessibleIds(AccessType.List, null);
            var currUser = await UserManager.GetUserByIdAsync(AbpSession.UserId.Value);
            var query = _accountRepository.GetAll();
            if (currUser.AppAccountId.HasValue)
                query = query.Where(x => x.Id == currUser.AppAccountId);

            var result = await query.ToListAsync();

            return new ListResultDto<AppAccountListDto>(ObjectMapper.Map<List<AppAccountListDto>>(result));
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_AppAccount)]
        public async Task<AppAccountDto> GetAccountById(Guid Id)
        {
            var account = await _accountRepository.GetAllIncluding(x => x.Address, x => x.Address.State, x => x.Address.Country, x => x.AccountPermissions).FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (account == null)
                throw new Exception("AppAccount not found for given Id");



            var mappedAccount = ObjectMapper.Map<AppAccountDto>(account);
            // mappedAccount.Users = account.AccountPermissions.Select(x => x.UserId).ToList();
            return mappedAccount;
        }

        //private async Task<List<int>> GetAccessibleIds(AccessType accessType, int? accountId)
        //{
        //    List<int> accountIds = null;
        //    var userPermissions = await _commonPermissionService.GetUserPermissions();

        //    if (userPermissions.Contains(AppPermissions.Pages_Administration_Tenant_AppAccount_All))
        //    {
        //        if (accessType == AccessType.List)
        //            return null;

        //        accountIds = new List<int>();
        //        accountIds.Add(accountId ?? 0);
        //        return accountIds;
        //    }

        //    accountIds = new List<int>();
        //    if (userPermissions.Contains(AppPermissions.Pages_Administration_Tenant_AppAccount_Assign))
        //    {
        //        if (accessType == AccessType.List)
        //            accountIds = await GetAssignedAccounts(AbpSession.UserId.Value);
        //        else if (accessType == AccessType.Get || accessType == AccessType.Edit || accessType == AccessType.Delete)
        //        {
        //            var hasAccess = await IsAccountAccessible(AbpSession.UserId.Value, accountId.Value);
        //            if (hasAccess)
        //                accountIds.Add(accountId.Value);
        //        }
        //        return accountIds;
        //    }

        //    return accountIds;
        //}

        //private async Task<List<int>> GetAssignedAccounts(long userId)
        //{
        //    var user = await UserManager.GetUserByIdAsync(userId);
        //    if (user == null)
        //        throw new Exception("User not found for given id");

        //    var selfAccounts = await _accountRepository.GetAllListAsync(x => x.CreatorUserId == userId);

        //    var filterAccount = await _accounPermissionRepository.GetAllListAsync(x => x.UserId == userId);

        //    var selfAccountIds = selfAccounts.Select(x => x.Id);
        //    var filterIds = filterAccount.Select(x => x.AppAccountId);

        //    selfAccountIds = selfAccountIds.Union(filterIds);

        //    return selfAccountIds.ToList();
        //}

        //private async Task<bool> IsAccountAccessible(long userId, int accountId)
        //{
        //    var selfAccounts = await _accountRepository.FirstOrDefaultAsync(x => x.CreatorUserId == userId && x.Id == accountId);

        //    var filterAccount = await _accounPermissionRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.AppAccountId == accountId);

        //    return selfAccounts != null || filterAccount != null;
        //}

    }
}
