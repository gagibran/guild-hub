using GuildHub.Api.Common.RequestHandler;

namespace GuildHub.Api.Common.DispatcherPattern;

public interface IDispatcher
{
    Task<TResponse> DispatchAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest
        where TResponse : IResponse;
}
