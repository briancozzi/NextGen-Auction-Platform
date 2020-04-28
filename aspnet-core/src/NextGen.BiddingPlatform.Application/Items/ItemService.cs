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

namespace NextGen.BiddingPlatform.Items
{
    public class ItemService : BiddingPlatformAppServiceBase, IItemService
    {
        private readonly IRepository<Item> _itemRepository;
        public ItemService(IRepository<Item> itemRepository)
        {
            _itemRepository = itemRepository;
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

        //create and update method is remaining
        public async Task DeleteItem(Guid Id)
        {
            var item = await _itemRepository.FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (item == null)
                throw new Exception("Item not found for give id");

            await _itemRepository.DeleteAsync(item);
        }
    }
}
