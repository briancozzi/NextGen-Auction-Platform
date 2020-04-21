using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.State.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.State
{
    public interface IStateAppService : IApplicationService
    {
        Task<List<StateListDto>> GetAllStates();
        Task<StateDto> Create(CreateStateDto input);
        Task<UpdateStateDto> Update(UpdateStateDto input);
        Task Delete(EntityDto<Guid> input);
        Task<StateDto> GetStateById(Guid input);
    }
}
