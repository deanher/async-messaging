using EasyNetQ;

namespace AsyncMessaging.Topic
{
    public class RabbitMqTopicPubSub : ITopicPubSub
    {
        private readonly string _connectionString;
        private readonly bool _usePublisherConfirmations;
        private readonly int _timeout;

        public RabbitMqTopicPubSub(string connectionString, bool usePublisherConfirmations = true, int timeout = 30)
        {
            _connectionString = connectionString;
            _usePublisherConfirmations = usePublisherConfirmations;
            _timeout = timeout;
        }

        public Task Publish<TMessage>(TMessage message, string topic)
        {
            using var bus = RabbitHutch.CreateBus($"{_connectionString};publisherConfirms={_usePublisherConfirmations};timeout={_timeout}");
            return bus.PubSub.PublishAsync(message, topic);
        }

        public Task Subscribe<TMessage>(ITopicMessageHandler<TMessage> messageHandler, string topic, CancellationToken cancellationToken, ushort? prefetchCount = null)
        {
            using var bus = RabbitHutch.CreateBus(_connectionString);
            return bus.PubSub.SubscribeAsync<TMessage>($"{typeof(TMessage)}", messageHandler.Handle, Configure(topic, prefetchCount), cancellationToken);
        }

        private Action<ISubscriptionConfiguration> Configure(string topic, ushort? prefetchCount) => configuration =>
                                                                                                     {
                                                                                                         if (prefetchCount != null)
                                                                                                             configuration.WithPrefetchCount(prefetchCount.Value);

                                                                                                         configuration.WithTopic(topic);
                                                                                                     };
    }
}