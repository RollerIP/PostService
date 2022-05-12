using Newtonsoft.Json;

namespace Post_Service.Messaging
{
    public class NatsMessage
    {
        public string origin { get; } = "Post-Service";
        public string target { get; private set; }
        public string content { get; private set; }

        public static NatsMessage Create<T>(string target, T content)
        {
            NatsMessage message = new NatsMessage();
            message.target = target;
            message.content = JsonConvert.SerializeObject(content);

            return message;
        }
    }
}
