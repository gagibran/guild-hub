namespace GuildHub.IntegrationTests.Api.Posts.CreatePost;

public sealed class CreatePostEndpointTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
    : IntegrationTest(integrationTestsWebApplicationFactory)
{
    [Fact]
    public async Task CreatePostAsync_WhenTitleIsNullOrWhiteSpace_ShouldReturnProblemHttpResult()
    {
        // Arrange:
        ProblemHttpResult expectedProblemHttpResult = TypedResults.Problem(
            title: "One or more validation errors occurred.",
            statusCode: (int)HttpStatusCode.UnprocessableEntity);
        var expectedErrors = new List<string> { "The post title cannot be empty." };
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, Constants.BasePostEndpoint)
        {
            Content = new StringContent(
                "{\"title\": \"\", \"content\": \"Content\", \"imagePath\": \"ImagePath\"}",
                Encoding.UTF8,
                MediaTypeNames.Application.Json)
        };

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
    public async Task CreatePostAsync_WhenTitleIsValid_ShouldCreatePost()
    {
        // Arrange:
        const string ExpectedTitle = "Title";
        const string ExpectedContent = "Content";
        const string ExpectedImagePath = "ImagePath";
        var expectedRetrievedPostByIdDto = new RetrievedPostByIdDto(ExpectedTitle, ExpectedContent, ExpectedImagePath, [], It.IsAny<DateTime>());
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, Constants.BasePostEndpoint)
        {
            Content = new StringContent(
                $"{{\"title\": \"{ExpectedTitle}\", \"content\": \"{ExpectedContent}\", \"imagePath\": \"{ExpectedImagePath}\"}}",
                Encoding.UTF8,
                MediaTypeNames.Application.Json)
        };

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        httpResponseMessage.EnsureSuccessStatusCode();
        string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        Guid actualCreatedPostId = JsonSerializer.Deserialize<CreatedPostDto>(responseContent, JsonSerializerOptions)!.Id;
        RetrievedPostByIdDto actualRetrievedPostByIdDto = await GetAsync<RetrievedPostByIdDto>($"{Constants.BasePostEndpoint}/{actualCreatedPostId}");
        actualRetrievedPostByIdDto
            .Should()
            .BeEquivalentTo(
                expectedRetrievedPostByIdDto,
                options => options.Excluding(retrievedPostByIdDtos => retrievedPostByIdDtos.CreatedAt));
    }
}
