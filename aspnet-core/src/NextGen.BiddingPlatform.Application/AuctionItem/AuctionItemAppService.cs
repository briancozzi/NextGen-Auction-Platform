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

        public async Task<AuctionItemDto> Create(CreateAuctionItemDto input)
        {
            var hasItem = await _itemRepository.FirstOrDefaultAsync(x => x.UniqueId == input.ItemId);
            if (hasItem == null)
                throw new Exception("Item not found by this Id");

            var hasAuction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AuctionId);
            if (hasAuction == null)
                throw new Exception("Auction not found by this Id");

            await _auctionitemRepository.InsertAsync(new Core.AuctionItems.AuctionItem
            {
                UniqueId = Guid.NewGuid(),
                TenantId = _abpSession.TenantId.Value,
                AuctionId = hasAuction.Id,
                ItemId = hasItem.Id,
            });

            return new AuctionItemDto
            {
                UniqueId = Guid.NewGuid(),
                AuctionId = input.AuctionId,
                ItemId = input.ItemId,
                IsActive = input.IsActive,
            };
        }

        public async Task Delete(Guid Id)
        {
            var auctionItem = await _auctionitemRepository.FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (auctionItem == null)
                throw new Exception("No AuctionItem found on given Id");

            await _auctionitemRepository.DeleteAsync(auctionItem);
        }

        public async Task<ListResultDto<AuctionItemListDto>> GetAllAuctionItems()
        {
            var auctionItems = await _auctionitemRepository.GetAllIncluding(x => x.Auction, x => x.Item).ToListAsync();
            var result = ObjectMapper.Map<List<AuctionItemListDto>>(auctionItems);
            return new ListResultDto<AuctionItemListDto>(result);
        }

        public async Task<AuctionItemDto> GetAuctionItemById(Guid Id)
        {
            var output = await _auctionitemRepository.GetAllIncluding(x => x.Item, x => x.Auction).FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (output == null)
                throw new Exception("No data found");

            return ObjectMapper.Map<AuctionItemDto>(output);
        }

        public async Task Update(UpdateAuctionItemDto input)
        {
            var auctionItem = await _auctionitemRepository.FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (auctionItem == null)
                throw new Exception("AuctionItem Not found");

            var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AuctionId);
            if (auction == null)
                throw new Exception("Auction Not found");

            var item = await _itemRepository.FirstOrDefaultAsync(x => x.UniqueId == input.ItemId);
            if (item == null)
                throw new Exception("Item Not found");

            //Update the Properties.
            auctionItem.ItemId = item.Id;
            auctionItem.IsActive = input.IsActive;

            await _auctionitemRepository.UpdateAsync(auctionItem);
        }
    }
}
