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
        Task<ListResultDto<AppAccountListDto>> GetAllAccount();
        Task<CreateAppAccountDto> Create(CreateAppAccountDto input);
        Task<UpdateAppAccountDto> Update(UpdateAppAccountDto input);
        Task Delete(Guid Id);
        Task<AppAccountDto> GetAccountById(Guid input);
        Task<PagedResultDto<AppAccountListDto>> GetAllAccountFilter(AppAccountFilter input);
    }
}
