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
        Task<UpdateAuctionDto> GetAuctionById(Guid Id);
        Task<CreateAuctionDto> CreateAuction(CreateAuctionDto input);
        Task<UpdateAuctionDto> UpdateAuction(UpdateAuctionDto input);
        Task Delete(EntityDto<Guid> input);

        // AuctionType Filter.
        Task<PagedResultDto<AuctionListDto>> GetAllAuctionFilter(AuctionTypeFilter input);
        Task<List<AuctionSelectDto>> GetAuctions();
    }
}
