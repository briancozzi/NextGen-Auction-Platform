using Abp.Application.Services;
using NextGen.BiddingPlatform.UserfavoriteItems.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.UserfavoriteItems
{
    public interface IUserFavoriteItemAppService : IApplicationService
    {
        Task SetItemAsFavoriteOrUnFavorite(CreateOrEditFavoriteItemDto input);
        Task<List<GetUserFavoriteItemDto>> GetUserFavoriteItems(long userId);
    }
}
