﻿using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.Category.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Runtime.Session;
using System.Linq;

namespace NextGen.BiddingPlatform.Category
{
    public class CategoryAppService : BiddingPlatformAppServiceBase, ICategoryAppService
    {
        private readonly IRepository<Core.Categories.Category> _categoryRepository;
        private readonly IAbpSession _abpSession;
        public CategoryAppService(IRepository<Core.Categories.Category> categoryRepository,
                                  IAbpSession abpSession)
        {
            _categoryRepository = categoryRepository;
            _abpSession = abpSession;
        }

        public async Task<CategoryDto> GetCategoryById(Guid Id)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (category == null)
                throw new Exception("Category not found for given id");

            return ObjectMapper.Map<CategoryDto>(category);
        }

        public async Task<ListResultDto<CategoryListDto>> GetAllCategory()
        {
            var categories = await _categoryRepository.GetAll()
                                                      .Select(x => new CategoryListDto
                                                      {
                                                          Id = x.Id,
                                                          UniqueId = x.UniqueId,
                                                          CategoryName = x.CategoryName
                                                      })
                                                      .ToListAsync();

            return new ListResultDto<CategoryListDto>(categories);
        }

        public async Task<CategoryDto> Create(CreateCategoryDto input)
        {
            var category = await _categoryRepository.InsertAsync(new Core.Categories.Category
            {
                UniqueId = Guid.NewGuid(),
                TenantId = _abpSession.TenantId.Value,
                CategoryName = input.CategoryName
            });
            return new CategoryDto
            {
                UniqueId = category.UniqueId,
                CategoryName = category.CategoryName
            };
        }

        public async Task CreateSubCategory(CreateSubCategoryDto input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.CategoryId);
            if (category == null)
                throw new Exception("Category not found for given id");

            await _categoryRepository.InsertAsync(new Core.Categories.Category
            {
                UniqueId = Guid.NewGuid(),
                CategoryName = input.SubCategoryName,
                ParentId = category.Id,
                TenantId = _abpSession.TenantId.Value
            });

        }

        public async Task Update(UpdateCategoryDto input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (category == null)
                throw new Exception("Category not found for given id");

            category.CategoryName = input.CategoryName;
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task Delete(EntityDto<Guid> input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Id);
            if (category == null)
                throw new Exception("Category data not found for given id");

            var hasSubcategories = await _categoryRepository.GetAllListAsync(x => x.ParentId == category.Id);
            if (hasSubcategories.Count > 0)
                throw new Exception("Current category have subcategories, Please first delete subcategories");

            await _categoryRepository.DeleteAsync(category);
        }
    }
}