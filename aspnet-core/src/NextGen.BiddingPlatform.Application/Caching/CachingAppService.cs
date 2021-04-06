using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Runtime.Caching;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using NextGen.BiddingPlatform.Authorization;
using NextGen.BiddingPlatform.Caching.Dto;

namespace NextGen.BiddingPlatform.Caching
{
    //[AbpAuthorize(AppPermissions.Pages_Administration_Host_Maintenance)]
    public class CachingAppService : BiddingPlatformAppServiceBase, ICachingAppService
    {
        private readonly ICacheManager _cacheManager;

        public CachingAppService(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Maintenance)]
        public ListResultDto<CacheDto> GetAllCaches()
        {
            var caches = _cacheManager.GetAllCaches()
                                        .Select(cache => new CacheDto
                                        {
                                            Name = cache.Name
                                        })
                                        .ToList();

            return new ListResultDto<CacheDto>(caches);
        }
        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Maintenance)]
        public async Task ClearCache(EntityDto<string> input)
        {
            var cache = _cacheManager.GetCache(input.Id);
            await cache.ClearAsync();
        }
        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Maintenance)]
        public async Task ClearAllCaches()
        {
            var caches = _cacheManager.GetAllCaches();
            foreach (var cache in caches)
            {
                await cache.ClearAsync();
            }
        }
        public List<AuctionBidderHistoryDto> GetHistoryCache()
        {
            return _cacheManager.GetCache("AuctionHistoryCache").Get("auctionhistories", () => new List<AuctionBidderHistoryDto>());
        }
        public async Task SetHistoryCache(List<AuctionBidderHistoryDto> data)
        {
            await _cacheManager.GetCache("AuctionHistoryCache").SetAsync("auctionhistories", data);
        }
        public AuctionItemWinnerDto GetWinnerCache(string auctinItemId)
        {
            var auctionItemWinnerId = "auctionitem_" + auctinItemId + "_winner";
            var auctionWinnerCache = _cacheManager.GetCache("AuctionWinnerCache").Get(auctionItemWinnerId, () => null);
            return auctionWinnerCache as AuctionItemWinnerDto;
        }
        public async Task SetWinnerCache(AuctionItemWinnerDto winnerDto)
        {
            var auctionItemWinnerId = "auctionitem_" + winnerDto.AuctionItemId + "_winner";
            await _cacheManager.GetCache("AuctionWinnerCache").SetAsync(auctionItemWinnerId, winnerDto);
        }
    }
}