using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.RabbitMQ
{
    public class RabbitMqSettings
    {
        public string Hostname { get; set; }
        public string QueueName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
