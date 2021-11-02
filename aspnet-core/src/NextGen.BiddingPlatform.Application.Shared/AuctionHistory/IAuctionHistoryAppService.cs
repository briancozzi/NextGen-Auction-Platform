using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.AuctionHistory
{
    public interface IAuctionHistoryAppService : IApplicationService
    {
        Task CreateHistory(CreateAuctionHistoryDto input);
        Task<ListResultDto<AuctionHistoryListDto>> GetAllHistory();
        Task<ListResultDto<AuctionHistoryListDto>> GetHistoryByBidderId(Guid auctionBidderId);
        Task<AuctionHistoryListDto> GetHistoryByAuctionHistoryId(Guid Id);
        Task<ListResultDto<AuctionHistoryListDto>> GetHistoryByIds(Guid auctionBidderID, Guid auctionItemId);
        Task<List<GetAuctionHistoryByAuctionIdDto>> GetHistorbyAuctionItemId(Guid auctionItemId, int pageSize, int pageIndex);

        //Custom method for testing user
        Task SaveAuctionBidderWithHistory(object auctionBiddingHistory);
        //For Saving Fresh Bidder
        Task<GetAuctionBidderHistoryDto> SaveFreshBidder(AuctionBidderHistoryDto auctionBidderHistory);

        Task<AuctionHistoryDto> GetHighestBid(Guid auctionItemId);
        Task<SendEventAuctionItemWinnerDataDto> GetDataToSendEventAuctionItemDataToExternalApp(Guid auctionItemId);
    }
}
