using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.UI;
using Abp.Webhooks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.Caching;
using NextGen.BiddingPlatform.WebHooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NextGen.BiddingPlatform.Enums.ItemEnums;

namespace NextGen.BiddingPlatform.AuctionHistory
{
    public class AuctionHistoryAppService : BiddingPlatformAppServiceBase, IAuctionHistoryAppService
    {
        private readonly IRepository<Core.AuctionHistories.AuctionHistory> _auctionHistoryRepository;
        private readonly IRepository<Core.AuctionBidders.AuctionBidder> _auctionBidderRepository;
        private readonly IRepository<Core.AuctionItems.AuctionItem> _auctionItemRepository;
        private readonly IWebhookPublisher _webHookPublisher;
        private readonly ICachingAppService _cacheService;

        public AuctionHistoryAppService(IRepository<Core.AuctionHistories.AuctionHistory> auctionHistoryRepository,
                                        IRepository<Core.AuctionBidders.AuctionBidder> auctionBidderRepository,
                                        IRepository<Core.AuctionItems.AuctionItem> auctionItemRepository,
                                        UserManager userManager, IWebhookPublisher webhookPublisher,
                                        ICachingAppService cacheService)
        {
            _auctionHistoryRepository = auctionHistoryRepository;
            _auctionBidderRepository = auctionBidderRepository;
            _auctionItemRepository = auctionItemRepository;
            _webHookPublisher = webhookPublisher;
            _cacheService = cacheService;
        }

        public async Task CreateHistory(CreateAuctionHistoryDto input)
        {
            var auctionBidder = await _auctionBidderRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AuctionBidderId);
            if (auctionBidder == null)
                throw new Exception("Auction Bidder not found for given id");

            var auctionItem = await _auctionItemRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AuctionItemId);
            if (auctionItem == null)
                throw new Exception("Auction Item not found for given id");

            await _auctionHistoryRepository.InsertAsync(new Core.AuctionHistories.AuctionHistory
            {
                UniqueId = Guid.NewGuid(),
                AuctionBidderId = auctionBidder.Id,
                AuctionItemId = auctionItem.Id,
                BidAmount = input.BidAmount,
                BidStatus = input.BidStatus
            });
        }

        public async Task<ListResultDto<AuctionHistoryListDto>> GetAllHistory()
        {
            var historyData = _auctionHistoryRepository.GetAllIncluding(x => x.AuctionBidder, x => x.AuctionItem, x => x.AuctionItem.Item)
                                                       .OrderByDescending(x => x.Id)
                                                       .Select(x => new AuctionHistoryListDto
                                                       {
                                                           UniqueId = x.UniqueId,
                                                           BidderId = x.AuctionBidder.UniqueId,
                                                           AuctionItemId = x.AuctionItem.UniqueId,
                                                           BidAmount = x.BidAmount,
                                                           BidderName = x.AuctionBidder.BidderName,
                                                           BidStatus = x.BidStatus,
                                                           ItemName = x.AuctionItem.Item.ItemName,
                                                           ItemType = x.AuctionItem.Item.ItemType
                                                       });

            return new ListResultDto<AuctionHistoryListDto>(await historyData.ToListAsync());
        }

        public async Task<ListResultDto<AuctionHistoryListDto>> GetHistoryByBidderId(Guid auctionBidderId)
        {
            var auctionBidder = await _auctionBidderRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionBidderId);

            var historyData = _auctionHistoryRepository.GetAllIncluding(x => x.AuctionBidder, x => x.AuctionItem, x => x.AuctionItem.Item)
                                                       .OrderByDescending(x => x.Id)
                                                       .Where(x => x.AuctionBidderId == auctionBidder.Id)
                                                       .Select(x => new AuctionHistoryListDto
                                                       {
                                                           UniqueId = x.UniqueId,
                                                           BidderId = x.AuctionBidder.UniqueId,
                                                           AuctionItemId = x.AuctionItem.UniqueId,
                                                           BidAmount = x.BidAmount,
                                                           BidderName = x.AuctionBidder.BidderName,
                                                           BidStatus = x.BidStatus,
                                                           ItemName = x.AuctionItem.Item.ItemName,
                                                           ItemType = x.AuctionItem.Item.ItemType
                                                       });

            return new ListResultDto<AuctionHistoryListDto>(await historyData.ToListAsync());
        }

        public async Task<ListResultDto<AuctionHistoryListDto>> GetHistoryByIds(Guid auctionBidderID, Guid auctionItemId)
        {
            var auctionBidder = await _auctionBidderRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionBidderID);
            var auctionItem = await _auctionItemRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionItemId);
            var historyData = _auctionHistoryRepository.GetAllIncluding(x => x.AuctionBidder, x => x.AuctionItem, x => x.AuctionItem.Item)
                                                       .OrderByDescending(x => x.Id)
                                                       .Where(x => x.AuctionItemId == auctionItem.Id && x.AuctionBidderId == auctionBidder.Id)
                                                       .Select(x => new AuctionHistoryListDto
                                                       {
                                                           UniqueId = x.UniqueId,
                                                           BidderId = x.AuctionBidder.UniqueId,
                                                           AuctionItemId = x.AuctionItem.UniqueId,
                                                           BidAmount = x.BidAmount,
                                                           BidderName = x.AuctionBidder.BidderName,
                                                           BidStatus = x.BidStatus,
                                                           ItemName = x.AuctionItem.Item.ItemName,
                                                           ItemType = x.AuctionItem.Item.ItemType
                                                       });

            return new ListResultDto<AuctionHistoryListDto>(await historyData.ToListAsync());
        }
        public async Task<List<GetAuctionHistoryByAuctionIdDto>> GetHistorbyAuctionItemId(Guid auctionItemId, int pageSize, int pageIndex)
        {
            var auctionItem = await _auctionItemRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionItemId);
            if (auctionItem == null)
                throw new UserFriendlyException("Auction item not found for given id");

            var historyData = await _auctionHistoryRepository.GetAllIncluding(x => x.AuctionBidder).AsNoTracking()
                                                       .Where(x => x.AuctionItemId == auctionItem.Id)
                                                       .OrderByDescending(x => x.CreationTime)
                                                       .Skip((pageIndex - 1) * pageSize).Take(pageSize)
                                                       .Select(x => new GetAuctionHistoryByAuctionIdDto
                                                       {
                                                           BidAmount = x.BidAmount,
                                                           BidderName = x.AuctionBidder.BidderName,
                                                           BiddingDate = x.CreationTime.ToString("MM/dd/yyyy"),
                                                           BiddingTime = x.CreationTime.ToString("hh:mm tt"),
                                                           BidHistoryDate = x.CreationTime
                                                       }).ToListAsync();

            return historyData;
        }

        //Get Auction Item History by AuctionItemId
        public async Task<AuctionHistoryListDto> GetHistoryByAuctionHistoryId(Guid Id)
        {

            var historyData = await _auctionHistoryRepository.GetAllIncluding(x => x.AuctionBidder, x => x.AuctionItem, x => x.AuctionItem.Item)
                                                       .Select(x => new AuctionHistoryListDto
                                                       {
                                                           UniqueId = x.UniqueId,
                                                           BidderId = x.AuctionBidder.UniqueId,
                                                           AuctionItemId = x.AuctionItem.UniqueId,
                                                           BidAmount = x.BidAmount,
                                                           BidderName = x.AuctionBidder.BidderName,
                                                           BidStatus = x.BidStatus,
                                                           ItemName = x.AuctionItem.Item.ItemName,
                                                           ItemType = x.AuctionItem.Item.ItemType
                                                       }).FirstOrDefaultAsync(x => x.UniqueId == Id);

            return historyData;
        }

        //custom Method
        [AllowAnonymous]
        public async Task SaveAuctionBidderWithHistory(object auctionBidderHistory)
        {
            if (auctionBidderHistory.GetType() == typeof(AuctionBidderHistoryDto))
                await SaveSingleHistory(auctionBidderHistory as AuctionBidderHistoryDto);
            else if (auctionBidderHistory.GetType() == typeof(List<AuctionBidderHistoryDto>))
                await SaveBulkHistory(auctionBidderHistory as List<AuctionBidderHistoryDto>);
        }

        private async Task SaveBulkHistory(List<AuctionBidderHistoryDto> auctionItemBids)
        {
            var auctionItemId = auctionItemBids.FirstOrDefault().AuctionItemId;
            var auctionItem = await _auctionItemRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionItemId);
            if (auctionItem == null)
                throw new UserFriendlyException("Auction item not found for bidding");

            //var currUserId = auctionBidderHistory.UserId;
            foreach (var item in auctionItemBids)
            {
                await _auctionHistoryRepository.InsertAsync(new Core.AuctionHistories.AuctionHistory
                {
                    AuctionItemId = auctionItem.Id,
                    BidAmount = item.BidAmount,
                    BidStatus = BiddingStatus.Pending.ToString(),
                    CreationTime = Clock.Now,
                    UniqueId = Guid.NewGuid(),
                    AuctionBidderId = item.AuctionBidderId.Value
                });
            }

            await CurrentUnitOfWork.SaveChangesAsync();
            var tenantId = auctionItemBids.FirstOrDefault()?.TenantId;
            var auctionItemHistoryDetails = await GetAuctionItemHistoryCount(auctionItem.Id);
            //if we want to send webhook to specific tenant then we have optional parameter TenantId with PublishAsync Method
            //if we will not pass the TenantId parameter then it will pick the subscriptions of the host
            await _webHookPublisher.PublishAsync(AppWebHookNames.TestAuctionHistoryWebhook, new GetAuctionBidderHistoryDto
            {
                //AuctionBidderId = auctionBidderHistory.AuctionBidderId.Value,
                // BidderName = auctionBidderHistory.BidderName,
                HistoryCount = auctionItemHistoryDetails.Key,
                AuctionItemId = auctionItemId,
                LastHistoryAmount = auctionItemHistoryDetails.Value,
                AuctionItemHistory = await GetHistorbyAuctionItemId(auctionItemId, 10, 1),
            }, tenantId);
        }
        private async Task SaveSingleHistory(AuctionBidderHistoryDto auctionBidderHistory)
        {
            var auctionItem = await _auctionItemRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionBidderHistory.AuctionItemId);
            if (auctionItem == null)
                throw new UserFriendlyException("Auction item not found for bidding");

            await _auctionHistoryRepository.InsertAsync(new Core.AuctionHistories.AuctionHistory
            {
                AuctionItemId = auctionItem.Id,
                BidAmount = auctionBidderHistory.BidAmount,
                BidStatus = BiddingStatus.Pending.ToString(),
                CreationTime = Clock.Now,
                UniqueId = Guid.NewGuid(),
                AuctionBidderId = auctionBidderHistory.AuctionBidderId.Value
            });
            await CurrentUnitOfWork.SaveChangesAsync();
            var auctionItemHistoryDetails = await GetAuctionItemHistoryCount(auctionItem.Id);
            //if we want to send webhook to specific tenant then we have optional parameter TenantId with PublishAsync Method
            //if we will not pass the TenantId parameter then it will pick the subscriptions of the host
            var tenantId = auctionBidderHistory.TenantId;
            await _webHookPublisher.PublishAsync(AppWebHookNames.TestAuctionHistoryWebhook, new GetAuctionBidderHistoryDto
            {
                // AuctionBidderId = auctionBidderHistory.AuctionBidderId.Value,
                // = auctionBidderHistory.BidderName,
                HistoryCount = auctionItemHistoryDetails.Key,
                AuctionItemId = auctionBidderHistory.AuctionItemId,
                LastHistoryAmount = auctionItemHistoryDetails.Value,
                AuctionItemHistory = await GetHistorbyAuctionItemId(auctionBidderHistory.AuctionItemId, 10, 1),
            },
            auctionBidderHistory.TenantId);
        }
        private async Task<KeyValuePair<int, double>> GetAuctionItemHistoryCount(int auctionItemId)
        {
            var auctionItemHistory = await _auctionHistoryRepository.GetAllListAsync(x => x.AuctionItemId == auctionItemId);
            var lastAmount = auctionItemHistory.OrderByDescending(x => x.Id).FirstOrDefault()?.BidAmount;
            return new KeyValuePair<int, double>(auctionItemHistory.Count, lastAmount ?? 0);
        }

        public async Task<GetAuctionBidderHistoryDto> SaveFreshBidder(AuctionBidderHistoryDto auctionBidderHistory)
        {
            var auctionItem = await _auctionItemRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionBidderHistory.AuctionItemId);
            if (auctionItem == null)
                throw new UserFriendlyException("Auction item not found for bidding");
            var currUserId = auctionBidderHistory.UserId;
            var auctionBidder = new Core.AuctionBidders.AuctionBidder
            {
                AuctionId = auctionItem.AuctionId,
                BidderName = auctionBidderHistory.BidderName,
                CreationTime = Clock.Now,
                CreatorUserId = currUserId,
                UserId = currUserId,
                UniqueId = Guid.NewGuid()
            };
            auctionBidder.AuctionHistories.Add(new Core.AuctionHistories.AuctionHistory
            {
                AuctionItemId = auctionItem.Id,
                BidAmount = auctionBidderHistory.BidAmount,
                BidStatus = BiddingStatus.Pending.ToString(),
                CreationTime = Clock.Now,
                CreatorUserId = currUserId,
                UniqueId = Guid.NewGuid()
            });
            await _auctionBidderRepository.InsertAsync(auctionBidder);
            await CurrentUnitOfWork.SaveChangesAsync();
            var auctionItemHistoryDetails = await GetAuctionItemHistoryCount(auctionItem.Id);

            var auctionWinnerCache = _cacheService.GetWinnerCache(auctionBidderHistory.AuctionItemId.ToString());
            if (auctionWinnerCache != null)
            {
                var castObj = auctionWinnerCache as AuctionItemWinnerDto;
                if (auctionBidderHistory.BidAmount > castObj.BidAmount)
                    await _cacheService.SetWinnerCache(new AuctionItemWinnerDto { });
            }
            else
                await _cacheService.SetWinnerCache(new AuctionItemWinnerDto { AuctionItemId = auctionBidderHistory.AuctionItemId, AuctionBidderId = auctionBidder.Id, BidAmount = auctionBidderHistory.BidAmount, UserId = auctionBidderHistory.UserId });

            var auctionHistory = await GetHistorbyAuctionItemId(auctionBidderHistory.AuctionItemId, 10, 1);
            await _webHookPublisher.PublishAsync(AppWebHookNames.TestAuctionHistoryWebhook, new GetAuctionBidderHistoryDto
            {
                HistoryCount = auctionItemHistoryDetails.Key,
                AuctionItemId = auctionBidderHistory.AuctionItemId,
                LastHistoryAmount = auctionItemHistoryDetails.Value,
                AuctionItemHistory = auctionHistory
            });

            return new GetAuctionBidderHistoryDto
            {
                HistoryCount = auctionItemHistoryDetails.Key,
                LastHistoryAmount = auctionItemHistoryDetails.Value,
                AuctionBidderId = auctionBidder.Id,
                AuctionItemHistory = auctionHistory
            };
        }
    }
}
