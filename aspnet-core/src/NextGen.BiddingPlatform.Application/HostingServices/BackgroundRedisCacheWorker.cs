using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using NextGen.BiddingPlatform.AuctionHistory;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.HostingServices
{
    public class BackgroundRedisCacheWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly ICacheManager _cacheManager;
        private readonly IAuctionHistoryAppService _auctionHistoryService;
        public BackgroundRedisCacheWorker(AbpTimer timer, ICacheManager cacheManager, IAuctionHistoryAppService auctionHistoryAppService)
        : base(timer)
        {
            Timer.Period = 5000; //5 seconds (good for tests, but normally will be more)
            _cacheManager = cacheManager;
            _auctionHistoryService = auctionHistoryAppService;
        }
        protected override async void DoWork()
        {
            // _cacheManager.GetCache("AuctionHistoryCache").Remove("auctionhistories");
            List<AuctionBidderHistoryDto> lstData = new List<AuctionBidderHistoryDto>();
            lstData = _cacheManager.GetCache("AuctionHistoryCache").Get("auctionhistories", () => new List<AuctionBidderHistoryDto>());
            var processData = lstData.FirstOrDefault();
            if (processData != null)
            {
                await _auctionHistoryService.SaveAuctionBidderWithHistory(processData);
                var data = _cacheManager.GetCache("AuctionHistoryCache").Get("auctionhistories", () => new List<AuctionBidderHistoryDto>());
                data = data.Where(x => x.UniqueId != processData.UniqueId).ToList();
                _cacheManager.GetCache("AuctionHistoryCache").Set("auctionhistories", data);
            }
        }
    }
}
