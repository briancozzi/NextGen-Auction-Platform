using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using NextGen.BiddingPlatform.AuctionHistory;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using System;
using System.Collections.Generic;
using System.Text;

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
        protected override void DoWork()
        {
            var data = _cacheManager.GetCache("AuctionHistoryCache").Get("AuctionHistoryTest", () => GetData());
            if (data != null)
            {

            }
        }
        private AuctionBidderHistoryDto GetData()
        {
            return null;
        }
    }
}
