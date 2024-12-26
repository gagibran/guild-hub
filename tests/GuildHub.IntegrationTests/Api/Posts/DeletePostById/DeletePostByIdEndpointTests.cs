namespace GuildHub.IntegrationTests.Api.Posts.DeletePostById;

public sealed class DeletePostByIdEndpointTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
    : IntegrationTest(integrationTestsWebApplicationFactory)
{
    [Fact]
    public async Task DeletePostByIdAsync_WhenPostDoesNotExist_ShouldReturnProblemHttpResult()
    {
        // Arrange:
        Guid postId = Guid.NewGuid();
        ProblemHttpResult expectedProblemHttpResult = TypedResults.Problem(
            title: "One or more validation errors occurred.",
            statusCode: (int)HttpStatusCode.NotFound);
        var expectedErrors = new List<string> { $"No post with the ID '{postId}' was found." };
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{Constants.BasePostEndpoint}/{postId}");

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        ProblemDetails? actualValidationProblemDetails = await httpResponseMessage.Content.ReadFromJsonAsync<ProblemDetails>();
        List<string>? actualErrors = ((JsonElement)actualValidationProblemDetails!.Extensions["errors"]!).Deserialize<List<string>>();
        actualErrors.Should().BeEquivalentTo(expectedErrors);
        actualValidationProblemDetails!.Extensions["traceId"].Should().NotBeNull();
        actualValidationProblemDetails
            .Should()
            .BeEquivalentTo(
                expectedProblemHttpResult.ProblemDetails,
                options => options.Excluding(problemDetails => problemDetails.Extensions));
    }

    [Fact]
    public async Task DeletePostByIdAsync_WhenPostExists_ShouldDeletePost()
    {
        // Arrange:
        Guid postId = (await CreateAsync<CreatedPostDto>(
            $"{{\"title\": \"Title\", \"content\": \"Content\", \"imagePath\": \"ImagePath\"}}",
            Constants.BasePostEndpoint)).Id;
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{Constants.BasePostEndpoint}/{postId}");

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        httpResponseMessage.EnsureSuccessStatusCode();
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
