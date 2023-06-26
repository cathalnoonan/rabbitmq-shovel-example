using MassTransit;
using Newtonsoft.Json;
using RabbitMqShovelExample.Models;

namespace RabbitMqShovelExample.Consumer.Consumers
{
    public class PersonCreatedConsumer : IConsumer<PersonCreatedEvent>
    {
        private readonly ILogger<PersonCreatedConsumer> _logger;

        public PersonCreatedConsumer(ILogger<PersonCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<PersonCreatedEvent> context)
        {
            // Comment this line out to consume the messages successfully
            throw new NotImplementedException();

            _logger.LogInformation($"Consumed message: {JsonConvert.SerializeObject(context.Message)}");
            return Task.CompletedTask;
        }
    }
}
