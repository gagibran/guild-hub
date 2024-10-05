namespace GuildHub.Common.RequestHandler;

public interface IRequestDispatcher
{
    Task<Result<TResponse>> DispatchRequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest
        where TResponse : IResponse;
    Task<Result<TResponse>> DispatchRequestAsync<TResponse>(CancellationToken cancellationToken)
        where TResponse : IResponse;
}
