using Abp.Application.Services;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.BackgroundService.RabbitMqService
{
    public interface IRabbitMqService : IApplicationService
    {
        void AddToQueue(AuctionBidderHistoryDto data);
    }
}
