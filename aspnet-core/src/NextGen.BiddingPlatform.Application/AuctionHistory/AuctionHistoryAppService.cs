using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.AuctionHistory
{
    [AbpAuthorize]
    public class AuctionHistoryAppService : BiddingPlatformAppServiceBase, IAuctionHistoryAppService
    {
        private readonly IRepository<Core.AuctionHistories.AuctionHistory> _auctionHistoryRepository;
        private readonly IRepository<Core.AuctionBidders.AuctionBidder> _auctionBidderRepository;
        private readonly IRepository<Core.AuctionItems.AuctionItem> _auctionItemRepository;
        public AuctionHistoryAppService(IRepository<Core.AuctionHistories.AuctionHistory> auctionHistoryRepository,
                                        IRepository<Core.AuctionBidders.AuctionBidder> auctionBidderRepository,
                                        IRepository<Core.AuctionItems.AuctionItem> auctionItemRepository)
        {
            _auctionHistoryRepository = auctionHistoryRepository;
            _auctionBidderRepository = auctionBidderRepository;
            _auctionItemRepository = auctionItemRepository;
        }

        public async Task CreateHistory(CreateAuctionHistoryDto input)
        {
            var auctionBidder = await _auctionBidderRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AuctionBidderId);
            if (auctionBidder == null)
                throw new Exception("Auction Bidder not found for given id");

            var auctionItem = await _auctionItemRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AuctionItemId);
            if (auctionItem == null)
                throw new Exception("Auction Item not found for given id");

            if (!AbpSession.TenantId.HasValue)
                throw new Exception("You are not authorized user");

            await _auctionHistoryRepository.InsertAsync(new Core.AuctionHistories.AuctionHistory
            {
                UniqueId = Guid.NewGuid(),
                TenantId = AbpSession.TenantId.Value,
                AuctionBidderId = auctionBidder.Id,
                AuctionItemId = auctionItem.Id,
                BidAmount = input.BidAmount,
                BidStatus = input.BidStatus
            });
        }

        public async Task<ListResultDto<AuctionHistoryListDto>> GetAllHistory()
        {
            var historyData = _auctionHistoryRepository.GetAllIncluding(x => x.AuctionBidder, x => x.AuctionItem, x => x.AuctionItem.Item)
                                                       .OrderByDescending(x => x.Id)
                                                       .Select(x => new AuctionHistoryListDto
                                                       {
                                                           UniqueId = x.UniqueId,
                                                           BidderId = x.AuctionBidder.UniqueId,
                                                           AuctionItemId = x.AuctionItem.UniqueId,
                                                           BidAmount = x.BidAmount,
                                                           BidderName = x.AuctionBidder.BidderName,
                                                           BidStatus = x.BidStatus,
                                                           ItemName = x.AuctionItem.Item.ItemName,
                                                           ItemType = x.AuctionItem.Item.ItemType
                                                       });

            return new ListResultDto<AuctionHistoryListDto>(await historyData.ToListAsync());
        }

        public async Task<ListResultDto<AuctionHistoryListDto>> GetHistoryByBidderId(Guid auctionBidderId)
        {
            var auctionBidder = await _auctionBidderRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionBidderId);

            var historyData = _auctionHistoryRepository.GetAllIncluding(x => x.AuctionBidder, x => x.AuctionItem, x => x.AuctionItem.Item)
                                                       .OrderByDescending(x => x.Id)
                                                       .Where(x => x.AuctionBidderId == auctionBidder.Id)
                                                       .Select(x => new AuctionHistoryListDto
                                                       {
                                                           UniqueId = x.UniqueId,
                                                           BidderId = x.AuctionBidder.UniqueId,
                                                           AuctionItemId = x.AuctionItem.UniqueId,
                                                           BidAmount = x.BidAmount,
                                                           BidderName = x.AuctionBidder.BidderName,
                                                           BidStatus = x.BidStatus,
                                                           ItemName = x.AuctionItem.Item.ItemName,
                                                           ItemType = x.AuctionItem.Item.ItemType
                                                       });

            return new ListResultDto<AuctionHistoryListDto>(await historyData.ToListAsync());
        }

        public async Task<ListResultDto<AuctionHistoryListDto>> GetHistoryByIds(Guid auctionBidderID, Guid auctionItemId)
        {
            var auctionBidder = await _auctionBidderRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionBidderID);
            var auctionItem = await _auctionItemRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionItemId);
            var historyData = _auctionHistoryRepository.GetAllIncluding(x => x.AuctionBidder, x => x.AuctionItem, x => x.AuctionItem.Item)
                                                       .OrderByDescending(x => x.Id)
                                                       .Where(x => x.AuctionItemId == auctionItem.Id && x.AuctionBidderId == auctionBidder.Id)
                                                       .Select(x => new AuctionHistoryListDto
                                                       {
                                                           UniqueId = x.UniqueId,
                                                           BidderId = x.AuctionBidder.UniqueId,
                                                           AuctionItemId = x.AuctionItem.UniqueId,
                                                           BidAmount = x.BidAmount,
                                                           BidderName = x.AuctionBidder.BidderName,
                                                           BidStatus = x.BidStatus,
                                                           ItemName = x.AuctionItem.Item.ItemName,
                                                           ItemType = x.AuctionItem.Item.ItemType
                                                       });

            return new ListResultDto<AuctionHistoryListDto>(await historyData.ToListAsync());
        }

        public async Task<AuctionHistoryListDto> GetHistoryByAuctionHistoryId(Guid Id)
        {

            var historyData = await _auctionHistoryRepository.GetAllIncluding(x => x.AuctionBidder, x => x.AuctionItem, x => x.AuctionItem.Item)
                                                       .Select(x => new AuctionHistoryListDto
                                                       {
                                                           UniqueId = x.UniqueId,
                                                           BidderId = x.AuctionBidder.UniqueId,
                                                           AuctionItemId = x.AuctionItem.UniqueId,
                                                           BidAmount = x.BidAmount,
                                                           BidderName = x.AuctionBidder.BidderName,
                                                           BidStatus = x.BidStatus,
                                                           ItemName = x.AuctionItem.Item.ItemName,
                                                           ItemType = x.AuctionItem.Item.ItemType
                                                       }).FirstOrDefaultAsync(x => x.UniqueId == Id);

            return historyData;
        }
    }
}
