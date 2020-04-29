using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.AuctionItem.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.AuctionItem
{
    public interface IAuctionItemAppService : IApplicationService
    {
        Task<ListResultDto<AuctionItemListDto>> GetAllAuctionItems();
        Task<ListResultDto<AuctionItemListDto>> GetAuctionItemsByAuctionId(Guid auctionId);
        Task<AuctionItemDto> GetAuctionItemById(Guid Id);
        Task<AuctionItemDto> Create(CreateAuctionItemDto input);
        Task CreateAuctionItems(List<CreateAuctionItemDto> auctionItems);
        Task Delete(Guid Id);
    }
}
