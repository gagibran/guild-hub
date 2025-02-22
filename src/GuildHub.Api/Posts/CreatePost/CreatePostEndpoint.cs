namespace GuildHub.Api.Posts.CreatePost;

public static class CreatePostEndpoint
{
    public static async Task<Results<ProblemHttpResult, CreatedAtRoute<CreatedPostDto>>> CreatePostAsync(
        IRequestDispatcher dispatcher,
        HttpContext httpContext,
        CreatePostRequest createPostRequest,
        CancellationToken cancellationToken = default)
    {
        Result<CreatedPostDto> createdPostDtoResult = await dispatcher.DispatchRequestAsync<CreatePostRequest, CreatedPostDto>(
            createPostRequest,
            cancellationToken);
        if (!createdPostDtoResult.IsSuccess)
        {
            return ApiHelper.CreateProblemDetails(HttpStatusCode.UnprocessableEntity, createdPostDtoResult.Errors, httpContext);
        }
        return TypedResults.CreatedAtRoute(createdPostDtoResult.Value, nameof(GetPostByIdEndpoint.GetPostByIdAsync), new { createdPostDtoResult.Value!.Id });
    }
}
