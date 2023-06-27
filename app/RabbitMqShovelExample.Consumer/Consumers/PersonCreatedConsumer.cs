using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMqShovelExample.Models;
using System;
using System.Threading.Tasks;

namespace RabbitMqShovelExample.Consumer.Consumers
{
    public class PersonCreatedConsumer : IConsumer<PersonCreatedEvent>
    {
        private readonly ILogger<PersonCreatedConsumer> _logger;
        private readonly bool _badConsumer;

        public PersonCreatedConsumer(ILogger<PersonCreatedConsumer> logger, IConfiguration configuration)
        {
            _logger = logger;
            _badConsumer = configuration.GetValue<bool>("AppSettings:BadConsumer");
        }

        public Task Consume(ConsumeContext<PersonCreatedEvent> context)
        {
            if (_badConsumer)
            {
                throw new Exception($"Intentional exception consuming message {JsonConvert.SerializeObject(context.Message)}");
            }

            _logger.LogInformation($"Consumed message: {JsonConvert.SerializeObject(context.Message)}");
            return Task.CompletedTask;
        }
    }
}
