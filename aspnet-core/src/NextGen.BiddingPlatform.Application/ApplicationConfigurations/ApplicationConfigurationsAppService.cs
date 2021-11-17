using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using NextGen.BiddingPlatform.ApplicationConfigurations.Dtos;
using NextGen.BiddingPlatform.Dto;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using NextGen.BiddingPlatform.Storage;
using Abp.Domain.Uow;

namespace NextGen.BiddingPlatform.ApplicationConfigurations
{
    public class ApplicationConfigurationsAppService : BiddingPlatformAppServiceBase, IApplicationConfigurationsAppService
    {
        private readonly IRepository<ApplicationConfiguration> _applicationConfigurationRepository;

        public ApplicationConfigurationsAppService(IRepository<ApplicationConfiguration> applicationConfigurationRepository)
        {
            _applicationConfigurationRepository = applicationConfigurationRepository;

        }

        public async Task<PagedResultDto<GetApplicationConfigurationForViewDto>> GetAll(GetAllApplicationConfigurationsInput input)
        {

            var filteredApplicationConfigurations = _applicationConfigurationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ConfigKey.Contains(input.Filter) || e.ConfigValue.Contains(input.Filter));

            var pagedAndFilteredApplicationConfigurations = filteredApplicationConfigurations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var applicationConfigurations = from o in pagedAndFilteredApplicationConfigurations
                                            select new
                                            {

                                                o.ConfigKey,
                                                o.ConfigValue,
                                                Id = o.Id
                                            };

            var totalCount = await filteredApplicationConfigurations.CountAsync();

            var dbList = await applicationConfigurations.ToListAsync();
            var results = new List<GetApplicationConfigurationForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetApplicationConfigurationForViewDto()
                {
                    ApplicationConfiguration = new ApplicationConfigurationDto
                    {

                        ConfigKey = o.ConfigKey,
                        ConfigValue = o.ConfigValue,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetApplicationConfigurationForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ApplicationConfigurations_Edit)]
        public async Task<GetApplicationConfigurationForEditOutput> GetApplicationConfigurationForEdit(EntityDto input)
        {
            var applicationConfiguration = await _applicationConfigurationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetApplicationConfigurationForEditOutput { ApplicationConfiguration = ObjectMapper.Map<CreateOrEditApplicationConfigurationDto>(applicationConfiguration) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditApplicationConfigurationDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ApplicationConfigurations_Create)]
        protected virtual async Task Create(CreateOrEditApplicationConfigurationDto input)
        {
            var applicationConfiguration = ObjectMapper.Map<ApplicationConfiguration>(input);

            if (AbpSession.TenantId != null)
            {
                applicationConfiguration.TenantId = (int)AbpSession.TenantId;
            }

            await _applicationConfigurationRepository.InsertAsync(applicationConfiguration);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ApplicationConfigurations_Edit)]
        protected virtual async Task Update(CreateOrEditApplicationConfigurationDto input)
        {
            var applicationConfiguration = await _applicationConfigurationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, applicationConfiguration);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ApplicationConfigurations_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _applicationConfigurationRepository.DeleteAsync(input.Id);
        }

        public async Task<string> GetConfigByKey(string configKey, int tenantId =0)
        {
            if (tenantId == 0)
            {
                var configFromDb = await _applicationConfigurationRepository.FirstOrDefaultAsync(s => s.ConfigKey == configKey);

                if (configFromDb == null)
                    throw new UserFriendlyException(configKey + " not found in database. Please configure first!!");

                return configFromDb.ConfigValue;
            }
            else {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    var configFromDb = await _applicationConfigurationRepository.FirstOrDefaultAsync(s => s.ConfigKey == configKey && s.TenantId == tenantId);

                    if (configFromDb == null)
                        throw new UserFriendlyException(configKey + " not found in database. Please configure first!!");

                    return configFromDb.ConfigValue;
                }
            }
        }

    }
}