using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.AppAccountEvent.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.AppAccountEvent
{
    public interface IAccountEventAppService : IApplicationService
    {
        Task<ListResultDto<AccountEventListDto>> GetAllAccountEvents();
        Task<PagedResultDto<AccountEventListDto>> GetAccountEventsWithFilter(AccountEventFilter input);
        Task<CreateAccountEventDto> Create(CreateAccountEventDto input);
        Task<UpdateAccountEventDto> Update(UpdateAccountEventDto input);
        Task Delete(EntityDto<Guid> input);
        Task<AccountEventDto> GetAccountEventById(Guid input);
        Task<AccountEventListDto> GetEventDateTimeByEventId(Guid Id);
    }
}
