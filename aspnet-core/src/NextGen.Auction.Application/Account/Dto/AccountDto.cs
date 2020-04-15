using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.Auction.Account.Dto
{
    [AutoMapFrom(typeof(Accounts.Account))]
    public class AccountDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public int TenantId { get; set; }//tenant as organization
        public string CountryCodeForMobile { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsMobileVerified { get; set; }
    }
}

