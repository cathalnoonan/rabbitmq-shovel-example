using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMqShovelExample.Producer
{
    public interface IRabbitProducer
    {
        Task ProduceAsync<T>(T item, CancellationToken cancellationToken = default)
            where T : class;
    }

    public class RabbitProducer : IRabbitProducer
    {
        private readonly ILogger<RabbitProducer> _logger;
        private readonly IBus _bus;

        public RabbitProducer(ILogger<RabbitProducer> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task ProduceAsync<T>(T item, CancellationToken cancellationToken = default)
            where T : class
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item), $"Unable to produce a message of type {typeof(T).FullName} because 'item' is null.");
            }

            try
            {
                await _bus.Publish(item, cancellationToken);
                _logger.LogInformation($"Produced message: {JsonConvert.SerializeObject(item)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error producing message of type {typeof(T).FullName}, {ex.Message}");
                throw;
            }
        }
    }
}
