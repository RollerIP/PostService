using NATS.Client;
using Newtonsoft.Json;
using Post_Service.Models;
using System.Diagnostics;
using System.Text;

namespace Post_Service.Messaging
{
    public class NatsService : IMessageService
    {
        private IConfiguration configuration;
        private IConnection connection = null;
        private IAsyncSubscription subscription = null;
        private readonly string connectionString = "nats://host.docker.internal:4444";

        public NatsService(IConfiguration configuration)
        {
            this.configuration = configuration;
            Connect();
        }

        public void Connect()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            Options options = ConnectionFactory.GetDefaultOptions();
            options.Url = connectionString;

            Console.WriteLine("Trying to connect to the NATS Server");

            try
            {
                connection = connectionFactory.CreateConnection(options);
                Console.WriteLine("Succesfully connected to the NATS server");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to the NATS server");
                Console.WriteLine(ex.Message);
            }
        }

        public void Publish<T>(string target, T data)
        {
            if (connection == null)
            {
                Console.WriteLine("Unable to publish: no connection");
                return;
            }

            string message = JsonConvert.SerializeObject(NatsMessage.Create(target, data));
            connection.Publish(target, Encoding.UTF8.GetBytes(message));

            Console.WriteLine("Message sent: " + message);
        }

        public void Subscribe(string target, Action<NatsMessage> handler)
        {
            if (connection == null)
            {
                Console.WriteLine("Unable to subscribe: no connection");
                return;
            }

            try
            {
                subscription = connection.SubscribeAsync(target);
                if (subscription == null)
                {
                    Console.WriteLine("Unable to subscribe to" + target);
                    return;
                }

                subscription.MessageHandler += (_, args) =>
                {
                    string jsonString = Encoding.UTF8.GetString(args.Message.Data);
                    NatsMessage message = JsonConvert.DeserializeObject<NatsMessage>(jsonString);

                    if (message == null) return;

                    handler(message);
                };

                subscription.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to subscribe to " + target + ": " + ex.Message);
            }
        }
    }
}
