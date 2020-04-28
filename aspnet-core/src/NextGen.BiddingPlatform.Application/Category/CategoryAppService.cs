using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.Category.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Runtime.Session;

namespace NextGen.BiddingPlatform.Category
{
    public class CategoryAppService : BiddingPlatformAppServiceBase, ICategoryAppService
    {
        private readonly IRepository<Core.Categories.Category> _categoryRepository;
        private readonly IAbpSession _abpSession;
        public CategoryAppService(IRepository<Core.Categories.Category> categoryRepository,IAbpSession abpSession)
        {
            _categoryRepository = categoryRepository;
            _abpSession = abpSession;
        }

        public async Task<CreateCategoryDto> Create(CreateCategoryDto input)
        {
            var output = ObjectMapper.Map<Core.Categories.Category>(input);
            output.UniqueId = Guid.NewGuid();
            output.TenantId = _abpSession.TenantId.Value;
            await _categoryRepository.InsertAsync(output);
            return input;
        }

        public async Task<CreateCategoryDto> CreateSubCategory(CreateCategoryDto input)
        {
            var data = await _categoryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (data == null)
                throw new Exception("Category not found for given id");

            await _categoryRepository.InsertAsync(new Core.Categories.Category
            {
                UniqueId = Guid.NewGuid(),
                CategoryName = input.CategoryName,
                ParentId = data.Id,
                TenantId = data.TenantId
            });
            return input;
        }

        public async Task Delete(EntityDto<Guid> input)
        {
            var category = await _categoryRepository.GetAll().FirstOrDefaultAsync(x => x.UniqueId == input.Id);
            if (category == null)
                throw new Exception("No data found");

            await _categoryRepository.DeleteAsync(category);
        }

        public async Task<ListResultDto<CategoryListDto>> GetAllCategory()
        {
            var categories = await _categoryRepository.GetAllListAsync();
            var result = ObjectMapper.Map<List<CategoryListDto>>(categories);
            return new ListResultDto<CategoryListDto>(result);
        }

        public async Task<CategoryDto> GetCategoryById(Guid Id)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (category == null)
                throw new Exception("No data found");

            return ObjectMapper.Map<CategoryDto>(category);
        }

        public async Task<UpdateCategoryDto> Update(UpdateCategoryDto input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (category == null)
                throw new Exception("No data found");

            category.CategoryName = input.CategoryName;
            await _categoryRepository.UpdateAsync(category);
            return input;
        }
    }
}
