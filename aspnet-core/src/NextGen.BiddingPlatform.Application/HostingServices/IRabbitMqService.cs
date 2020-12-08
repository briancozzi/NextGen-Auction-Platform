using Abp.Application.Services;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.BackgroundService.RabbitMqService
{
    public interface IRabbitMqService : IApplicationService
    {
        Task AddToQueue(AuctionBidderHistoryDto data);
    }
}
