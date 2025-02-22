using GuildHub.Api.Posts.PostReplies.CreatePostReply;

namespace GuildHub.Api.Posts.PostReplies;

public static class DependencyInjection
{
    public static IServiceCollection AddPostReplyServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddRequestHandlers();
        serviceCollection.AddMapHandlers();
        return serviceCollection;
    }

    public static void AddPostReplyEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        RouteGroupBuilder postRepliesGroupBuilder = endpointRouteBuilder.MapGroup("/api/posts/{postId}/replies");
        postRepliesGroupBuilder
            .MapGet("/", GetPostRepliesEndpoint.GetPostRepliesAsync)
            .WithName(nameof(GetPostRepliesEndpoint.GetPostRepliesAsync));
        postRepliesGroupBuilder.MapPost("/", CreatePostReplyEndpoint.CreatePostReplyAsync);
    }

    private static void AddRequestHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IRequestHandler<GetPostRepliesRequest, RetrievedPostRepliesDto>, GetPostRepliesHandler>();
        serviceCollection.AddScoped<IRequestHandler<CreatePostReplyRequest, CreatedPostReplyDto>, CreatePostReplyHandler>();
    }

    private static void AddMapHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IMapHandler<PagedList<PostReply>, RetrievedPostRepliesDto>, PagedPostRepliesToRetrievedPostRepliesDtoMapper>();
        serviceCollection.AddTransient<IMapHandler<PostReply, CreatedPostReplyDto>, PostReplyToCreatedPostReplyDtoMapper>();
    }
}
