using EasyNetQ;

namespace AsyncMessaging.RequestResponse
{
    public class RabbitMqRequestResponse : IRequestResponse
    {
        private readonly string _connectionString;

        public RabbitMqRequestResponse(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<TResponse> Request<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken, string? queueName = null)
        {
            using var bus = RabbitHutch.CreateBus(_connectionString);

            return bus.Rpc.RequestAsync<TRequest, TResponse>(request,
                configuration =>
                {
                    if (!string.IsNullOrWhiteSpace(queueName))
                        configuration.WithQueueName(queueName);
                },
                cancellationToken);
        }

        public Task Respond<TRequest, TResponse>(IResponseHandler<TRequest, TResponse> responseHandler, string? queueName = null, bool durable = true, int workers = 10)
        {
            using var bus = RabbitHutch.CreateBus("host=localhost;username=test;password=test123!");

            var workHandler = new WorkHandler();
            bus.Rpc.RespondAsync<TRequest, TResponse>((request, cancellationToken) => workHandler.Execute(responseHandler, request, cancellationToken),
                configuration =>
                {
                    if (!string.IsNullOrWhiteSpace(queueName))
                        configuration.WithQueueName(queueName);

                    configuration.WithDurable(durable);
                });

            return Task.CompletedTask;
        }
    }
}