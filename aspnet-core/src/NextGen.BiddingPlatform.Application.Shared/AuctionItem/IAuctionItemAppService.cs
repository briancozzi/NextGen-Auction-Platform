﻿using Abp.Application.Services;
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
        Task<PagedResultDto<AuctionItemListDto>> GetAuctionItemsWithFilter(AuctionItemFilter input);
        Task<ListResultDto<AuctionItemListDto>> GetAuctionItemsByAuctionId(Guid auctionId);
        Task<CreateAuctionItemDto> GetAuctionItemById(Guid Id);
        Task<AuctionItemDto> Create(CreateAuctionItemDto input);
        Task<AuctionItemDto> Update(CreateAuctionItemDto input);
        Task CreateAuctionItems(List<CreateAuctionItemDto> auctionItems);
        Task Delete(Guid Id);
    }
}
