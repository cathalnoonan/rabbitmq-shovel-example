using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RabbitMqShovelExample.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMqShovelExample.Producer.Tests
{
    public class RabbitProducerTests
    {
        private Mock<ILogger<RabbitProducer>> _logger;
        private Mock<IBus> _goodBus;
        private Mock<IBus> _badBus;
        private RabbitProducer _producer;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _logger = new Mock<ILogger<RabbitProducer>>();
            _goodBus = new Mock<IBus>();
            _badBus = new Mock<IBus>();

            _badBus.Setup(x => x.Publish(It.IsAny<PersonCreatedEvent>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Bad consumer exception."));
        }

        [SetUp]
        public void SetUp()
        {
            _producer = new RabbitProducer(_logger.Object, _goodBus.Object);
        }

        [Test]
        public void produce_null_throws_error()
        {
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _producer.ProduceAsync<PersonCreatedEvent>(null);
            });
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Does.StartWith("Unable to produce a message of type"));
            Assert.That(exception.Message, Does.EndWith("because 'item' is null. (Parameter 'item')"));
        }

        [Test]
        public void bus_throws_error()
        {
            // Use the bad producer for this test only
            _producer = new RabbitProducer(_logger.Object, _badBus.Object);

            PersonCreatedEvent @event = new() { FirstName = "John", LastName = "Smith" };
            var exception = Assert.ThrowsAsync<Exception>(async () =>
            {
                await _producer.ProduceAsync(@event, default);
            });
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Does.Contain("Bad consumer exception"));
        }

        [Test]
        public async Task consumes_successfully()
        {
            PersonCreatedEvent @event = new() { FirstName = "John", LastName = "Smith" };
            await _producer.ProduceAsync(@event);
        }
    }
}
