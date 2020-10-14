using Microsoft.AspNetCore.SignalR;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Application
{
    public class BidHub : Hub
    {
        public async Task SaveBid(string auctionHistoryDto)
        {
            await Clients.All.SendAsync("BidSaved", auctionHistoryDto);
        }
    }
    
}
