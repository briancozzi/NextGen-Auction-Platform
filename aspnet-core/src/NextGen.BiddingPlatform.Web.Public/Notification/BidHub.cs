using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Web.Public.Notification
{
    public class BidHub : Hub
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConnectionManager _connectionManager;
        public BidHub(IHttpContextAccessor httpContext, IConnectionManager connectionManager)
        {
            _httpContext = httpContext;
            _connectionManager = connectionManager;
        }
        public void GetAndStoreConnection(string auctionItemId)
        {
            if (!string.IsNullOrEmpty(auctionItemId))
            {
                _connectionManager.Add(auctionItemId, Context.ConnectionId);
            }
        }

        public void StoreCurrentBidConnections()
        {
            _connectionManager.Add("CurrentUserBids", Context.ConnectionId);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var auctionItemId = _httpContext.HttpContext.Request.Query["auctionItemId"];
            if (!string.IsNullOrEmpty(auctionItemId))
            {
                _connectionManager.Remove(auctionItemId, Context.ConnectionId);
            }
            _connectionManager.Remove("CurrentUserBids", Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
