using Newtonsoft.Json;

namespace User_Service.Messaging
{
    public class NatsMessage
    {
        public string origin { get; set; }
        public string target { get; set; }
        public string content { get; set; }

        public static NatsMessage Create<T>(string target, T content)
        {
            NatsMessage message = new NatsMessage();
            message.origin = "User-Service";
            message.target = target;
            message.content = JsonConvert.SerializeObject(content);

            return message;
        }
    }
}
