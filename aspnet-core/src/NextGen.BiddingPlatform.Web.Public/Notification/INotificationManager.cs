using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Web.Public.Notification
{
    public interface INotificationManager
    {
        Task SendAsync(string auctionItemId, object data);
        Task UpdateCurrentBidsAsync();
    }
}
