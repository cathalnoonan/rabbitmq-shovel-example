using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMqShovelExample.Configuration;

namespace RabbitMqShovelExample
{
    public static class ConfigurationExtensions
    {
        public static IRabbitOptions GetRabbitOptions(this IConfiguration configuration)
        {
            return configuration.GetSection("Rabbit").Get<RabbitOptions>();
        }

        public static IServiceCollection AddRabbitOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRabbitOptions, RabbitOptions>();
            return services;
        }
    }
}
