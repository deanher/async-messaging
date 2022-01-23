namespace AsyncMessaging.RequestResponse;

public interface IRequestResponse
{
    Task<TResponse> Request<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken, string? queueName = null);
    Task Respond<TRequest, TResponse>(IResponseHandler<TRequest, TResponse> responseHandler, string? queueName = null, bool durable = true, int workers = 10);
}