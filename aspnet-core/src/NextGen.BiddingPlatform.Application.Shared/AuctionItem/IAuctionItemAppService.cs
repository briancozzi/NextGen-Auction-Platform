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
        //Task<PagedResultDto<AuctionItemDto>> GetItemsWithFilter(ItemFilter input);
        Task<AuctionItemDto> GetAuctionItemById(Guid Id);
        Task<AuctionItemDto> Create(CreateAuctionItemDto input);
        Task Update(UpdateAuctionItemDto input);
        Task Delete(Guid Id);
    }
}
