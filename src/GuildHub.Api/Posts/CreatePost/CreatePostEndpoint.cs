using GuildHub.Api.Common.DispatcherPattern;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GuildHub.Api.Posts.CreatePost;

public sealed class CreatePostEndpoint
{
    public static async Task<Ok<PostCreatedDto>> CreatePostAsync(IDispatcher dispatcher, CreatePostDto createPostDto, CancellationToken cancellationToken = default)
    {
        PostCreatedDto postCreatedDto = await dispatcher.DispatchAsync<CreatePostDto, PostCreatedDto>(createPostDto, cancellationToken);
        return TypedResults.Ok(postCreatedDto);
    }
}
