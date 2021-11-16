using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.UserEvents
{
    public interface IUserEventsAppService : IApplicationService
    {
        Task<List<Guid>> GetUserRegisterEvents(long userId);
    }
}
