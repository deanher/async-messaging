using AsyncMessaging.Shared;
using EasyNetQ;

namespace AsyncMessaging.PubSub.Implementations.RabbitMq;

public class RabbitMqPubSub : IPubSub
{
    private readonly bool _usePublisherConfirmations;
    private readonly int _timeout;
    private readonly string _connectionString;

    public RabbitMqPubSub(string connectionString, bool usePublisherConfirmations = true, int timeout = 30)
    {
        _usePublisherConfirmations = usePublisherConfirmations;
        _timeout = timeout;
        _connectionString = connectionString;
    }

    public Task Publish<TMessage>(TMessage message)
    {
        using var bus = RabbitHutch.CreateBus($"{_connectionString};publisherConfirms={_usePublisherConfirmations};timeout={_timeout}");
        return bus.PubSub.PublishAsync(message);
    }

    public Task Subscribe<TMessage>(IMessageHandler<TMessage> messageHandler)
    {
        using var bus = RabbitHutch.CreateBus(_connectionString);
        return bus.PubSub.SubscribeAsync<TMessage>($"{typeof(TMessage)}", messageHandler.Handle);
    }

    public Task Unsubscribe<TMessage>(IMessageHandler<TMessage> messageHandler)
    {
        return Task.CompletedTask;
    }
}