using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.Items.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Items
{
    public interface IItemAppService : IApplicationService
    {
        Task<List<ItemListDto>> GetAllItems();
        Task<long> GetNextItemNumber();
        Task<PagedResultDto<ItemListDto>> GetItemsWithFilter(ItemFilter input);
        Task<UpdateItemDto> GetItemById(Guid Id);
        Task<ItemDto> CreateItem(ItemDto input);
        Task UpdateItem(UpdateItemDto input);
        Task DeleteItem(Guid Id);
        Dropdowns GetDropdowns();
        Task<List<ItemSelectDto>> GetItems();
    }
}
