namespace GuildHub.IntegrationTests.Api.Posts.GetPostById;

[Collection(nameof(SharedDatabaseFixture))]
public sealed class GetPostByIdEndpointTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
    : IntegrationTest(integrationTestsWebApplicationFactory)
{
    [Fact]
    public async Task GetPostByIdAsync_WhenPostDoesNotExist_ShouldReturnProblemHttpResult()
    {
        // Arrange:
        Guid postId = Guid.NewGuid();
        ProblemHttpResult expectedProblemHttpResult = TypedResults.Problem(
            title: "One or more validation errors occurred.",
            statusCode: (int)HttpStatusCode.NotFound);
        var expectedErrors = new List<string> { $"No post with the ID '{postId}' was found." };
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Constants.BasePostEndpoint}/{postId}");

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        await AssertProblemDetailsAsync(httpResponseMessage, expectedErrors, expectedProblemHttpResult);
    }

    [Fact]
    public async Task GetPostByIdAsync_WhenPostExists_ShouldReturnPost()
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;
        var expectedRetrievedPostByIdDto = new RetrievedPostByIdDto(
            post.Id,
            post.Title.ToString(),
            post.Content!.ToString(),
            post.ImagePath,
            $"http://localhost/api/posts/{post.Id}/replies",
            post.CreatedAtUtc,
            post.UpdatedAtUtc);
        await ApplicationDbContext.Posts.AddAsync(post);
        await ApplicationDbContext.SaveChangesAsync();
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Constants.BasePostEndpoint}/{post.Id}");

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        await AssertModelWithoutDateAuditsAsync(httpResponseMessage, expectedRetrievedPostByIdDto);
    }
}
