using Abp.Authorization;
using Abp.Runtime.Caching;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NextGen.BiddingPlatform.AuctionHistory;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using NextGen.BiddingPlatform.Caching;
using NextGen.BiddingPlatform.Configuration;
using NextGen.BiddingPlatform.RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.BackgroundService.RabbitMqService
{
    //[AbpAuthorize]
    public class RabbitMqService : BiddingPlatformAppServiceBase, IRabbitMqService
    {
        private readonly RabbitMqSettings _rabbitMqSettings;
        private ConnectionFactory factory;
        private readonly string EnabledQueue;
        private readonly IAuctionHistoryAppService _auctionHistoryService;
        private readonly ICachingAppService _cacheAppService;
        private readonly IConfigurationRoot _appConfiguration;
        public RabbitMqService(IOptions<RabbitMqSettings> rabbitMqSettings,
                                                    IAuctionHistoryAppService auctionHistoryAppService,
                                                    ICachingAppService cacheAppService,
                                                    IWebHostEnvironment env)
        {
            _rabbitMqSettings = rabbitMqSettings.Value;
            _auctionHistoryService = auctionHistoryAppService;
            _cacheAppService = cacheAppService;
            _appConfiguration = env.GetAppConfiguration();
            EnabledQueue = _appConfiguration["EnabledQueue"];
        }
        public async Task AddToQueue(AuctionBidderHistoryDto data)
        {
            if (EnabledQueue?.ToLower() == "rabbitmq")
            {
                if (_rabbitMqSettings.Hostname == "localhost")
                    factory = new ConnectionFactory() { HostName = _rabbitMqSettings.Hostname };
                else
                    factory = new ConnectionFactory() { HostName = _rabbitMqSettings.Hostname, UserName = _rabbitMqSettings.UserName, Password = _rabbitMqSettings.Password };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare(queue: _rabbitMqSettings.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                //data.UserId = AbpSession.UserId.Value;
                //data.TenantId = AbpSession.TenantId;
                var json = JsonConvert.SerializeObject(data);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: _rabbitMqSettings.QueueName, basicProperties: null, body: body);
            }
            else if (EnabledQueue?.ToLower() == "redis")
            {
                List<AuctionBidderHistoryDto> lstData = new List<AuctionBidderHistoryDto>();
                data.CreationTime = DateTime.UtcNow;
                data.UniqueId = Guid.NewGuid();
                lstData = _cacheAppService.GetHistoryCache();

                //check wheather highest bid cache available or not


                var highestBidFromCache = _cacheAppService.GetHighesBidHistoryCache(data.AuctionItemId.ToString());
                if (highestBidFromCache == null)
                {
                    var highestBidFromDb = await _auctionHistoryService.GetHighestBid(data.AuctionItemId);
                    if (highestBidFromDb == null)
                    {
                        await _cacheAppService.SetHighesBidHistoryCache(new AuctionItemHighestBid
                        {
                            AuctionItemId = data.AuctionItemId,
                            HighestBidAmount = data.BidAmount,
                            MinNextBidAmount = Math.Round(Helper.Helper.GetNextBidAmount(data.BidAmount), 2),
                            IsBidClosed = false
                        });
                    }
                    else
                    {
                        if (data.BidAmount >= Math.Round(highestBidFromDb.BidAmount, 2))
                        {
                            await _cacheAppService.SetHighesBidHistoryCache(new AuctionItemHighestBid
                            {
                                AuctionItemId = data.AuctionItemId,
                                HighestBidAmount = data.BidAmount,
                                MinNextBidAmount = Math.Round(Helper.Helper.GetNextBidAmount(data.BidAmount), 2),
                                IsBidClosed = false
                            });
                        }
                        else if (data.BidAmount < Math.Round(highestBidFromDb.BidAmount, 2))
                        {
                            data.IsOutBid = true;
                            await _cacheAppService.SetHighesBidHistoryCache(new AuctionItemHighestBid
                            {
                                AuctionItemId = data.AuctionItemId,
                                HighestBidAmount = highestBidFromDb.BidAmount,
                                MinNextBidAmount = Math.Round(Helper.Helper.GetNextBidAmount(highestBidFromDb.BidAmount), 2),
                                IsBidClosed = highestBidFromDb.IsBiddingClosed
                            });
                        }
                    }
                }
                else
                {
                    if (data.BidAmount >= Math.Round(highestBidFromCache.MinNextBidAmount, 2))
                    {
                        await _cacheAppService.SetHighesBidHistoryCache(new AuctionItemHighestBid
                        {
                            AuctionItemId = data.AuctionItemId,
                            HighestBidAmount = data.BidAmount,
                            MinNextBidAmount = Math.Round(Helper.Helper.GetNextBidAmount(data.BidAmount), 2),
                            IsBidClosed = false
                        });
                    }
                    else if (data.BidAmount < Math.Round(highestBidFromCache.MinNextBidAmount, 2))
                        data.IsOutBid = true;
                }

                lstData = lstData.OrderBy(x => x.CreationTime).ToList();
                lstData.Add(data);
                await _cacheAppService.SetHistoryCache(lstData);
            }
            else
                await _auctionHistoryService.SaveAuctionBidderWithHistory(data);
        }
    }
}
