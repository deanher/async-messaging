namespace AsyncMessaging.Topic;

public interface ITopicMessageHandler<in TMessage>
{
    Task Handle(TMessage message, CancellationToken cancellationToken);
}