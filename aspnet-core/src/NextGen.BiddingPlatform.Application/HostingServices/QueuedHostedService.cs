using Castle.Core.Logging;
using Microsoft.Extensions.Options;
using NextGen.BiddingPlatform.AuctionHistory;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using NextGen.BiddingPlatform.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.HostingServices
{
    public class QueuedHostedService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly ILogger _logger;
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private readonly RabbitMqSettings _rabbitMqSettings;
        private readonly IAuctionHistoryAppService _auctionHistoryService;
        public QueuedHostedService(IOptions<RabbitMqSettings> rabbitMqSettings,
                                                             ILogger logger,
                                                             IAuctionHistoryAppService auctionHistoryService)
        {
            _rabbitMqSettings = rabbitMqSettings.Value;
            _logger = logger;
            _auctionHistoryService = auctionHistoryService;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            if (_rabbitMqSettings.Hostname == "localhost")
            {
                _connectionFactory = new ConnectionFactory { HostName = _rabbitMqSettings.Hostname, DispatchConsumersAsync = true };
            }
            else
            {
                _connectionFactory = new ConnectionFactory { HostName = _rabbitMqSettings.Hostname, UserName = _rabbitMqSettings.UserName, Password = _rabbitMqSettings.Password, DispatchConsumersAsync = true };
            }

            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _rabbitMqSettings.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.BasicQos(0, 1, false);

            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //here we will do our work
            stoppingToken.ThrowIfCancellationRequested();


            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (bc, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                try
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        var dataFromQueue = JsonSerializer.Deserialize<AuctionBidderHistoryDto>(message);
                        await _auctionHistoryService.SaveAuctionBidderWithHistory(dataFromQueue);
                    }
                    //await Task.Delay(new Random().Next(1, 3) * 1000, stoppingToken); // simulate an async email process

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (JsonException ex)
                {
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
                catch (AlreadyClosedException ex)
                {
                    _logger.Error("RabbitMQ is closed!");
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message, e);
                }
            };

            _channel.BasicConsume(queue: _rabbitMqSettings.QueueName, autoAck: false, consumer: consumer);

            await Task.CompletedTask;
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _connection.Close();
        }
    }
}
