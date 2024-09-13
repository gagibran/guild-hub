namespace GuildHub.Api.Common.RequestHandler;

public interface IRequestHandler<TCommand, TResponse> where TCommand : IRequest where TResponse : IResponse
{
    Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken);
}
