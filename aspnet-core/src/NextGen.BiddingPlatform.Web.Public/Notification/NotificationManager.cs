using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Web.Public.Notification
{
    public class NotificationManager : INotificationManager
    {
        private readonly IConnectionManager _connectionManager;
        private readonly IHubContext<BidHub> _context;
        public NotificationManager(IConnectionManager connectionManager, IHubContext<BidHub> context)
        {
            _connectionManager = connectionManager;
            _context = context;
        }
        public async Task SendAsync(string auctionItemId, object data)
        {
            var connections = _connectionManager.GetConnections(auctionItemId);
            if (connections != null && connections.Count() > 0)
            {
                await _context.Clients.Clients(connections.ToList()).SendAsync("BidSaved", data);
            }
        }
        public async Task UpdateCurrentBidsAsync()
        {
            var connections = _connectionManager.GetConnections("CurrentUserBids");
            if (connections != null && connections.Count() > 0)
            {
                await _context.Clients.Clients(connections.ToList()).SendAsync("UpdateCurrentUserBids");
            }
        }
    }
}
