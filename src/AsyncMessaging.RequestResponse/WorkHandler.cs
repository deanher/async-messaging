using System.Collections.Concurrent;

namespace AsyncMessaging.RequestResponse;

internal class WorkHandler
{
    private readonly BlockingCollection<Worker> _workers;

    public WorkHandler(int workers = 10)
    {
        _workers = new BlockingCollection<Worker>(workers);
        for (var i = 0; i < workers; i++)
        {
            _workers.Add(new Worker());
        }
    }

    public Task<TResponse> Execute<TRequest, TResponse>(IResponseHandler<TRequest, TResponse> responseHandler, TRequest request, CancellationToken cancellationToken)
    {
        var worker = _workers.Take(cancellationToken);
        try
        {
            return worker.Execute(responseHandler, request, cancellationToken);
        }
        finally
        {
            _workers.Add(worker, cancellationToken);
        }
    }
}