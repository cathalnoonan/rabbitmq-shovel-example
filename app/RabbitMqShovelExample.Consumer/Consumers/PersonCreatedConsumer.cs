using MassTransit;
using Newtonsoft.Json;
using RabbitMqShovelExample.Models;

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
