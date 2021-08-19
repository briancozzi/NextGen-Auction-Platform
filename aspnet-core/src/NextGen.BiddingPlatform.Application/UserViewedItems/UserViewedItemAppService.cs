using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.AuctionItem.Dto;
using NextGen.BiddingPlatform.UserfavoriteItems;
using NextGen.BiddingPlatform.UserViewedItems.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NextGen.BiddingPlatform.Enums.ItemEnums;

namespace NextGen.BiddingPlatform.UserViewedItems
{
    public class UserViewedItemAppService : BiddingPlatformAppServiceBase, IUserViewedItemAppService
    {
        private readonly IRepository<UserViewedItem, long> _userViewedItemRepository;
        private readonly IRepository<Core.AuctionItems.AuctionItem> _auctionItemRepository;
        public UserViewedItemAppService(IRepository<UserViewedItem, long> userViewedItemRepository,
                                        IRepository<Core.AuctionItems.AuctionItem> auctionItemRepository)
        {
            _userViewedItemRepository = userViewedItemRepository;
            _auctionItemRepository = auctionItemRepository;
        }

        public async Task AddViewedItem(CreateOrEditUserViewedItem input)
        {

            using (CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant))
            {
                var auctionItem = await _auctionItemRepository.GetAll().FirstOrDefaultAsync(s => s.UniqueId == input.AuctionItemId && s.TenantId == input.TenantId);

                if (auctionItem == null)
                    throw new UserFriendlyException("Item not found!!");

                var userFavoriteItem = await _userViewedItemRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(s => s.UserId == input.UserId && s.ItemId == auctionItem.ItemId && s.TenantId == input.TenantId);

                if (userFavoriteItem == null)
                {
                    await _userViewedItemRepository.InsertAsync(new UserViewedItem
                    {
                        UserId = input.UserId,
                        ItemId = auctionItem.ItemId,
                        CreationTime = DateTime.UtcNow,
                        TenantId = input.TenantId
                    });
                }
            }
        }

        public async Task<List<AuctionItemWithHistoryDto>> GetUserViewedItems(long userId, int? tenantId)
        {
            using (CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant))
            {
                var itemIds = await _userViewedItemRepository.GetAll()
                                .AsNoTracking().Where(s => s.UserId == userId && s.TenantId == tenantId).Select(s => s.ItemId).ToListAsync();

                var auctionItems = await _auctionItemRepository.GetAll().AsNoTracking()
                                               .Where(x => itemIds.Contains(x.ItemId))
                                               .Include(s => s.Item)
                                               .Include(s => s.Auction)
                                               .Include(s => s.AuctionHistories)
                                               .Select(item => new AuctionItemWithHistoryDto
                                               {
                                                   AuctionItemId = item.UniqueId,
                                                   ItemName = item.Item.ItemName,
                                                   AuctionEndDateTime = item.Auction.AuctionEndDateTime,
                                                   AuctionStartDateTime = item.Auction.AuctionStartDateTime,
                                                   ItemNumber = item.Item.ItemNumber,
                                                   ItemStatus = item.Item.ItemStatus,
                                                   ImageName = item.Item.MainImageName,
                                                   Thumbnail = item.Item.ThumbnailImage,
                                                   IsAuctionExpired = (item.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalHours <= 0,
                                                   RemainingDays = Convert.ToInt32((item.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalDays).ToString(),
                                                   RemainingTime = Convert.ToInt32((item.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalHours) + ":" + Convert.ToInt32((item.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalMinutes),
                                                   IsClosedItemStatus = item.Item.ItemStatus == (int)ItemStatus.Closed,
                                                   LastBidAmount = item.AuctionHistories.OrderByDescending(x => x.CreationTime).FirstOrDefault().BidAmount
                                               })
                                               .ToListAsync();

                return auctionItems;
            }
        }
    }
}
