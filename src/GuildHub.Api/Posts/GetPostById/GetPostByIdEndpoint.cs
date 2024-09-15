namespace GuildHub.Api.Posts.GetPostById;

public sealed class GetPostByIdEndpoint
{
    public static async Task<Results<ProblemHttpResult, Ok<RetrievedPostByIdDto>>> GetPostByIdAsync(
        IRequestDispatcher dispatcher,
        HttpContext httpContext,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        Result<RetrievedPostByIdDto> retrievedPostDtoResult = await dispatcher.DispatchRequestAsync<GetPostByIdDto, RetrievedPostByIdDto>(new GetPostByIdDto(id), cancellationToken);
        if (!retrievedPostDtoResult.IsSuccess)
        {
            return ApiHelper.CreateProblemDetails(HttpStatusCode.NotFound, retrievedPostDtoResult.Errors, httpContext);
        }
        return TypedResults.Ok(retrievedPostDtoResult.Value);
    }
}
