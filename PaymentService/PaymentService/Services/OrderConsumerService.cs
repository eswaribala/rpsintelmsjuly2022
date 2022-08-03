using Confluent.Kafka;
using Newtonsoft.Json;
using PaymentService.Repositories;
using PaymentService.Models;
namespace PaymentService.Services
{
    public class OrderConsumerService : BackgroundService
    {
        private readonly IOrderRepository _OrderRepository;
        private readonly IConfiguration _configuration;

        public OrderConsumerService(IOrderRepository OrderRepository, IConfiguration configuration)
        {
            _OrderRepository =OrderRepository;
            _configuration = configuration;

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var groupId = this._configuration["GroupId"];
            var topicName = this._configuration["TopicName"];
            var boostrapServer = this._configuration["BootStrapServer"];
            return GetOrderData(groupId, topicName, boostrapServer);
        }



        private async Task<String> GetOrderData(string groupId, string topicName, string bootstrapServer)
        {
            string response = null;

            var conf = new ConsumerConfig
            {


                GroupId = groupId,
                BootstrapServers = bootstrapServer,
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(topicName);

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                            response = $"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.";
                            var result = JsonConvert.DeserializeObject<BsonOrder>(cr.Value);
                            _OrderRepository.AddOrder(result);
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                            response = $"Error occured: {e.Error.Reason}";
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
                return response;
            }
        }
    }

}
