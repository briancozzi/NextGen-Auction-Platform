using Abp.Authorization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using NextGen.BiddingPlatform.RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.BackgroundService.RabbitMqService
{
    [AbpAuthorize]
    public class RabbitMqService : BiddingPlatformAppServiceBase, IRabbitMqService
    {
        private readonly RabbitMqSettings _rabbitMqSettings;
        private ConnectionFactory factory;
        public RabbitMqService(IOptions<RabbitMqSettings> rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings.Value;
        }
        public void AddToQueue(AuctionBidderHistoryDto data)
        {
            if (_rabbitMqSettings.Hostname == "localhost")
                factory = new ConnectionFactory() { HostName = _rabbitMqSettings.Hostname };
            else
                factory = new ConnectionFactory() { HostName = _rabbitMqSettings.Hostname, UserName = _rabbitMqSettings.UserName, Password = _rabbitMqSettings.Password };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: _rabbitMqSettings.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            data.UserId = AbpSession.UserId.Value;
            data.TenantId = AbpSession.TenantId;
            var json = JsonConvert.SerializeObject(data);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: _rabbitMqSettings.QueueName, basicProperties: null, body: body);
        }
    }
}
