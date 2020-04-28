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
        Task<PagedResultDto<ItemListDto>> GetItemsWithFilter(ItemFilter input);
        Task<GetItemDto> GetItemById(Guid Id);
        Task CreateItem(ItemDto input);
        Task UpdateItem(UpdateItemDto input);
        Task DeleteItem(Guid Id);
    }
}
