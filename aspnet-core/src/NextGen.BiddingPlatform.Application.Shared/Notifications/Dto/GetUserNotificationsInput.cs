using System;
using Abp.Notifications;
using NextGen.BiddingPlatform.Dto;

namespace NextGen.BiddingPlatform.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}