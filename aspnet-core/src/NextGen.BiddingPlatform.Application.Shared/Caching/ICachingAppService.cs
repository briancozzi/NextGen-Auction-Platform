using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using NextGen.BiddingPlatform.Caching.Dto;

namespace NextGen.BiddingPlatform.Caching
{
    public interface ICachingAppService : IApplicationService
    {
        ListResultDto<CacheDto> GetAllCaches();

        Task ClearCache(EntityDto<string> input);

        Task ClearAllCaches();
        Task SetWinnerCache(AuctionItemWinnerDto winnerDto);
        AuctionItemWinnerDto GetWinnerCache(string auctinItemId);
        List<AuctionBidderHistoryDto> GetHistoryCache();
        Task SetHistoryCache(List<AuctionBidderHistoryDto> data);


        //highest bid cache
        AuctionItemHighestBid GetHighesBidHistoryCache(string auctionItemId);
        Task SetHighesBidHistoryCache(AuctionItemHighestBid data);
    }
}
