namespace AsyncMessaging.RequestResponse;

internal class Worker
{
    public Task<TResponse> Execute<TResponse, TRequest>(IResponseHandler<TRequest, TResponse> responseHandler, TRequest request, CancellationToken cancellationToken)
    {
        return responseHandler.Handle(request, cancellationToken);
    }
}