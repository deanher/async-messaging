using AsyncMessaging.Shared;

namespace AsyncMessaging.PubSub;

public interface IPubSub
{
    Task Publish<TMessage>(TMessage message);
    Task Subscribe<TMessage>(IMessageHandler<TMessage> messageHandler);
    Task Unsubscribe<TMessage>(IMessageHandler<TMessage> messageHandler);
}