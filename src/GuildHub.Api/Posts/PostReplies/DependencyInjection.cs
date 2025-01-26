using GuildHub.Api.Posts.PostReplies.CreatePostReply;

namespace GuildHub.Api.Posts.PostReplies;

public static class DependencyInjection
{
    public static IServiceCollection AddPostReplyServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddRequestHandlers();
        return serviceCollection;
    }

    public static void AddPostReplyEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        RouteGroupBuilder postRepliesGroupBuilder = endpointRouteBuilder.MapGroup("/api/posts/{postId}/replies");
        postRepliesGroupBuilder.MapPost("/", CreatePostReplyEndpoint.CreatePostReplyAsync);
    }

    private static void AddRequestHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IRequestHandler<CreatePostReplyRequest>, CreatePostReplyHandler>();
    }
}
