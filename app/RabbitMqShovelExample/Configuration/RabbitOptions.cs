namespace RabbitMqShovelExample.Configuration
{
    public interface IRabbitOptions
    {
        string Host { get; }
        string Username { get; }
        string Password { get; }
    }

    internal class RabbitOptions : IRabbitOptions
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
