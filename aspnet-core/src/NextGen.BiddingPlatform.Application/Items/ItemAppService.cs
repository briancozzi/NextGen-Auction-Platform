using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Core.Items;
using NextGen.BiddingPlatform.Items.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using NextGen.BiddingPlatform.Enums;
using NextGen.BiddingPlatform.Authorization.Users;
using Abp.UI;
using Abp.Authorization;
using Abp.Webhooks;
using NextGen.BiddingPlatform.WebHooks;
using NextGen.BiddingPlatform.AppAccountEvent.Dto;

namespace NextGen.BiddingPlatform.Items
{
    [AbpAuthorize]
    public class ItemAppService : BiddingPlatformAppServiceBase, IItemAppService
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<ItemGallery> _itemGalleryRepository;
        private readonly IRepository<ItemCategory.ItemCategory> _itemCategoryRepository;
        private readonly IRepository<Core.AppAccounts.AppAccount> _appAccountRepository;
        private readonly IUserAppService _userAppService;
        private readonly IRepository<Core.AuctionItems.AuctionItem> _auctionItemRepository;
        private readonly IWebhookPublisher _webHookPublisher;
        public ItemAppService(IRepository<Item> itemRepository,
                           IRepository<ItemGallery> itemGalleryRepository,
                           IRepository<ItemCategory.ItemCategory> itemCategoryRepository,
                           IRepository<Core.AppAccounts.AppAccount> appAccountRepository,
                           IUserAppService userAppService,
                           IRepository<Core.AuctionItems.AuctionItem> auctionItemRepository,
                           IWebhookPublisher webHookPublisher)
        {
            _itemRepository = itemRepository;
            _itemGalleryRepository = itemGalleryRepository;
            _itemCategoryRepository = itemCategoryRepository;
            _appAccountRepository = appAccountRepository;
            _userAppService = userAppService;
            _auctionItemRepository = auctionItemRepository;
            _webHookPublisher = webHookPublisher;
        }

        public async Task<List<ItemListDto>> GetAllItems()
        {
            var currUser = _userAppService.GetCurrUser();
            var query = _itemRepository.GetAllIncluding(x => x.AppAccount);
            if (currUser.AppAccountId.HasValue)
                query = query.Where(x => x.AppAccountId == currUser.AppAccountId.Value);

            var items = await query.Select(x => new ItemListDto
            {
                UniqueId = x.UniqueId,
                Description = x.Description,
                ItemName = x.ItemName,
                ItemNumber = x.ItemNumber,
                MainImageName = x.MainImageName,
                ThumbnailImage = x.ThumbnailImage,
                ItemStatus = x.ItemStatus,
                AppAccountName = x.AppAccount.FirstName + " " + x.AppAccount.LastName
            }).ToListAsync();

            foreach (var item in items)
            {
                item.ItemStatusName = GetItemStatus(item.ItemStatus);
            }
            return items;
        }

        public async Task<long> GetNextItemNumber()
        {
            using (CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.SoftDelete))
            {
                var totalItemsCount = await _itemRepository.GetAll().AsNoTracking().CountAsync();
                return 100 + totalItemsCount;
            }

        }

        public async Task<PagedResultDto<ItemListDto>> GetItemsWithFilter(ItemFilter input)
        {
            var currUser = _userAppService.GetCurrUser();
            var query = _itemRepository.GetAllIncluding(x => x.AppAccount);
            if (currUser.AppAccountId.HasValue)
                query = query.Where(x => x.AppAccountId == currUser.AppAccountId.Value);

            var result = query.WhereIf(!input.Search.IsNullOrWhiteSpace(), x => x.ItemName.ToLower().IndexOf(input.Search.ToLower()) > -1)
                                .Select(x => new ItemListDto
                                {
                                    UniqueId = x.UniqueId,
                                    Description = x.Description,
                                    ItemName = x.ItemName,
                                    ItemNumber = x.ItemNumber,
                                    MainImageName = x.MainImageName,
                                    ThumbnailImage = x.ThumbnailImage,
                                    ItemStatus = x.ItemStatus,
                                    AppAccountName = x.AppAccount.FirstName + " " + x.AppAccount.LastName
                                });

            var resultCount = await result.CountAsync();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
                result = result.OrderBy(input.Sorting);

            var resultQuery = result.PageBy(input).ToList();

            foreach (var item in resultQuery)
            {
                item.ItemStatusName = GetItemStatus(item.ItemStatus);
            }

            return new PagedResultDto<ItemListDto>(resultCount, resultQuery);
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

        public async Task<ItemDto> CreateItem(ItemDto input)
        {
            try
            {
                //var account = await _appAccountRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AppAccountUniqueId);
                //if (account == null)
                //    throw new UserFriendlyException("AppAccount not found for given id");

                var currUser = await UserManager.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == AbpSession.UserId);
                if (currUser == null)
                    throw new UserFriendlyException("User not found!!");

                if (!currUser.AppAccountId.HasValue)
                    throw new UserFriendlyException("Sorry you do not have permission to add items. Please contact your admin.");

                //if (input.Categories.Count() == 0)
                //    throw new UserFriendlyException("Please select at least one category for item");

                if (!AbpSession.TenantId.HasValue)
                    throw new UserFriendlyException("You are not authorized user");

                var mappedItem = ObjectMapper.Map<Item>(input);
                mappedItem.AppAccountId = currUser.AppAccountId.Value;
                mappedItem.UniqueId = Guid.NewGuid();
                mappedItem.TenantId = AbpSession.TenantId.Value;

                mappedItem.ItemImages = new List<ItemGallery>();
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

                var itemId = await _itemRepository.InsertAndGetIdAsync(mappedItem);
                await CurrentUnitOfWork.SaveChangesAsync();

                input.ItemUniqueId = mappedItem.UniqueId;

                return input;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateItem(UpdateItemDto input)
        {
            try
            {
                var existingItem = await _itemRepository.FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
                if (existingItem == null)
                    throw new UserFriendlyException("Item not available for given id");

                var account = await _appAccountRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AppAccountUniqueId);
                if (account == null)
                    throw new UserFriendlyException("AppAccount not found for given id");

                //if (input.Categories.Count() == 0)
                //    throw new UserFriendlyException("Please select at least one category for item");

                //first remove gallery
                foreach (var item in input.RemoveImageIds)
                {
                    await _itemGalleryRepository.DeleteAsync(x => x.Id == item);
                }

                //second remove categories
                //var itemCategories = await _itemCategoryRepository.GetAllListAsync(x => x.ItemId == existingItem.Id);
                //if (itemCategories.Count > 0)
                //    await _itemCategoryRepository.DeleteAsync(x => x.ItemId == existingItem.Id);

                //add category to ItemCategory Table
                //foreach (var category in input.Categories)
                //{
                //    existingItem.ItemCategories.Add(new ItemCategory.ItemCategory
                //    {
                //        UniqueId = Guid.NewGuid(),
                //        CategoryId = category
                //    });
                //}
                //add images to ItemGallery Table

                foreach (var image in input.ItemImages)
                {
                    if (image.Id == 0)
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
                existingItem.CategoryId = input.CategoryId;
                existingItem.IsHide = input.IsHide;
                await _itemRepository.UpdateAsync(existingItem);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteItem(Guid Id)
        {
            var item = await _itemRepository.FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (item == null)
                throw new Exception("Item not found for give id");

            await _itemRepository.DeleteAsync(item);
        }

        public async Task<List<ItemSelectDto>> GetItems()
        {
            var currUser = _userAppService.GetCurrUser();
            if (currUser.AppAccountId.HasValue)
                return await _itemRepository.GetAllIncluding(x => x.AppAccount).Where(x => x.AppAccountId == currUser.AppAccountId.Value)
                                            .Select(x => new ItemSelectDto
                                            {
                                                UniqueId = x.UniqueId,
                                                ItemName = x.ItemName,
                                                Id = x.Id
                                            }).ToListAsync();
            else
                return await _itemRepository.GetAllIncluding(x => x.AppAccount)
                                                .Select(x => new ItemSelectDto
                                                {
                                                    UniqueId = x.UniqueId,
                                                    ItemName = x.ItemName,
                                                    Id = x.Id
                                                }).ToListAsync();

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

        public async Task CloseBiddingOnItem(Guid itemId)
        {
            var item = await _itemRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(s => s.UniqueId == itemId);

            if (item == null)
                throw new UserFriendlyException("Item not found!!");

            var auctionItem = await _auctionItemRepository.FirstOrDefaultAsync(s => s.ItemId == item.Id);
            if (auctionItem == null)
                throw new UserFriendlyException("Auction item not found!!");

            auctionItem.IsBiddingClosed = true;
            await _auctionItemRepository.UpdateAsync(auctionItem);
            await CurrentUnitOfWork.SaveChangesAsync();

            await _webHookPublisher.PublishAsync(AppWebHookNames.CloseBiddingOnEventOrItem,
                new CloseEventOrItemDto
                {
                    AuctionItemIds = new List<Guid>() { auctionItem.UniqueId },
                    TenantId = AbpSession.TenantId
                }, AbpSession.TenantId);
        }
    }
}
