using RabbitMqShovelExample.Models;

namespace RabbitMqShovelExample.Producer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRabbitProducer _producer;

        public Worker(ILogger<Worker> logger, IRabbitProducer producer)
        {
            _logger = logger;
            _producer = producer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = new PersonCreatedEvent()
                {
                    FirstName = Faker.Name.FirstName(),
                    LastName = Faker.Name.LastName(),
                };
                await _producer.ProduceAsync(message);
                await Task.Delay(1000);
            }
        }

    }
}
