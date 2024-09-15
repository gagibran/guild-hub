namespace GuildHub.Common.RequestHandler;

public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest where TResponse : IResponse
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
