using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.AuctionItem.Dto;
using NextGen.BiddingPlatform.DashboardCustomization.Dto;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.AuctionItem
{
    public class AuctionItemAppService : BiddingPlatformDomainServiceBase, IAuctionItemAppService
    {

        private readonly IRepository<Core.AuctionItems.AuctionItem> _auctionitemRepository;
        private readonly IRepository<Core.Auctions.Auction> _auctionRepository;
        private readonly IRepository<Core.Items.Item> _itemRepository;
        private readonly IAbpSession _abpSession;

        public AuctionItemAppService(IRepository<Core.AuctionItems.AuctionItem> auctionitemRepository,
                                     IRepository<Core.Auctions.Auction> auctionRepository,
                                     IRepository<Core.Items.Item> itemRepository,
                                     IAbpSession abpSession)
        {
            _auctionitemRepository = auctionitemRepository;
            _auctionRepository = auctionRepository;
            _itemRepository = itemRepository;
            _abpSession = abpSession;
        }
        public async Task<ListResultDto<AuctionItemListDto>> GetAllAuctionItems()
        {
            var auctionItems = await _auctionitemRepository.GetAllIncluding(x => x.Auction, x => x.Item)
                                                            .Select(s => new AuctionItemListDto
                                                            {
                                                                AuctionId = s.Auction.UniqueId,
                                                                AuctionEndDateTime = s.Auction.AuctionEndDateTime,
                                                                AuctionStartDateTime = s.Auction.AuctionStartDateTime,
                                                                AuctionType = s.Auction.AuctionType,
                                                                ItemId = s.Item.UniqueId,
                                                                ItemName = s.Item.ItemName,
                                                                ItemNumber = s.Item.ItemNumber,
                                                                ItemType = s.Item.ItemType
                                                            })
                                                            .ToListAsync();

            return new ListResultDto<AuctionItemListDto>(auctionItems);
        }

        public async Task<ListResultDto<AuctionItemListDto>> GetAuctionItemsByAuctionId(Guid auctionId)
        {
            var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionId);
            if (auction == null)
                throw new Exception("Auction not available for give auction id");

            var auctionItems = await _auctionitemRepository.GetAllIncluding(x => x.Auction, x => x.Item)
                                                            .Where(x => x.AuctionId == auction.Id)
                                                            .Select(s => new AuctionItemListDto
                                                            {
                                                                AuctionId = s.Auction.UniqueId,
                                                                AuctionEndDateTime = s.Auction.AuctionEndDateTime,
                                                                AuctionStartDateTime = s.Auction.AuctionStartDateTime,
                                                                AuctionType = s.Auction.AuctionType,
                                                                ItemId = s.Item.UniqueId,
                                                                ItemName = s.Item.ItemName,
                                                                ItemNumber = s.Item.ItemNumber,
                                                                ItemType = s.Item.ItemType
                                                            })
                                                            .ToListAsync();

            return new ListResultDto<AuctionItemListDto>(auctionItems);
        }

        public async Task<AuctionItemDto> GetAuctionItemById(Guid Id)
        {
            var output = await _auctionitemRepository.GetAllIncluding(x => x.Item, x => x.Auction).FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (output == null)
                throw new Exception("Auction Item not found for given id");

            return ObjectMapper.Map<AuctionItemDto>(output);
        }

        public async Task<AuctionItemDto> Create(CreateAuctionItemDto input)
        {
            var item = await _itemRepository.FirstOrDefaultAsync(x => x.UniqueId == input.ItemId);
            if (item == null)
                throw new Exception("Item not found by given id");

            var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AuctionId);
            if (auction == null)
                throw new Exception("Auction not found by given Id");

            var existAuctionItem = await _auctionitemRepository.FirstOrDefaultAsync(x => x.AuctionId == auction.Id && x.ItemId == item.Id);
            if (existAuctionItem != null)
                throw new Exception("Auction item already exist");

            var auctionItem = await _auctionitemRepository.InsertAsync(new Core.AuctionItems.AuctionItem
            {
                UniqueId = Guid.NewGuid(),
                TenantId = _abpSession.TenantId.Value,
                AuctionId = item.Id,
                ItemId = auction.Id,
                IsActive = input.IsActive
            });

            return new AuctionItemDto
            {
                UniqueId = auctionItem.UniqueId,
                AuctionId = input.AuctionId,
                ItemId = input.ItemId,
            };
        }

        public async Task CreateAuctionItems(List<CreateAuctionItemDto> auctionItems)
        {
            var items = _itemRepository.GetAll();
            var auctionId = auctionItems.FirstOrDefault();

            var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionId.AuctionId);
            if (auction == null)
                throw new Exception("Auction not available for give auction id");

            foreach (var item in auctionItems)
            {
                var currentItem = items.FirstOrDefault(x => x.UniqueId == item.ItemId);
                if (currentItem == null)
                    throw new Exception("Item not available for given id");

                await _auctionitemRepository.InsertAsync(new Core.AuctionItems.AuctionItem
                {
                    UniqueId = Guid.NewGuid(),
                    TenantId = _abpSession.TenantId.Value,
                    AuctionId = auction.Id,
                    ItemId = currentItem.Id,
                    IsActive = item.IsActive
                });
            }
        }

        public async Task Delete(Guid Id)
        {
            var auctionItem = await _auctionitemRepository.FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (auctionItem == null)
                throw new Exception("AuctionItem not found for given Id");

            await _auctionitemRepository.DeleteAsync(auctionItem);
        }
    }
}
