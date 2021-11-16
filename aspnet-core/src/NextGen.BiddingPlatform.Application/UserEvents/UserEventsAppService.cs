using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.UserEvents
{
    public class UserEventsAppService : BiddingPlatformAppServiceBase, IUserEventsAppService
    {
        private readonly IRepository<UserEvent, Guid> _userEventRepository;
        public UserEventsAppService(IRepository<UserEvent, Guid> userEventRepository)
        {
            _userEventRepository = userEventRepository;
        }

        public async Task<List<Guid>> GetUserRegisterEvents(long userId)
        {
            var userEventIds = await _userEventRepository.GetAll().AsNoTracking().Include(s => s.EventFk).Where(x => x.UserId == userId).Select(x => x.EventFk.UniqueId).ToListAsync();

            return userEventIds;
        }
    }
}
