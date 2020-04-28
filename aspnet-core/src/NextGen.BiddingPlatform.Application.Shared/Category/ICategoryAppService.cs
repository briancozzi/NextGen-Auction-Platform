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
        Task<ListResultDto<CategoryListDto>> GetAllCategory();
        Task<UpdateCategoryDto> Update(UpdateCategoryDto input);
        Task<CreateCategoryDto> CreateSubCategory(CreateCategoryDto input);
        Task<CreateCategoryDto> Create(CreateCategoryDto input);
        Task Delete(EntityDto<Guid> input);
        
    }
}
