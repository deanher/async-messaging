namespace AsyncMessaging.Topic;

public interface ITopicPubSub
{
    Task Publish<TMessage>(TMessage message, string topic);
    Task Subscribe<TMessage>(ITopicMessageHandler<TMessage> messageHandler, string topic, CancellationToken cancellationToken, ushort? prefetchCount);
}