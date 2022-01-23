using EasyNetQ;

namespace AsyncMessaging.SendReceive;

public class SendReceive : ISendReceive
{
    private readonly string _connectionString;

    public SendReceive(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Task Send<TMessage>(string queueName, TMessage message, CancellationToken cancellationToken, ushort? priority = null)
    {
        using var bus = RabbitHutch.CreateBus(_connectionString);
        return bus.SendReceive.SendAsync(queueName, message, Action(priority), cancellationToken);
    }

    private Action<ISendConfiguration> Action(ushort? priority)
    {
        return configuration =>
               {
                   if (priority != null)
                       configuration.WithPriority(Convert.ToByte(priority));

               };
    }

    public Task Receive<TMessage>(string queueName, IReceptionHandler<TMessage> receptionHandler, CancellationToken cancellationToken, bool isExclusive = true, ushort? prefetchCount = null, IDictionary<string, object>? arguments = null, ushort? priority = null)
    {
        using var bus = RabbitHutch.CreateBus(_connectionString);
        bus.SendReceive.ReceiveAsync(queueName, registration => registration.Add<TMessage>(receptionHandler.Handle), Configure(isExclusive, prefetchCount, arguments, priority), cancellationToken);
        return Task.CompletedTask;
    }

    private Action<IConsumerConfiguration> Configure(bool isExclusive, ushort? prefetchCount, IDictionary<string, object>? arguments, ushort? priority)
    {
        return configuration =>
               {
                   configuration.WithExclusive(isExclusive);

                   foreach (var (key, value) in arguments ?? new Dictionary<string, object>())
                   {
                       configuration.WithArgument(key, value);
                   }

                   if (prefetchCount != null)
                       configuration.WithPrefetchCount(prefetchCount.Value);
                   if (priority != null)
                       configuration.WithPriority(priority.Value);
               };
    }
}