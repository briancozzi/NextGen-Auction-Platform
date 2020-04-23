using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.Auction.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Auction
{
    public interface IAuctionAppService : IApplicationService
    {
        Task<ListResultDto<AuctionListDto>> GetAll();
        Task<AuctionDto> GetAuctionById(Guid Id);
        Task CreateAuction(CreateAuctionDto input);
        Task UpdateAuction(UpdateAuctionDto input);
    }
}
