using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using NextGen.BiddingPlatform.AuctionHistory;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using NextGen.BiddingPlatform.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.HostingServices
{
    public class BackgroundRedisCacheWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IAuctionHistoryAppService _auctionHistoryService;
        private readonly ICachingAppService _cacheService;
        public BackgroundRedisCacheWorker(AbpTimer timer, IAuctionHistoryAppService auctionHistoryAppService, ICachingAppService cacheService)
        : base(timer)
        {
            Timer.Period = 5000; //5 seconds (good for tests, but normally will be more)
            _cacheService = cacheService;
            _auctionHistoryService = auctionHistoryAppService;
        }
        protected override async void DoWork()
        {
            // _cacheManager.GetCache("AuctionHistoryCache").Remove("auctionhistories");
            List<AuctionBidderHistoryDto> lstData = new List<AuctionBidderHistoryDto>();
            lstData = _cacheService.GetHistoryCache();
            //here we will process all the request at a time
            if (lstData.Count > 0)
            {
                foreach (var auctionItem in lstData.GroupBy(s => s.AuctionItemId))
                {
                    var auctionItemBids = auctionItem.ToList();
                    await _auctionHistoryService.SaveAuctionBidderWithHistory(auctionItemBids);
                    //winner cache
                    var highestBidder = auctionItemBids.OrderBy(x => x.CreationTime).ThenByDescending(x => x.BidAmount).FirstOrDefault();
                    var auctionWinnerCache = _cacheService.GetWinnerCache(auctionItem.Key.ToString());
                    if (auctionWinnerCache != null)
                    {
                        var castObj = auctionWinnerCache as AuctionItemWinnerDto;
                        if (highestBidder.BidAmount > castObj.BidAmount)
                        {
                            await _cacheService.SetWinnerCache(new AuctionItemWinnerDto { AuctionItemId = highestBidder.AuctionItemId, AuctionBidderId = highestBidder.AuctionBidderId, BidAmount = highestBidder.BidAmount, UserId = highestBidder.UserId });
                        }
                    }
                    else
                        await _cacheService.SetWinnerCache(new AuctionItemWinnerDto { AuctionItemId = highestBidder.AuctionItemId, AuctionBidderId = highestBidder.AuctionBidderId, BidAmount = highestBidder.BidAmount, UserId = highestBidder.UserId });

                    var data = _cacheService.GetHistoryCache();
                    var processIds = auctionItemBids.Select(x => x.UniqueId).ToList();
                    data = data.Where(x => !processIds.Contains(x.UniqueId)).ToList();
                    await _cacheService.SetHistoryCache(data);
                }
            }
        }
    }
}
