using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.Items.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Items
{
    public interface IItemService : IApplicationService
    {
        Task<List<ItemListDto>> GetAllItems();
        Task<PagedResultDto<ItemListDto>> GetItemsWithFilter(ItemFilter input);
        Task CreateItem(ItemDto input);
        Task UpdateItem(ItemDto input);
        Task DeleteItem(Guid Id);
    }
}
