namespace AsyncMessaging.RequestResponse;

public interface IResponseHandler<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}