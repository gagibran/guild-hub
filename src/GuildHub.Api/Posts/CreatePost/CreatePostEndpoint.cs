namespace GuildHub.Api.Posts.CreatePost;

public static class CreatePostEndpoint
{
    public static async Task<Results<ProblemHttpResult, CreatedAtRoute<CreatedPostDto>>> CreatePostAsync(
        IRequestDispatcher dispatcher,
        HttpContext httpContext,
        CreatePostDto createPostDto,
        CancellationToken cancellationToken = default)
    {
        Result<CreatedPostDto> createdPostDtoResult = await dispatcher.DispatchRequestAsync<CreatePostDto, CreatedPostDto>(createPostDto, cancellationToken);
        if (!createdPostDtoResult.IsSuccess)
        {
            return ApiHelper.CreateProblemDetails(HttpStatusCode.UnprocessableEntity, createdPostDtoResult.Errors, httpContext);
        }
        return TypedResults.CreatedAtRoute(createdPostDtoResult.Value, nameof(GetPostByIdEndpoint.GetPostByIdAsync), new { createdPostDtoResult.Value!.Id });
    }
}
