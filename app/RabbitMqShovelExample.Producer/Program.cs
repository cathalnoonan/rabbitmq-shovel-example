using MassTransit;
using RabbitMqShovelExample;
using RabbitMqShovelExample.Producer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;
        services.AddRabbitOptions(configuration);
        services.AddMassTransit(busConfig =>
        {
            busConfig.UsingRabbitMq((ctx, busFactoryConfigurator) =>
            {
                var rabbitOptions = configuration.GetRabbitOptions();
                busFactoryConfigurator.Host(rabbitOptions.Host, hostConfigurator =>
                {
                    hostConfigurator.Username(rabbitOptions.Username);
                    hostConfigurator.Password(rabbitOptions.Password);
                });
            });
        });

        services.AddSingleton<IRabbitProducer, RabbitProducer>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
