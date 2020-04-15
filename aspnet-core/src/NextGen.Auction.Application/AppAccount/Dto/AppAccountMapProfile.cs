using AutoMapper;
using NextGen.Auction.Account.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.Auction.AppAccount.Dto
{
    public class AppAccountMapProfile :Profile
    {
        public AppAccountMapProfile()
        {
            CreateMap<AppAccountDto, AppAccounts.AppAccount>();
        }
    }
}
