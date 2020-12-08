using Abp.Authorization;
using Abp.Runtime.Caching;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NextGen.BiddingPlatform.AuctionHistory;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using NextGen.BiddingPlatform.Configuration;
using NextGen.BiddingPlatform.RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.BackgroundService.RabbitMqService
{
    //[AbpAuthorize]
    public class RabbitMqService : BiddingPlatformAppServiceBase, IRabbitMqService
    {
        private readonly RabbitMqSettings _rabbitMqSettings;
        private ConnectionFactory factory;
        private readonly bool IsRabbitMqEnabled;
        private readonly bool IsRedisCacheEnabled;
        private readonly IAuctionHistoryAppService _auctionHistoryService;
        private readonly ICacheManager _cacheManager;
        private readonly IConfigurationRoot _appConfiguration;
        public RabbitMqService(IOptions<RabbitMqSettings> rabbitMqSettings,
                                                    IAuctionHistoryAppService auctionHistoryAppService,
                                                    ICacheManager cacheManager,
                                                    IWebHostEnvironment env)
        {
            _rabbitMqSettings = rabbitMqSettings.Value;
            _auctionHistoryService = auctionHistoryAppService;
            _cacheManager = cacheManager;
            _appConfiguration = env.GetAppConfiguration();
            IsRabbitMqEnabled = bool.Parse(_appConfiguration["RabbitMQ:IsEnabled"]);
            IsRedisCacheEnabled = bool.Parse(_appConfiguration["Abp:RedisCache:IsEnabled"]);
        }
        public async Task AddToQueue(AuctionBidderHistoryDto data)
        {
            if (IsRabbitMqEnabled)
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
            else if (IsRedisCacheEnabled)
            {
                await _cacheManager.GetCache("AuctionHistoryCache").SetAsync("AuctionHistoryTest", data);
            }
            else
            {
                await _auctionHistoryService.SaveAuctionBidderWithHistory(data);
            }
        }
    }
}
