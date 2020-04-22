using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.AppAccount.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace NextGen.BiddingPlatform.AppAccount
{
    public interface IAppAccountAppService : IApplicationService
    {
        Task<List<AppAccountListDto>> GetAllAccount();
        Task Create(CreateAppAccountDto input);
        Task Update(UpdateAppAccountDto input);
        Task Delete(EntityDto<Guid> input);
        Task<AppAccountDto> GetAccountById(Guid input);

    }
}
