namespace GuildHub.IntegrationTests.Api.Posts.GetPostById;

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
    public async Task GetPostByIdAsync_WhenPostExists_ShouldReturnPost()
    {
        // Arrange:
        const string ExpectedTitle = "Title";
        const string ExpectedContent = "Content";
        const string ExpectedImagePath = "ImagePath";
        Guid postId = (await CreateAsync<CreatedPostDto>(
            $"{{\"title\": \"{ExpectedTitle}\", \"content\": \"{ExpectedContent}\", \"imagePath\": \"{ExpectedImagePath}\"}}",
            Constants.BasePostEndpoint)).Id;
        var expectedRetrievedPostByIdDto = new RetrievedPostByIdDto(
            It.IsAny<Guid>(),
            ExpectedTitle,
            ExpectedContent,
            ExpectedImagePath,
            [],
            It.IsAny<DateTime>(),
            It.IsAny<DateTime?>());
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Constants.BasePostEndpoint}/{postId}");

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        httpResponseMessage.EnsureSuccessStatusCode();
        string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        RetrievedPostByIdDto? actualRetrievedPostByIdDto = JsonSerializer.Deserialize<RetrievedPostByIdDto>(responseContent, JsonSerializerOptions);
        actualRetrievedPostByIdDto
            .Should()
            .BeEquivalentTo(
                expectedRetrievedPostByIdDto,
                options => options
                    .Excluding(retrievedPostByIdDtos => retrievedPostByIdDtos.CreatedAtUtc)
                    .Excluding(retrievedPostByIdDtos => retrievedPostByIdDtos.UpdatedAtUtc)
                    .Excluding(retrievedPostByIdDtos => retrievedPostByIdDtos.Id));
    }
}
