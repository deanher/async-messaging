namespace AsyncMessaging.SendReceive;

public interface IReceptionHandler<in TMessage>
{
    Task Handle(TMessage message, CancellationToken cancellationToken);
}