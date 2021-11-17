using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.ApplicationConfigurations.Dtos;
using NextGen.BiddingPlatform.Dto;

namespace NextGen.BiddingPlatform.ApplicationConfigurations
{
    public interface IApplicationConfigurationsAppService : IApplicationService
    {
        Task<PagedResultDto<GetApplicationConfigurationForViewDto>> GetAll(GetAllApplicationConfigurationsInput input);

        Task<GetApplicationConfigurationForEditOutput> GetApplicationConfigurationForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditApplicationConfigurationDto input);

        Task Delete(EntityDto input);

        Task<string> GetConfigByKey(string configKey, int tenantId=0);

    }
}