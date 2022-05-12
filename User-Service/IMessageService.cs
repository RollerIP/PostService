namespace Post_Service.Messaging
{
    public interface IMessageService
    {
        public void Connect();
        public void Publish<T>(string target, T data);
        public void Subscribe(string target, Action<NatsMessage> action);
    }
}
