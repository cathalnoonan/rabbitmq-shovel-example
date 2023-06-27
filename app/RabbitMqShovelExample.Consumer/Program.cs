using MassTransit;
using Microsoft.Extensions.Hosting;
using RabbitMqShovelExample;
using RabbitMqShovelExample.Consumer.Consumers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddMassTransit(busRegConfig =>
        {
            var rabbitOptions = context.Configuration.GetRabbitOptions();
            busRegConfig.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(rabbitOptions.Host, hostOptions =>
                {
                    hostOptions.Username(rabbitOptions.Username);
                    hostOptions.Password(rabbitOptions.Password);
                });
                cfg.ReceiveEndpoint("person_created", e =>
                {
                    e.ConfigureConsumer<PersonCreatedConsumer>(ctx);
                });
            });
            busRegConfig.AddConsumer<PersonCreatedConsumer>();
        });
    })
    .Build();

await host.RunAsync();
