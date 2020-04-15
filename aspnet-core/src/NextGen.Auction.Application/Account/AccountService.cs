using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using NextGen.Auction.Account.Dto;
using NextGen.Auction.MultiTenancy.Dto;
using NextGen.Auction.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Auction.Account
{
    public class MyAccountAppService : CrudAppService<Accounts.Account, AccountDto, Guid>, IMyAccountAppService
    {
        public MyAccountAppService(IRepository<Accounts.Account, Guid> accountRepository) : base(accountRepository)
        {
        }
    }
}
