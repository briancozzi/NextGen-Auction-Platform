using Abp.Application.Services;
using Abp.Domain.Repositories;
using NextGen.Auction.Account.Dto;
using NextGen.Auction.AppAccounts;
using System;
namespace NextGen.Auction.Account
{
    public class AppAccountAppService : AsyncCrudAppService<AppAccounts.AppAccount, AppAccountDto, Guid>, IAppAccountAppService
    {
        public AppAccountAppService(IRepository<AppAccounts.AppAccount, Guid> accountRepository) : base(accountRepository)
        {
        }
    }
}
