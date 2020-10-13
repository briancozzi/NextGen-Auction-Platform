using Castle.Core.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NextGen.BiddingPlatform.RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.BackGroundTask.BackGroundService
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private IModel _channel;
        private readonly RabbitMqSettings _rabbitMqSettings;

        public QueuedHostedService(IOptions<RabbitMqSettings> rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings.Value;
        }
        protected async override Task ExecuteAsync(
            CancellationToken cancellationToken)
        {
        }
    }

}
