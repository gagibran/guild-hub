using GuildHub.Api.Common.RequestHandler;
using GuildHub.Api.Posts.CreatePost;

namespace GuildHub.Api.Posts;

public static class DependencyInjection
{
    public static IServiceCollection AddPostServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IRequestHandler<CreatePostDto, PostCreatedDto>, CreatePostHandler>();
        return serviceCollection;
    }

    public static void AddPostEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        RouteGroupBuilder postsGroupBuilder = endpointRouteBuilder.MapGroup("/api/v1/posts");
        postsGroupBuilder.MapPost("/", CreatePostEndpoint.CreatePostAsync);
    }
}
