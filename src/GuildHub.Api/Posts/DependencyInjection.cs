using GuildHub.Api.Posts.CreatePost;
using GuildHub.Api.Posts.DeletePostById;
using GuildHub.Api.Posts.GetPosts;
using GuildHub.Api.Posts.PostReplies;
using GuildHub.Api.Posts.UpdatePostById;

namespace GuildHub.Api.Posts;

public static class DependencyInjection
{
    public static IServiceCollection AddPostServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddRequestHandlers();
        serviceCollection.AddMapHandlers();
        return serviceCollection;
    }

    public static void AddPostEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        RouteGroupBuilder postsGroupBuilder = endpointRouteBuilder.MapGroup("/api/posts");
        postsGroupBuilder.MapPost("/", CreatePostEndpoint.CreatePostAsync);
        postsGroupBuilder.MapGet("/{id}", GetPostByIdEndpoint.GetPostByIdAsync).WithName(nameof(GetPostByIdEndpoint.GetPostByIdAsync));
        postsGroupBuilder.MapGet("/", GetPostsEndpoint.GetPostsAsync);
        postsGroupBuilder.MapDelete("/{id}", DeletePostByIdEndpoint.DeletePostByIdAsync);
        postsGroupBuilder.MapPut("/{id}", UpdatePostByIdEndpoint.UpdatePostByIdAsync);
    }

    private static void AddRequestHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IRequestHandler<CreatePostDto, CreatedPostDto>, CreatePostHandler>();
        serviceCollection.AddTransient<IRequestHandler<GetPostByIdDto, RetrievedPostByIdDto>, GetPostByIdHandler>();
        serviceCollection.AddTransient<IRequestHandler<GetPostsDto, RetrievedPostsDto>, GetPostsHandler>();
        serviceCollection.AddTransient<IRequestHandler<DeletePostByIdDto>, DeletePostByIdHandler>();
        serviceCollection.AddTransient<IRequestHandler<UpdatePostByIdRequest>, UpdatePostByIdHandler>();
    }

    private static void AddMapHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IMapHandler<ICollection<PostReply>, List<RetrievedPostReplyForPostDto>>, PostRepliesToRetrievedPostRepliesForPostDtoMapper>();
        serviceCollection.AddTransient<IMapHandler<Post, RetrievedPostByIdDto>, PostToRetrievedPostByIdDtoMapper>();
        serviceCollection.AddTransient<IMapHandler<Post, CreatedPostDto>, PostToCreatedPostDtoMapper>();
        serviceCollection.AddTransient<IMapHandler<PagedList<Post>, RetrievedPostsDto>, PagedPostsToRetrievedPostsDtoMapper>();
    }
}
