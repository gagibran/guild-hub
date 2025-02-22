using GuildHub.Api.Posts.CreatePost;
using GuildHub.Api.Posts.DeletePostById;
using GuildHub.Api.Posts.GetPosts;
using GuildHub.Api.Posts.UpdatePostById;

namespace GuildHub.Api.Posts;

public static class DependencyInjection
{
    public static IServiceCollection AddPostServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddRequestHandlers();
        serviceCollection.AddMapHandlers();
        serviceCollection.AddPostReplyServices();
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
        endpointRouteBuilder.AddPostReplyEndpoints();
    }

    private static void AddRequestHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IRequestHandler<CreatePostRequest, CreatedPostDto>, CreatePostHandler>();
        serviceCollection.AddScoped<IRequestHandler<GetPostByIdRequest, RetrievedPostByIdDto>, GetPostByIdHandler>();
        serviceCollection.AddScoped<IRequestHandler<GetPostsRequest, RetrievedPostsDto>, GetPostsHandler>();
        serviceCollection.AddScoped<IRequestHandler<DeletePostByIdRequest>, DeletePostByIdHandler>();
        serviceCollection.AddScoped<IRequestHandler<UpdatePostByIdRequest>, UpdatePostByIdHandler>();
    }

    private static void AddMapHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IMapHandler<Post, RetrievedPostByIdDto>, PostToRetrievedPostByIdDtoMapper>();
        serviceCollection.AddTransient<IMapHandler<Post, CreatedPostDto>, PostToCreatedPostDtoMapper>();
        serviceCollection.AddTransient<IMapHandler<PagedList<Post>, RetrievedPostsDto>, PagedPostsToRetrievedPostsDtoMapper>();
    }
}
