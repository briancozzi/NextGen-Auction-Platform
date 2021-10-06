using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NextGen.BiddingPlatform.AuctionItem.Dto;
using NextGen.BiddingPlatform.Caching;
using NextGen.BiddingPlatform.Configuration;
using NextGen.BiddingPlatform.Core.AppAccountEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using static NextGen.BiddingPlatform.Enums.ItemEnums;

namespace NextGen.BiddingPlatform.AuctionItem
{
    public class AuctionItemAppService : BiddingPlatformAppServiceBase, IAuctionItemAppService
    {
        private readonly IRepository<Core.AuctionItems.AuctionItem> _auctionitemRepository;
        private readonly IRepository<Core.Auctions.Auction> _auctionRepository;
        private readonly IRepository<Core.Items.Item> _itemRepository;
        private readonly IRepository<Core.AuctionBidders.AuctionBidder> _auctionBidderRepository;
        private readonly IRepository<Core.AuctionHistories.AuctionHistory> _auctionHistoryRepository;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<Event> _eventRepository;
        public AuctionItemAppService(IRepository<Core.AuctionItems.AuctionItem> auctionitemRepository,
                                     IRepository<Core.Auctions.Auction> auctionRepository,
                                     IRepository<Core.Items.Item> itemRepository,
                                     IAbpSession abpSession,
                                     IRepository<Core.AuctionBidders.AuctionBidder> auctionBidderRepository,
                                     IRepository<Core.AuctionHistories.AuctionHistory> auctionHistoryRepository,
                                     IRepository<Event> eventRepository)
        {
            _auctionitemRepository = auctionitemRepository;
            _auctionRepository = auctionRepository;
            _itemRepository = itemRepository;
            _abpSession = abpSession;
            _auctionBidderRepository = auctionBidderRepository;
            _auctionHistoryRepository = auctionHistoryRepository;
            _eventRepository = eventRepository;
        }
        [AllowAnonymous]
        public async Task<ListResultDto<AuctionItemListDto>> GetAllAuctionItems(int categoryId = 0, string search = "")
        {
            var query = _auctionitemRepository.GetAll()
                                                //.GetAllIncluding(x => x.Auction, x => x.Item, x => x.AuctionHistories)
                                                .Include(s => s.Auction)
                                                .Include(s => s.Item).ThenInclude(s => s.CategoryFk)
                                                .Include(s => s.AuctionHistories)
                                                    .AsNoTracking();

            if (categoryId > 0)
                query = query.Where(s => s.Item.CategoryId == categoryId);
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(s => s.Item.ItemName.ToLower().IndexOf(search) > -1 || s.Item.ItemNumber.ToString().IndexOf(search) > -1);
            }

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
                MainImageName = s.Item.MainImageName,
                ThumbnailImage = s.Item.ThumbnailImage,
                IsBiddingStarted = (s.Auction.AuctionStartDateTime - DateTime.UtcNow).TotalSeconds <= 0,
                IsAuctionExpired = (s.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalHours <= 0,
                RemainingDays = Convert.ToInt32((s.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalDays).ToString(),
                RemainingTime = Convert.ToInt32((s.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalHours) + ":" + Convert.ToInt32((s.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalMinutes),
                LastBidAmount = s.AuctionHistories.OrderByDescending(x => x.CreationTime).FirstOrDefault().BidAmount,
                IsClosedItemStatus = s.Item.ItemStatus == (int)ItemStatus.Closed,
                IsFavorite = "",
                ActualItemId = s.ItemId,
                IsHide = s.Item.IsHide,
                IsActive = s.Item.IsActive,
                CategoryId = s.Item.CategoryFk.Id,
                UniqueCategoryId = s.Item.CategoryFk.UniqueId
            }).ToListAsync();

            return new ListResultDto<AuctionItemListDto>(auctionItems);
        }
        public async Task<PagedResultDto<AuctionItemListDto>> GetAuctionItemsWithFilter(AuctionItemFilter input)
        {
            var query = _auctionitemRepository.GetAllIncluding(x => x.Auction, x => x.Item).AsNoTracking()
                                       .WhereIf(!input.Search.IsNullOrWhiteSpace(), x => x.Auction.AuctionType.ToLower().IndexOf(input.Search.ToLower()) > -1)
                                       .Select(s => new AuctionItemListDto
                                       {
                                           AuctionItemId = s.UniqueId,
                                           AuctionId = s.Auction.UniqueId,
                                           AuctionEndDateTime = s.Auction.AuctionEndDateTime,
                                           AuctionStartDateTime = s.Auction.AuctionStartDateTime,
                                           AuctionType = s.Auction.AuctionType,
                                           ItemId = s.Item.UniqueId,
                                           ItemName = s.Item.ItemName,
                                           ItemNumber = s.Item.ItemNumber,
                                           ItemType = s.Item.ItemType,
                                           FairMarketValue_FMV = s.Item.FairMarketValue_FMV,
                                           MainImageName = s.Item.MainImageName,
                                           ThumbnailImage = s.Item.ThumbnailImage,
                                       });

            var resultCount = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);

            var resultQuery = query.PageBy(input).ToList();

            return new PagedResultDto<AuctionItemListDto>(resultCount, resultQuery);
        }
        public async Task<ListResultDto<AuctionItemListDto>> GetAuctionItemsByAuctionId(Guid auctionId)
        {
            var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionId);
            if (auction == null)
                throw new Exception("Auction not available for give auction id");

            var auctionItems = await _auctionitemRepository.GetAllIncluding(x => x.Auction, x => x.Item)
                                                            .Where(x => x.AuctionId == auction.Id)
                                                            .Select(s => new AuctionItemListDto
                                                            {
                                                                AuctionId = s.Auction.UniqueId,
                                                                AuctionEndDateTime = s.Auction.AuctionEndDateTime,
                                                                AuctionStartDateTime = s.Auction.AuctionStartDateTime,
                                                                AuctionType = s.Auction.AuctionType,
                                                                ItemId = s.Item.UniqueId,
                                                                ItemName = s.Item.ItemName,
                                                                ItemNumber = s.Item.ItemNumber,
                                                                ItemType = s.Item.ItemType
                                                            })
                                                            .ToListAsync();

            return new ListResultDto<AuctionItemListDto>(auctionItems);
        }
        public async Task<CreateAuctionItemDto> GetAuctionItemById(Guid Id)
        {
            var output = await _auctionitemRepository.GetAllIncluding(x => x.Item, x => x.Auction).FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (output == null)
                throw new UserFriendlyException("Auction Item not found for given id");

            return ObjectMapper.Map<CreateAuctionItemDto>(output);
        }

        [AllowAnonymous]
        public async Task<AuctionItemListDto> GetAuctionItem(Guid Id)
        {
            try
            {
                var output = await _auctionitemRepository.GetAllIncluding(x => x.Item, x => x.Auction, x => x.AuctionHistories).AsNoTracking().FirstOrDefaultAsync(x => x.UniqueId == Id);
                if (output == null)
                    throw new UserFriendlyException("Auction Item not found for given id");
                var lastBidAmount = output.AuctionHistories.OrderByDescending(x => x.CreationTime).FirstOrDefault()?.BidAmount ?? 0;
                return new AuctionItemListDto
                {
                    AuctionItemId = output.UniqueId,
                    AuctionId = output.Auction.UniqueId,
                    AuctionEndDateTime = output.Auction.AuctionEndDateTime,
                    AuctionStartDateTime = output.Auction.AuctionStartDateTime,
                    AuctionType = output.Auction.AuctionType,
                    ItemId = output.Item.UniqueId,
                    ItemName = output.Item.ItemName,
                    ItemNumber = output.Item.ItemNumber,
                    ItemType = output.Item.ItemType,
                    FairMarketValue_FMV = output.Item.FairMarketValue_FMV,
                    MainImageName = output.Item.MainImageName,
                    ThumbnailImage = output.Item.ThumbnailImage,
                    IsBiddingStarted = (output.Auction.AuctionStartDateTime - DateTime.UtcNow).TotalSeconds <= 0,
                    IsAuctionExpired = (output.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalHours <= 0,
                    RemainingDays = Convert.ToInt32((output.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalDays).ToString(),
                    RemainingTime = Convert.ToInt32((output.Auction.AuctionEndDateTime - DateTime.UtcNow).Hours) + ":" + Convert.ToInt32((output.Auction.AuctionEndDateTime - DateTime.UtcNow).Minutes),
                    LastBidAmount = lastBidAmount,
                    TotalBidCount = output.AuctionHistories.Count,
                    ItemDescription = output.Item.Description,
                    IsActive = output.Item.IsActive,
                    IsHide = output.Item.IsHide
                };
            }
            catch (Exception ex)
            {
                throw;
            }

            //return ObjectMapper.Map<AuctionItemListDto>(output);
        }

        [AllowAnonymous]
        public async Task<AuctionItemWithHistoryDto> GetAuctionItemWithHistory(Guid Id, int itemStatus, long userId)
        {
            try
            {
                var output = await _auctionitemRepository.
                                                                    GetAllIncluding(
                                                                     x => x.Item,
                                                                     x => x.Auction,
                                                                     x => x.AuctionHistories)
                                                                    .Include($"{nameof(Core.AuctionItems.AuctionItem.AuctionHistories)}.{nameof(Core.AuctionHistories.AuctionHistory.AuctionBidder)}").AsNoTracking().FirstOrDefaultAsync(x => x.UniqueId == Id && x.Item.ItemStatus == itemStatus);
                if (output == null)
                    throw new UserFriendlyException("Auction Item not found for given id and status");


                var isBiddingStarted = (output.Auction.AuctionStartDateTime - DateTime.UtcNow).TotalSeconds <= 0;

                var result = new AuctionItemWithHistoryDto
                {
                    AuctionItemId = output.UniqueId,
                    AuctionId = output.Auction.UniqueId,
                    AuctionEndDateTime = output.Auction.AuctionEndDateTime,
                    AuctionStartDateTime = output.Auction.AuctionStartDateTime,
                    AuctionType = output.Auction.AuctionType,
                    ItemId = output.Item.UniqueId,
                    ItemName = output.Item.ItemName,
                    ItemNumber = output.Item.ItemNumber,
                    ItemType = output.Item.ItemType,
                    FairMarketValue_FMV = output.Item.FairMarketValue_FMV,
                    ImageName = output.Item.MainImageName,
                    Thumbnail = output.Item.ThumbnailImage,
                    RemainingDays = (output.Auction.AuctionEndDateTime - output.Auction.AuctionStartDateTime).TotalDays.ToString(),
                    RemainingTime = (output.Auction.AuctionEndDateTime - output.Auction.AuctionStartDateTime).Hours + ":" + (output.Auction.AuctionEndDateTime - output.Auction.AuctionStartDateTime).Seconds,
                    TotalBidCount = output.AuctionHistories.Count,
                    ItemDescription = output.Item.Description,
                    //BidStepIncrementValue = output.Item.BidStepIncrementValue,
                    AuctionItemHistories = output.AuctionHistories.OrderByDescending(x => x.CreationTime).Take(10).Select(s => new AuctionItemHistoryDto
                    {
                        BidderName = s.AuctionBidder.BidderName,
                        BidAmount = s.BidAmount,
                        BiddingDate = s.CreationTime.ToString("MM/dd/yyyy"),
                        BiddingTime = s.CreationTime.ToString("hh:mm tt"),
                        BidDate = s.CreationTime,
                        AuctionBidderUserId = s.AuctionBidder.UserId,
                        AuctionBidderId = s.AuctionBidderId
                    }).ToList(),
                    IsBiddingStarted = isBiddingStarted
                };
                result.LastBidAmount = result.AuctionItemHistories.OrderByDescending(x => x.BidDate).FirstOrDefault()?.BidAmount ?? 0;
                result.LastBidWinnerName = result.AuctionItemHistories.OrderByDescending(x => x.BidDate).FirstOrDefault()?.BidderName ?? string.Empty;

                //var totalBidAmount = result.AuctionItemHistories.Sum(x => x.BidAmount);
                result.BidStepIncrementValue = Helper.Helper.GetNextBidAmount(result.LastBidAmount);
                //var currntUseid = 3;
                if (userId > 0)
                {
                    result.CurrentUserAuctionHistoryCount = output.AuctionHistories.Count(x => x.AuctionBidder.UserId == userId);
                    result.CurrUserBidderName = output.AuctionHistories.FirstOrDefault(x => x.AuctionBidder.UserId == userId)?.AuctionBidder?.BidderName;
                    result.CurrUserBiddingId = output.AuctionHistories.FirstOrDefault(x => x.AuctionBidder.UserId == userId)?.AuctionBidderId ?? 0;
                    if (string.IsNullOrWhiteSpace(result.CurrUserBidderName))
                    {
                        var auctionBidder = await _auctionBidderRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(s => s.AuctionId == output.Auction.Id && s.UserId == userId);
                        if (auctionBidder != null)
                        {
                            result.CurrUserBiddingId = auctionBidder.Id;
                            result.CurrUserBidderName = auctionBidder.BidderName;
                        }
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AbpAuthorize]
        public async Task<AuctionItemDto> Create(CreateAuctionItemDto input)
        {
            var item = await ValidateItemAndGet(input);
            var auction = await ValidateAuctionAndGet(input);

            //var existAuctionItem = await _auctionitemRepository.FirstOrDefaultAsync(x => x.AuctionId == auction.Id && x.ItemId == item.Id);

            var existAuctionItem = await _auctionitemRepository.FirstOrDefaultAsync(x => x.ItemId == item.Id);

            if (existAuctionItem != null)
                throw new UserFriendlyException("Auction item already exist");

            var auctionItem = await _auctionitemRepository.InsertAsync(new Core.AuctionItems.AuctionItem
            {
                UniqueId = Guid.NewGuid(),
                TenantId = _abpSession.TenantId.Value,
                AuctionId = auction.Id,
                ItemId = item.Id,
                IsActive = input.IsActive
            });

            return new AuctionItemDto
            {
                UniqueId = auctionItem.UniqueId,
                AuctionId = input.AuctionId,
                ItemId = input.ItemId,
            };
        }
        private async Task<Core.Auctions.Auction> ValidateAuctionAndGet(CreateAuctionItemDto input)
        {
            var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AuctionId);
            if (auction == null)
                throw new UserFriendlyException("Auction not found by given Id");
            else
                return auction;
        }
        private async Task<Core.Items.Item> ValidateItemAndGet(CreateAuctionItemDto input)
        {
            var item = await _itemRepository.FirstOrDefaultAsync(x => x.UniqueId == input.ItemId);
            if (item == null)
                throw new UserFriendlyException("Item not found by given id");
            else
                return item;
        }
        [AbpAuthorize]
        public async Task<AuctionItemDto> Update(CreateAuctionItemDto input)
        {
            if (input.UniqueId == Guid.Empty)
                throw new UserFriendlyException("Invalid id!!.");

            var auctionItem = await _auctionitemRepository.FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (auctionItem == null)
                throw new UserFriendlyException("Auction item not found!!.");

            var item = await ValidateItemAndGet(input);
            var auction = await ValidateAuctionAndGet(input);

            auctionItem.AuctionId = auction.Id;
            auctionItem.ItemId = item.Id;
            auctionItem.LastModificationTime = Clock.Now.ToUniversalTime();
            auctionItem.LastModifierUserId = _abpSession.UserId;
            await _auctionitemRepository.UpdateAsync(auctionItem);
            return new AuctionItemDto
            {
                UniqueId = auctionItem.UniqueId,
                AuctionId = input.AuctionId,
                ItemId = input.ItemId,
            };
        }
        public async Task CreateAuctionItems(List<CreateAuctionItemDto> auctionItems)
        {
            var items = _itemRepository.GetAll();
            var auctionId = auctionItems.FirstOrDefault();

            var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionId.AuctionId);
            if (auction == null)
                throw new Exception("Auction not available for give auction id");

            foreach (var item in auctionItems)
            {
                var currentItem = items.FirstOrDefault(x => x.UniqueId == item.ItemId);
                if (currentItem == null)
                    throw new Exception("Item not available for given id");

                await _auctionitemRepository.InsertAsync(new Core.AuctionItems.AuctionItem
                {
                    UniqueId = Guid.NewGuid(),
                    TenantId = _abpSession.TenantId.Value,
                    AuctionId = auction.Id,
                    ItemId = currentItem.Id,
                    IsActive = item.IsActive
                });
            }
        }
        [AbpAuthorize]
        public async Task Delete(Guid Id)
        {
            var auctionItem = await _auctionitemRepository.FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (auctionItem == null)
                throw new Exception("AuctionItem not found for given Id");

            await _auctionitemRepository.DeleteAsync(auctionItem);
        }

        public async Task<List<AuctionItemWithHistoryDto>> GetUsersBiddingHistory(long userId)
        {
            var auctionBidderIds = await _auctionBidderRepository.GetAll().AsNoTracking()
                .Where(s => s.UserId == userId).Select(s => new { AuctionBidderId = s.Id, UserId = s.UserId, AuctionId = s.AuctionId }).ToListAsync();

            var auctionItemIdsFromAuctionHistory = await _auctionHistoryRepository.GetAll().AsNoTracking().Where(s => auctionBidderIds.Select(x => x.AuctionBidderId).Contains(s.AuctionBidderId)).Select(x => x.AuctionItemId).Distinct().ToListAsync();

            var auctionItems = await _auctionitemRepository.GetAll()
                .Where(x => auctionItemIdsFromAuctionHistory.Contains(x.Id))
                .AsNoTracking()
                .Include(s => s.Item)
                .Include(s => s.Auction)
                .Include(s => s.AuctionHistories)
                .ToListAsync();


            List<AuctionItemWithHistoryDto> output = new List<AuctionItemWithHistoryDto>();
            foreach (var item in auctionItems)
            {
                var auctionId = item.Auction.Id;
                var auctionBidderIdForCurrentItem = auctionBidderIds.FirstOrDefault(s => s.UserId == userId && s.AuctionId == auctionId);

                var finalAuctionItem = new AuctionItemWithHistoryDto
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
                };

                double currUserLastBid = 0;
                if (auctionBidderIdForCurrentItem != null)
                {

                    currUserLastBid = item.AuctionHistories.OrderByDescending(x => x.CreationTime).FirstOrDefault(s => s.AuctionBidderId == auctionBidderIdForCurrentItem.AuctionBidderId && s.AuctionItemId == item.Id)?.BidAmount ?? 0;

                    var lastAuctionHistory = item.AuctionHistories.OrderByDescending(x => x.CreationTime).FirstOrDefault();

                    if (lastAuctionHistory != null)
                        finalAuctionItem.IsLastBidByCurrentUser = lastAuctionHistory.AuctionBidderId == auctionBidderIdForCurrentItem.AuctionBidderId && lastAuctionHistory.AuctionItemId == item.Id;
                }

                finalAuctionItem.LastBidAmount = currUserLastBid;
                output.Add(finalAuctionItem);
            }
            return output;
        }

        public async Task<List<EventAuctionItemWinnerDto>> GetEventWinners(Guid eventUniqueId)
        {
            var @event = await _eventRepository.FirstOrDefaultAsync(s => s.UniqueId == eventUniqueId);
            if (@event == null)
                throw new UserFriendlyException("Event not found!!");

            var auctionIds = await _auctionRepository.GetAll().AsNoTracking().Where(s => s.EventId == @event.Id).Select(s => s.Id).ToListAsync();

            var auctionItems = await _auctionitemRepository.GetAll().AsNoTracking()
                                    .Include(s => s.Item)
                                    .Include(s => s.Auction)
                                    .Include(s => s.AuctionHistories)
                                    .Where(s => auctionIds.Contains(s.AuctionId)).ToListAsync();

            List<EventAuctionItemWinnerDto> result = new List<EventAuctionItemWinnerDto>();

            foreach (var s in auctionItems)
            {
                var auctionItem = new EventAuctionItemWinnerDto
                {
                    ItemName = s.Item.ItemName,
                    ItemPrice = (decimal)Math.Round(s.Item.FairMarketValue_FMV, 2),
                    AuctionStatus = "In Progress"
                };

                if ((s.Auction.AuctionEndDateTime - DateTime.UtcNow).TotalHours <= 0)
                {
                    auctionItem.AuctionStatus = "Winner";
                    //status inprogress
                    var maxBidBidder = s.AuctionHistories.OrderByDescending(s => s.CreationTime).ThenByDescending(s => s.BidAmount).FirstOrDefault();
                    if (maxBidBidder != null)
                    {
                        var bidderDetails = await _auctionBidderRepository.GetAll().AsNoTracking().Include(s => s.User).FirstOrDefaultAsync(s => s.Id == maxBidBidder.AuctionBidderId);

                        auctionItem.LastBiddingAmountOfWinner = (decimal)Math.Round(maxBidBidder.BidAmount, 2);
                        auctionItem.WinnerName = bidderDetails.BidderName;
                    }
                }

                result.Add(auctionItem);
            }

            return result;
        }
    }
}
