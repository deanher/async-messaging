namespace AsyncMessaging.Shared
{
    public interface IMessageHandler<in TMessage>
    {
        Task Handle(TMessage message);
    }
}
