namespace GuildHub.Api.Posts.UpdatePostById;

public sealed class UpdatePostByIdEndpoint
{
    public static async Task<Results<ProblemHttpResult, NoContent>> UpdatePostByIdAsync(
        IRequestDispatcher dispatcher,
        HttpContext httpContext,
        Guid id,
        UpdatePostByIdDto updatePostByIdDto,
        CancellationToken cancellationToken = default)
    {
        Result updatePostByIdResult = await dispatcher.DispatchRequestAsync(
            new UpdatePostByIdRequest(id, updatePostByIdDto.Title, updatePostByIdDto.Content, updatePostByIdDto.ImagePath),
            cancellationToken);
        if (updatePostByIdResult.IsSuccess)
        {
            return TypedResults.NoContent();
        }
        if (updatePostByIdResult.Errors.Contains($"No post with the ID '{id}' was found."))
        {
            return ApiHelper.CreateProblemDetails(HttpStatusCode.NotFound, updatePostByIdResult.Errors, httpContext);
        }
        return ApiHelper.CreateProblemDetails(HttpStatusCode.UnprocessableEntity, updatePostByIdResult.Errors, httpContext);
    }
}
