using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Core.Items;
using NextGen.BiddingPlatform.Items.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Runtime.Session;

namespace NextGen.BiddingPlatform.Items
{
    public class ItemService : BiddingPlatformAppServiceBase, IItemService
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<ItemGallery> _itemGalleryRepository;
        private readonly IRepository<ItemCategory.ItemCategory> _itemCategoryRepository;
        private readonly IAbpSession _abpSession;
        public ItemService(IRepository<Item> itemRepository,
                           IRepository<ItemGallery> itemGalleryRepository,
                           IRepository<ItemCategory.ItemCategory> itemCategoryRepository,
                           IAbpSession abpSession)
        {
            _itemRepository = itemRepository;
            _itemGalleryRepository = itemGalleryRepository;
            _itemCategoryRepository = itemCategoryRepository;
            _abpSession = abpSession;
        }
        public async Task<List<ItemListDto>> GetAllItems()
        {
            var items = await _itemRepository.GetAll()
                                             .Select(x => new ItemListDto
                                             {
                                                 UniqueId = x.UniqueId,
                                                 Description = x.Description,
                                                 ItemName = x.ItemName,
                                                 ItemNumber = x.ItemNumber,
                                                 ItemType = x.ItemType,
                                                 MainImageName = x.MainImageName,
                                                 ItemStatus = x.ItemStatus
                                             }).ToListAsync();
            return items;
        }

        public async Task<PagedResultDto<ItemListDto>> GetItemsWithFilter(ItemFilter input)
        {
            var query = _itemRepository.GetAll()
                                       .WhereIf(!input.Search.IsNullOrWhiteSpace(), x => x.ItemName.ToLower().IndexOf(input.Search.ToLower()) > -1)
                                       .Select(x => new ItemListDto
                                       {
                                           UniqueId = x.UniqueId,
                                           Description = x.Description,
                                           ItemName = x.ItemName,
                                           ItemNumber = x.ItemNumber,
                                           ItemType = x.ItemType,
                                           MainImageName = x.MainImageName,
                                           ItemStatus = x.ItemStatus
                                       });

            var resultCount = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);

            query = query.PageBy(input);

            return new PagedResultDto<ItemListDto>(resultCount, await query.ToListAsync());
        }

        public async Task CreateItem(ItemDto input)
        {
            //if (input.ItemCategories.Count() == 0)
            //    throw new Exception("Please select at least one category for item");

            if (!_abpSession.TenantId.HasValue)
                throw new Exception("You are not authorized user");

            var mappedItem = ObjectMapper.Map<Item>(input);
            mappedItem.UniqueId = Guid.NewGuid();
            mappedItem.TenantId = _abpSession.TenantId.Value;
            //add category to ItemCategory Table
            //foreach (var category in input.ItemCategories)
            //{

            //}
            //add images to ItemGallery Table
            foreach (var image in input.ItemImages)
            {
                mappedItem.ItemImages.Add(new ItemGallery
                {
                    UniqueId = Guid.NewGuid(),
                    ImageName = image.ImageName,
                    Thumbnail = image.Thumbnail,
                    Title = image.Title,
                    Description = image.Description
                });
            }

            await _itemRepository.InsertAsync(mappedItem);
        }

        public async Task UpdateItem(ItemDto input)
        {
            var existingItem = await _itemRepository.FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (existingItem == null)
                throw new Exception("Item not available for given id");

            //first remove gallery
            var itemImages = await _itemGalleryRepository.GetAllListAsync(x => x.ItemId == existingItem.Id);
            if (itemImages.Count > 0)
                await _itemGalleryRepository.DeleteAsync(x => x.ItemId == existingItem.Id);

            //second remove categories
            var itemCategories = await _itemCategoryRepository.GetAllListAsync(x => x.ItemId == existingItem.Id);
            if (itemCategories.Count > 0)
                await _itemCategoryRepository.DeleteAsync(x => x.ItemId == existingItem.Id);

            //add category to ItemCategory Table
            //foreach (var category in input.ItemCategories)
            //{

            //}
            //add images to ItemGallery Table
            foreach (var image in input.ItemImages)
            {
                existingItem.ItemImages.Add(new ItemGallery
                {
                    UniqueId = Guid.NewGuid(),
                    ImageName = image.ImageName,
                    Thumbnail = image.Thumbnail,
                    Title = image.Title,
                    Description = image.Description
                });
            }
            //update the properties
            existingItem.ItemType = input.ItemType;
            existingItem.ItemNumber = input.ItemNumber;
            existingItem.ItemName = input.ItemName;
            existingItem.Description = input.Description;
            existingItem.ProcurementState = input.ProcurementState;
            existingItem.ItemStatus = input.ItemStatus;
            existingItem.Visibility = input.Visibility;
            existingItem.FairMarketValue_FMV = input.FairMarketValue_FMV;
            existingItem.StartingBidValue = input.StartingBidValue;
            existingItem.BidStepIncrementValue = input.BidStepIncrementValue;
            existingItem.AcquisitionValue = input.AcquisitionValue;
            existingItem.BuyNowPrice = input.BuyNowPrice;
            existingItem.ItemCertificateNotes = input.ItemCertificateNotes;
            existingItem.MainImageName = input.MainImageName;
            existingItem.VideoLink = input.VideoLink;
        }

        public async Task DeleteItem(Guid Id)
        {
            var item = await _itemRepository.FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (item == null)
                throw new Exception("Item not found for give id");

            await _itemRepository.DeleteAsync(item);
        }
    }
}
