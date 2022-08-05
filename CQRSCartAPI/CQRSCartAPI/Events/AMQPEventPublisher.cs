
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CQRSCartAPI.Events
{
    public class AMQPEventPublisher
    {
        private readonly ConnectionFactory connectionFactory;
        private readonly IConfiguration _configuration;

        public AMQPEventPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();


            connectionFactory = new ConnectionFactory();
            connectionFactory.UserName = configuration["amqp:username"];
            connectionFactory.Password = configuration["amqp:password"];
            connectionFactory.VirtualHost = configuration["amqp:virtualhost"];
            connectionFactory.HostName = configuration["amqp:hostname"];
            connectionFactory.Port = AmqpTcpEndpoint.UseDefaultPort;
           

            //connectionFactory.CreateConnection();
        }
        public void PublishEvent<T>(T @event) where T : IEvent
        {
            using (IConnection conn = connectionFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    var queue = @event is CartCreatedEvent ?
                        Constants.QUEUE_CART_CREATED : @event is CartUpdatedEvent ?
                            Constants.QUEUE_CART_UPDATED : Constants.QUEUE_CART_DELETED;
                    channel.QueueDeclare(
                        queue: queue,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: queue,
                        basicProperties: null,
                        body: body
                    );
                    Debug.WriteLine("HI.....................");
                }
            }
        }
    }
}
