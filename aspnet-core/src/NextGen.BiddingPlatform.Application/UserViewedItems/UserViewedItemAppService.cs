using Abp.Application.Services.Dto;
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

        public async Task<ListResultDto<AuctionItemListDto>> GetUserViewedItems(long userId, int? tenantId)
        {
            using (CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant))
            {
                var itemIds = await _userViewedItemRepository.GetAll()
                                .AsNoTracking().Where(s => s.UserId == userId && s.TenantId == tenantId).Select(s => s.ItemId).ToListAsync();


                var query = _auctionItemRepository.GetAll().AsNoTracking()
                                               .Where(x => itemIds.Contains(x.ItemId))
                                               .Include(s => s.Item)
                                               .Include(s => s.Auction)
                                               .Include(s => s.AuctionHistories);

                var auctionItems = await query.Select(s => new AuctionItemListDto
                {
                    AuctionItemId = s.UniqueId,
                    AuctionId = s.Auction.UniqueId,
                    AuctionEndDateTime = s.Auction.AuctionEndDateTime,
                    AuctionStartDateTime = s.Auction.AuctionStartDateTime,
                    AuctionType = s.Auction.AuctionType,
                    ItemId = s.Item.UniqueId,
                    ItemName = s.Item.ItemName,
                    ItemNumber = s.Item.ItemNumber,
                    ItemStatus = s.Item.ItemStatus,
                    ItemType = s.Item.ItemType,
                    FairMarketValue_FMV = s.Item.FairMarketValue_FMV,
                    ImageName = s.Item.MainImageName,
                    Thumbnail = s.Item.ThumbnailImage,
                    IsAuctionExpired = (s.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalHours <= 0,
                    RemainingDays = Convert.ToInt32((s.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalDays).ToString(),
                    RemainingTime = Convert.ToInt32((s.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalHours) + ":" + Convert.ToInt32((s.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalMinutes),
                    LastBidAmount = s.AuctionHistories.OrderByDescending(x => x.CreationTime).FirstOrDefault().BidAmount,
                    IsClosedItemStatus = s.Item.ItemStatus == (int)ItemStatus.Closed,
                    IsFavorite = "",
                    ActualItemId = s.ItemId
                }).ToListAsync();

                return new ListResultDto<AuctionItemListDto>(auctionItems);
            }
        }
    }
}
