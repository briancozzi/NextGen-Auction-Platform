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
using NextGen.BiddingPlatform.Enums;

namespace NextGen.BiddingPlatform.Items
{
    public class ItemAppService : BiddingPlatformAppServiceBase, IItemAppService
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<ItemGallery> _itemGalleryRepository;
        private readonly IRepository<ItemCategory.ItemCategory> _itemCategoryRepository;
        private readonly IRepository<Core.AppAccounts.AppAccount> _appAccountRepository;
        private readonly IAbpSession _abpSession;
        public ItemAppService(IRepository<Item> itemRepository,
                           IRepository<ItemGallery> itemGalleryRepository,
                           IRepository<ItemCategory.ItemCategory> itemCategoryRepository,
                           IRepository<Core.AppAccounts.AppAccount> appAccountRepository,
                           IAbpSession abpSession)
        {
            _itemRepository = itemRepository;
            _itemGalleryRepository = itemGalleryRepository;
            _itemCategoryRepository = itemCategoryRepository;
            _appAccountRepository = appAccountRepository;
            _abpSession = abpSession;
        }
        public async Task<List<ItemListDto>> GetAllItems()
        {
            var items = await _itemRepository.GetAllIncluding(x => x.AppAccount)
                                             .Select(x => new ItemListDto
                                             {
                                                 UniqueId = x.UniqueId,
                                                 Description = x.Description,
                                                 ItemName = x.ItemName,
                                                 ItemNumber = x.ItemNumber,
                                                 MainImageName = x.MainImageName,
                                                 ThumbnailImage = x.ThumbnailImage,
                                                 ItemStatus = x.ItemStatus,
                                                 ItemStatusName = GetItemStatus(x.ItemStatus),
                                                 AppAccountName = x.AppAccount.FirstName + " " + x.AppAccount.LastName
                                             }).ToListAsync();
            return items;
        }

        public async Task<PagedResultDto<ItemListDto>> GetItemsWithFilter(ItemFilter input)
        {
            var query = _itemRepository.GetAllIncluding(x => x.AppAccount)
                                       .WhereIf(!input.Search.IsNullOrWhiteSpace(), x => x.ItemName.ToLower().IndexOf(input.Search.ToLower()) > -1)
                                       .Select(x => new ItemListDto
                                       {
                                           UniqueId = x.UniqueId,
                                           Description = x.Description,
                                           ItemName = x.ItemName,
                                           ItemNumber = x.ItemNumber,
                                           MainImageName = x.MainImageName,
                                           ThumbnailImage = x.ThumbnailImage,
                                           ItemStatus = x.ItemStatus,
                                           ItemStatusName = GetItemStatus(x.ItemStatus),
                                           AppAccountName = x.AppAccount.FirstName + " " + x.AppAccount.LastName
                                       });

            var resultCount = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);

            query = query.PageBy(input);

            return new PagedResultDto<ItemListDto>(resultCount, await query.ToListAsync());
        }

        public async Task<UpdateItemDto> GetItemById(Guid Id)
        {
            var existingItem = await _itemRepository.GetAllIncluding(x => x.ItemImages, x => x.AppAccount).FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (existingItem == null)
                throw new Exception("Item not available for given id");

            var mappedItem = ObjectMapper.Map<UpdateItemDto>(existingItem);
            var itemCategories = await _itemCategoryRepository.GetAllIncluding(x => x.Category)
                                                              .Where(x => x.ItemId == existingItem.Id)
                                                              .Select(x => x.CategoryId)
                                                              .ToListAsync();

            mappedItem.Categories = itemCategories;
            mappedItem.AppAccountUniqueId = existingItem.AppAccount.UniqueId;
            return mappedItem;
        }

        public async Task CreateItem(ItemDto input)
        {
            var account = await _appAccountRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AppAccountUniqueId);
            if (account == null)
                throw new Exception("AppAccount not found for given id");
            if (input.Categories.Count() == 0)
                throw new Exception("Please select at least one category for item");

            if (!_abpSession.TenantId.HasValue)
                throw new Exception("You are not authorized user");

            var mappedItem = ObjectMapper.Map<Item>(input);
            mappedItem.AppAccountId = account.Id;
            mappedItem.UniqueId = Guid.NewGuid();
            mappedItem.TenantId = _abpSession.TenantId.Value;
            //add category to ItemCategory Table
            foreach (var category in input.Categories)
            {
                mappedItem.ItemCategories.Add(new ItemCategory.ItemCategory
                {
                    UniqueId = Guid.NewGuid(),
                    CategoryId = category
                });
            }
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

        public async Task UpdateItem(UpdateItemDto input)
        {
            var existingItem = await _itemRepository.FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (existingItem == null)
                throw new Exception("Item not available for given id");

            var account = await _appAccountRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AppAccountUniqueId);
            if (account == null)
                throw new Exception("AppAccount not found for given id");

            if (input.Categories.Count() == 0)
                throw new Exception("Please select at least one category for item");

            //first remove gallery
            var itemImages = await _itemGalleryRepository.GetAllListAsync(x => x.ItemId == existingItem.Id);
            if (itemImages.Count > 0)
                await _itemGalleryRepository.DeleteAsync(x => x.ItemId == existingItem.Id);

            //second remove categories
            var itemCategories = await _itemCategoryRepository.GetAllListAsync(x => x.ItemId == existingItem.Id);
            if (itemCategories.Count > 0)
                await _itemCategoryRepository.DeleteAsync(x => x.ItemId == existingItem.Id);

            //add category to ItemCategory Table
            foreach (var category in input.Categories)
            {
                existingItem.ItemCategories.Add(new ItemCategory.ItemCategory
                {
                    UniqueId = Guid.NewGuid(),
                    CategoryId = category
                });
            }
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
            //existingItem.ItemType = input.ItemType;
            existingItem.AppAccountId = account.Id;
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
            existingItem.ThumbnailImage = input.ThumbnailImage;
            existingItem.VideoLink = input.VideoLink;
            await _itemRepository.UpdateAsync(existingItem);
        }

        public async Task DeleteItem(Guid Id)
        {
            var item = await _itemRepository.FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (item == null)
                throw new Exception("Item not found for give id");

            await _itemRepository.DeleteAsync(item);
        }

        public Dropdowns GetDropdowns()
        {
            Dropdowns dropdowns = new Dropdowns
            {
                ProcurementStates = Utility.GetProcurementStateList(),
                ItemStatus = Utility.GetItemStatusList(),
                Visibilities = Utility.GetVisibilityList()
            };
            return dropdowns;
        }

        private string GetItemStatus(int id)
        {
            var itemStatuses = Utility.GetItemStatusList();
            var itemStatus = itemStatuses.FirstOrDefault(x => x.Id == id);
            return itemStatus?.Name;
        }
        private string GetProcurementState(int id)
        {
            var procurementStates = Utility.GetProcurementStateList();
            var precurementState = procurementStates.FirstOrDefault(x => x.Id == id);
            return precurementState?.Name;
        }
        private string GetVisibility(int id)
        {
            var visibilities = Utility.GetVisibilityList();
            var visibility = visibilities.FirstOrDefault(x => x.Id == id);
            return visibility?.Name;
        }
    }
}
