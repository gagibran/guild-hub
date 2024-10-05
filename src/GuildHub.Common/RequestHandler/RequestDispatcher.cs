namespace GuildHub.Common.RequestHandler;

public sealed class RequestDispatcher(IServiceProvider serviceProvider) : IRequestDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<Result<TResponse>> DispatchRequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest
        where TResponse : IResponse
    {
        var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        return await handler.HandleAsync(request, cancellationToken);
    }

    public async Task<Result<TResponse>> DispatchRequestAsync<TResponse>(CancellationToken cancellationToken)
        where TResponse : IResponse
    {
        var handler = _serviceProvider.GetRequiredService<IRequestHandler<TResponse>>();
        return await handler.HandleAsync(cancellationToken);
    }
}
