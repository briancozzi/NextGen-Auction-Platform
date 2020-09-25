using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
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
    public class AuctionItemAppService : BiddingPlatformAppServiceBase, IAuctionItemAppService
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
                                                                AuctionItemId = s.UniqueId,
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
        public async Task<PagedResultDto<AuctionItemListDto>> GetAuctionItemsWithFilter(AuctionItemFilter input)
        {
            var query = _auctionitemRepository.GetAllIncluding(x => x.Auction, x => x.Item).AsNoTracking()
                                       .WhereIf(!input.Search.IsNullOrWhiteSpace(), x => x.Auction.AuctionType.ToLower().IndexOf(input.Search.ToLower()) > -1)
                                       .Select(s => new AuctionItemListDto
                                       {
                                           AuctionItemId = s.UniqueId,
                                           AuctionId = s.Auction.UniqueId,
                                           AuctionEndDateTime = s.Auction.AuctionEndDateTime,
                                           AuctionStartDateTime = s.Auction.AuctionStartDateTime,
                                           AuctionType = s.Auction.AuctionType,
                                           ItemId = s.Item.UniqueId,
                                           ItemName = s.Item.ItemName,
                                           ItemNumber = s.Item.ItemNumber,
                                           ItemType = s.Item.ItemType
                                       }).AsQueryable();

            var resultCount = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);

            var resultQuery = query.PageBy(input).ToList();

            return new PagedResultDto<AuctionItemListDto>(resultCount, resultQuery);
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
        public async Task<CreateAuctionItemDto> GetAuctionItemById(Guid Id)
        {
            var output = await _auctionitemRepository.GetAllIncluding(x => x.Item, x => x.Auction).FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (output == null)
                throw new UserFriendlyException("Auction Item not found for given id");

            return ObjectMapper.Map<CreateAuctionItemDto>(output);
        }
        public async Task<AuctionItemDto> Create(CreateAuctionItemDto input)
        {
            var item = await ValidateItemAndGet(input);
            var auction = await ValidateAuctionAndGet(input);

            var existAuctionItem = await _auctionitemRepository.FirstOrDefaultAsync(x => x.AuctionId == auction.Id && x.ItemId == item.Id);
            if (existAuctionItem != null)
                throw new UserFriendlyException("Auction item already exist");
            var auctionItem = await _auctionitemRepository.InsertAsync(new Core.AuctionItems.AuctionItem
            {
                UniqueId = Guid.NewGuid(),
                TenantId = _abpSession.TenantId.Value,
                AuctionId = auction.Id,
                ItemId = item.Id,
                IsActive = input.IsActive
            });

            return new AuctionItemDto
            {
                UniqueId = auctionItem.UniqueId,
                AuctionId = input.AuctionId,
                ItemId = input.ItemId,
            };
        }
        private async Task<Core.Auctions.Auction> ValidateAuctionAndGet(CreateAuctionItemDto input)
        {
            var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AuctionId);
            if (auction == null)
                throw new UserFriendlyException("Auction not found by given Id");
            else
                return auction;
        }
        private async Task<Core.Items.Item> ValidateItemAndGet(CreateAuctionItemDto input)
        {
            var item = await _itemRepository.FirstOrDefaultAsync(x => x.UniqueId == input.ItemId);
            if (item == null)
                throw new UserFriendlyException("Item not found by given id");
            else
                return item;
        }
        public async Task<AuctionItemDto> Update(CreateAuctionItemDto input)
        {
            if (input.UniqueId == Guid.Empty)
                throw new UserFriendlyException("Invalid id!!.");

            var auctionItem = await _auctionitemRepository.FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (auctionItem == null)
                throw new UserFriendlyException("Auction item not found!!.");

            var item = await ValidateItemAndGet(input);
            var auction = await ValidateAuctionAndGet(input);

            auctionItem.AuctionId = auction.Id;
            auctionItem.ItemId = item.Id;
            auctionItem.LastModificationTime = Clock.Now.ToUniversalTime();
            auctionItem.LastModifierUserId = _abpSession.UserId;
            await _auctionitemRepository.UpdateAsync(auctionItem);
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
