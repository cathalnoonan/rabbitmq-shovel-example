using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RabbitMqShovelExample.Consumer.Consumers;
using RabbitMqShovelExample.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RabbitMqShovelExample.Consumer.Tests.Consumers
{
    public class PersonCreatedConsumerTests
    {
        private PersonCreatedEvent _event;
        private IConfiguration _badConfiguration;
        private IConfiguration _goodConfiguration;
        private Mock<ILogger<PersonCreatedConsumer>> _logger;
        private Mock<ConsumeContext<PersonCreatedEvent>> _context;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _event = new PersonCreatedEvent() { FirstName = "John", LastName = "Smith" };
            _badConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>() { ["AppSettings:BadConsumer"] = "true" })
                .Build();
            _goodConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>() { ["AppSettings:BadConsumer"] = "false" })
                .Build();

            _logger = new Mock<ILogger<PersonCreatedConsumer>>();

            _context = new Mock<ConsumeContext<PersonCreatedEvent>>();
            _context.SetupGet(x => x.Message).Returns(_event);
        }

        [Test]
        public void check_is_bad_consumer()
        {
            Assert.That(_badConfiguration.GetValue<bool>("AppSettings:BadConsumer"), Is.EqualTo(true));
        }

        [Test]
        public void check_is_good_consumer()
        {
            Assert.That(_goodConfiguration.GetValue<bool>("AppSettings:BadConsumer"), Is.EqualTo(false));
        }

        [Test]
        public void consume_message_with_error()
        {
            var consumer = new PersonCreatedConsumer(_logger.Object, _badConfiguration);
            var exception = Assert.ThrowsAsync<Exception>(async () =>
            {
                await consumer.Consume(_context.Object);
            });
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Does.StartWith("Intentional exception consuming message"));
        }

        [Test]
        public async Task consume_message_successfully()
        {
            var consumer = new PersonCreatedConsumer(_logger.Object, _goodConfiguration);
            await consumer.Consume(_context.Object);
        }
    }
}
