using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.Category.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Category
{
    public interface ICategoryAppService : IApplicationService
    {
        Task<CategoryDto> GetCategoryById(Guid Id);
        Task<ListResultDto<CategoryListDto>> GetAllCategory();
        Task<PagedResultDto<CategoryListDto>> GetCategoryWithFilter(CategoryFilter input);
        Task<CategoryDto> Create(CreateCategoryDto input);
        Task CreateSubCategory(CreateSubCategoryDto input);
        Task Update(UpdateCategoryDto input);
        Task Delete(Guid input);
    }
}
