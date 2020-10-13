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
        Task<ListResultDto<GetAuctionHistoryByAuctionIdDto>> GetHistorbyAuctionItemId(Guid auctionItemId);

        //Custom method for testing user
        Task<GetAuctionBidderHistoryDto> SaveAuctionBidderWithHistory(AuctionBidderHistoryDto auctionBiddingHistory);
    }
}
