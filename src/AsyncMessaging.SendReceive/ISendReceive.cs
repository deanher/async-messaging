namespace AsyncMessaging.SendReceive
{
    public interface ISendReceive
    {
        Task Send<TMessage>(string queueName, TMessage message, CancellationToken cancellationToken, ushort? priority = null);
        Task Receive<TMessage>(string queueName, IReceptionHandler<TMessage> receptionHandler, CancellationToken cancellationToken, bool isExclusive = true, ushort? prefetchCount = null, IDictionary<string, object>? arguments = null, ushort? priority = null);
    }
}