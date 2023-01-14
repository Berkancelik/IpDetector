namespace IpDetectorProjectTest.Publisher
{
    using RabbitMQ.Client;
    using System;

    namespace IpDetectorProjectTest
    {
        public class RabbitMQPublisher
        {
            private readonly IConnection _connection;
            private readonly IModel _channel;

            public RabbitMQPublisher()
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: "ip_location", durable: false, exclusive: false, autoDelete: false, arguments: null);
            }

            public void SendMessage(string message)
            {
                var body = System.Text.Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish(exchange: "", routingKey: "ip_location", basicProperties: null, body: body);
            }

            public void Dispose()
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}