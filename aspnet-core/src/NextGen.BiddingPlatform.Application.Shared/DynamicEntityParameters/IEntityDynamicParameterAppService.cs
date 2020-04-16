using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.DynamicEntityParameters.Dto;
using NextGen.BiddingPlatform.EntityDynamicParameters;

namespace NextGen.BiddingPlatform.DynamicEntityParameters
{
    public interface IEntityDynamicParameterAppService
    {
        Task<EntityDynamicParameterDto> Get(int id);

        Task<ListResultDto<EntityDynamicParameterDto>> GetAllParametersOfAnEntity(EntityDynamicParameterGetAllInput input);

        Task<ListResultDto<EntityDynamicParameterDto>> GetAll();

        Task Add(EntityDynamicParameterDto dto);

        Task Update(EntityDynamicParameterDto dto);

        Task Delete(int id);
    }
}
