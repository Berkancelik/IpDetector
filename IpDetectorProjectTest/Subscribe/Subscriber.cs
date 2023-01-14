using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StackExchange.Redis;
using System;
using System.Text;

namespace IpDetectorProjectTest
{
    public class Subscriber
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;
        private readonly ConnectionMultiplexer _redis;
        public Subscriber(string hostname, string queueName, string redisConnectionString)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = queueName;
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _redis = ConnectionMultiplexer.Connect(redisConnectionString);
        }
        public void Start()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine(" [x] Received {0}", message);
                // Redis'e bağlan
                var redis = ConnectionMultiplexer.Connect("redis:6379");
                var db = redis.GetDatabase();

                // Mesajı Redis'e ekle
                db.StringSet("message", message);
            };
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }
    }
}
