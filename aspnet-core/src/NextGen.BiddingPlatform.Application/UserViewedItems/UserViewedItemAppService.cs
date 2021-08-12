using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.UserfavoriteItems;
using NextGen.BiddingPlatform.UserViewedItems.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.UserViewedItems
{
    public class UserViewedItemAppService : BiddingPlatformAppServiceBase, IUserViewedItemAppService
    {
        private readonly IRepository<UserFavoriteItem, long> _userFavoriteItemRepository;
        private readonly IRepository<Core.AuctionItems.AuctionItem> _auctionItemRepository;
        public UserViewedItemAppService(IRepository<UserFavoriteItem, long> userFavoriteItemRepository,
                                          IRepository<Core.AuctionItems.AuctionItem> auctionItemRepository)
        {
            _userFavoriteItemRepository = userFavoriteItemRepository;
            _auctionItemRepository = auctionItemRepository;
        }

        public async Task AddViewedItem(CreateOrEditUserViewedItem input)
        {
            var auctionItem = await _auctionItemRepository.GetAll().FirstOrDefaultAsync(s => s.UniqueId == input.AuctionItemId);

            if (auctionItem == null)
                throw new UserFriendlyException("Item not found!!");

            var userFavoriteItem = await _userFavoriteItemRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(s => s.UserId == input.UserId && s.ItemId == auctionItem.ItemId);

            if (userFavoriteItem == null)
            {
                await _userFavoriteItemRepository.InsertAsync(new UserFavoriteItem
                {
                    UserId = input.UserId,
                    ItemId = auctionItem.ItemId,
                    CreationTime = DateTime.UtcNow,
                    TenantId = AbpSession.TenantId
                });
            }
        }
    }
}
