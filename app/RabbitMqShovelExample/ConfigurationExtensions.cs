using Microsoft.Extensions.Configuration;
using RabbitMqShovelExample.Configuration;

namespace RabbitMqShovelExample
{
    public static class ConfigurationExtensions
    {
        public static IRabbitOptions GetRabbitOptions(this IConfiguration configuration)
        {
            return configuration.GetSection("Rabbit").Get<RabbitOptions>();
        }
    }
}
