using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Core.Items;
using NextGen.BiddingPlatform.UserfavoriteItems.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.UserfavoriteItems
{
    public class UserFavoriteItemAppService : BiddingPlatformAppServiceBase, IUserFavoriteItemAppService
    {
        private readonly IRepository<UserFavoriteItem, long> _userFavoriteItemRepository;
        private readonly IRepository<Core.AuctionItems.AuctionItem> _auctionitemRepository;
        public UserFavoriteItemAppService(IRepository<UserFavoriteItem, long> userFavoriteItemRepository,
            IRepository<Core.AuctionItems.AuctionItem> auctionitemRepository)
        {
            _userFavoriteItemRepository = userFavoriteItemRepository;
            _auctionitemRepository = auctionitemRepository;
        }

        public async Task SetItemAsFavoriteOrUnFavorite(CreateOrEditFavoriteItemDto input)
        {
            var item = await _auctionitemRepository.GetAll().FirstOrDefaultAsync(s => s.UniqueId == input.ItemId);

            var userFavoriteItem = await _userFavoriteItemRepository.GetAll().FirstOrDefaultAsync(s => s.UserId == input.UserId && s.ItemId == item.ItemId);


            if (userFavoriteItem == null && input.IsFavorite)
            {

                await _userFavoriteItemRepository.InsertAsync(new UserFavoriteItem
                {
                    UserId = input.UserId,
                    ItemId = item.ItemId,
                    TenantId = AbpSession.TenantId,
                    CreationTime = DateTime.Now
                });
            }
            else
            {
                if (userFavoriteItem == null)
                    throw new UserFriendlyException("User favorite item not found!!");

                await _userFavoriteItemRepository.DeleteAsync(userFavoriteItem);
            }
        }

        public async Task GetUserFavoriteItems()
        {
            var userFavoriteItems = await _userFavoriteItemRepository.GetAll().AsNoTracking().Include(s => s.ItemFk).ToListAsync();
        }
    }
}
