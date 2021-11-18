using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Core.AppAccountEvents;
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
        private readonly IRepository<Event> _eventRepository;
        public UserEventsAppService(IRepository<UserEvent, Guid> userEventRepository,
                                    IRepository<Event> eventRepository)
        {
            _userEventRepository = userEventRepository;
            _eventRepository = eventRepository;
        }

        public async Task<List<Guid>> GetUserRegisterEvents(long userId)
        {
            var userEventIds = await _userEventRepository.GetAll().AsNoTracking().Include(s => s.EventFk).Where(x => x.UserId == userId).Select(x => x.EventFk.UniqueId).ToListAsync();

            return userEventIds;
        }

        public async Task<bool> IsUserRegisteredForAnEvent(long userId, Guid eventId, int tenantId)
        {

            using (CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant))
            {
                var @event = await _eventRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(s => s.UniqueId == eventId && s.TenantId == tenantId);
                if (@event == null)
                    throw new UserFriendlyException("Event not found!!");

                var isUserRegisterForEvent = await _userEventRepository.GetAll().AsNoTracking().AnyAsync(s => s.UserId == userId && s.EventId == @event.Id && s.TenantId == tenantId);
                return isUserRegisterForEvent;
            }
        }
    }
}
