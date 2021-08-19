using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.AuctionItem.Dto;
using NextGen.BiddingPlatform.Core.Items;
using NextGen.BiddingPlatform.UserfavoriteItems.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NextGen.BiddingPlatform.Enums.ItemEnums;

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
            using (CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant))
            {
                var item = await _auctionitemRepository.GetAll().FirstOrDefaultAsync(s => s.UniqueId == input.ItemId && s.TenantId == input.TenantId);

                var userFavoriteItem = await _userFavoriteItemRepository.GetAll().FirstOrDefaultAsync(s => s.UserId == input.UserId && s.ItemId == item.ItemId && s.TenantId == input.TenantId);


                if (userFavoriteItem == null && input.IsFavorite)
                {

                    await _userFavoriteItemRepository.InsertAsync(new UserFavoriteItem
                    {
                        UserId = input.UserId,
                        ItemId = item.ItemId,
                        TenantId = input.TenantId,
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
        }

        public async Task<List<GetUserFavoriteItemDto>> GetUserFavoriteItems(long userId, int? tenantId)
        {
            using (CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant))
            {
                var userFavoriteItems = await _userFavoriteItemRepository.GetAll()
                                .AsNoTracking().Include(s => s.ItemFk).Where(s => s.UserId == userId && s.TenantId == tenantId).Select(s => new GetUserFavoriteItemDto
                                {
                                    UserId = s.UserId,
                                    ItemId = s.ItemId
                                }).ToListAsync();

                return userFavoriteItems;
            }
        }


        public async Task<List<AuctionItemWithHistoryDto>> GetUserWatchListItems(long userId, int? tenantId)
        {
            using (CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant))
            {
                var itemIds = await _userFavoriteItemRepository.GetAll()
                                .AsNoTracking().Where(s => s.UserId == userId && s.TenantId == tenantId).Select(s => s.ItemId).ToListAsync();

                var auctionItems = await _auctionitemRepository.GetAll().AsNoTracking()
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
