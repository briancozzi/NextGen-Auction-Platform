using Abp.Application.Services;
using NextGen.BiddingPlatform.UserViewedItems.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.UserViewedItems
{
    public interface IUserViewedItemAppService : IApplicationService
    {
        Task AddViewedItem(CreateOrEditUserViewedItem input);
    }
}
