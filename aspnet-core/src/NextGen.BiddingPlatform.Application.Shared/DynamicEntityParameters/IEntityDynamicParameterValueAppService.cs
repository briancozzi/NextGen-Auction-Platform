using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.DynamicEntityParameters.Dto;
using NextGen.BiddingPlatform.EntityDynamicParameterValues.Dto;

namespace NextGen.BiddingPlatform.DynamicEntityParameters
{
    public interface IEntityDynamicParameterValueAppService
    {
        Task<EntityDynamicParameterValueDto> Get(int id);

        Task<ListResultDto<EntityDynamicParameterValueDto>> GetAll(GetAllInput input);

        Task Add(EntityDynamicParameterValueDto input);

        Task Update(EntityDynamicParameterValueDto input);

        Task Delete(int id);

        Task<GetAllEntityDynamicParameterValuesOutput> GetAllEntityDynamicParameterValues(GetAllEntityDynamicParameterValuesInput input);
    }
}
