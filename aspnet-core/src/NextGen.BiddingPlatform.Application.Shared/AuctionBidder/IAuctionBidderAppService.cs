using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.AuctionBidder.Dto;
using NextGen.BiddingPlatform.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.AuctionBidder
{
    public interface IAuctionBidderAppService : IApplicationService
    {
        Task CreateBidder(CreateAuctionBidderDto input);
        //Task UpdateBidder(UpdateAuctionBidderDto input);
        Task DeleteBidder(Guid Id);
        Task<ListResultDto<AuctionBidderListDto>> GetAllBidders();
        Task<ListResultDto<AuctionBidderListDto>> GetBiddersByAuctionId(Guid auctionId);
        Task<ApiResponse<object>> CreateBidderFromExternalApp(CreateAuctionBidderWithExternalApp input);
    }
}
