using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using NextGen.Auction.AppAccounts;
using System;
using System.ComponentModel.DataAnnotations;

namespace NextGen.Auction.Account.Dto
{
    [AutoMapFrom(typeof(AppAccounts.AppAccount))]
    public class AppAccountDto : EntityDto<Guid>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public string Email { get; set; }
    }
}

