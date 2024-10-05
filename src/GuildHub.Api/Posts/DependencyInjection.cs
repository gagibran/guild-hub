using GuildHub.Api.Posts.CreatePost;
using GuildHub.Api.Posts.GetPosts;

namespace GuildHub.Api.Posts;

public static class DependencyInjection
{
    public static IServiceCollection AddPostServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IRequestHandler<CreatePostDto, CreatedPostDto>, CreatePostHandler>();
        serviceCollection.AddTransient<IRequestHandler<GetPostByIdDto, RetrievedPostByIdDto>, GetPostByIdHandler>();
        serviceCollection.AddTransient<IRequestHandler<GetPostsDto, RetrievedPostsDto>, GetPostsHandler>();
        serviceCollection.AddTransient<IMapHandler<ICollection<PostReply>, List<RetrievedPostReplyForPostDto>>, PostRepliesToRetrievedPostRepliesForPostDtoMapper>();
        serviceCollection.AddTransient<IMapHandler<Post, RetrievedPostByIdDto>, PostToRetrievedPostByIdDtoMapper>();
        serviceCollection.AddTransient<IMapHandler<Post, CreatedPostDto>, PostToCreatedPostDtoMapper>();
        serviceCollection.AddTransient<IMapHandler<PagedList<Post>, RetrievedPostsDto>, PagedPostsToRetrievedPostsDtoMapper>();
        return serviceCollection;
    }

    public static void AddPostEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        RouteGroupBuilder postsGroupBuilder = endpointRouteBuilder.MapGroup("/api/posts");
        postsGroupBuilder.MapPost("/", CreatePostEndpoint.CreatePostAsync);
        postsGroupBuilder.MapGet("/{id}", GetPostByIdEndpoint.GetPostByIdAsync).WithName(nameof(GetPostByIdEndpoint.GetPostByIdAsync));
        postsGroupBuilder.MapGet("/", GetPostsEndpoint.GetPostsAsync);
    }
}
