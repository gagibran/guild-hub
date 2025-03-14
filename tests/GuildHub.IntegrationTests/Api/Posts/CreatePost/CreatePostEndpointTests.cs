namespace GuildHub.IntegrationTests.Api.Posts.CreatePost;

[Collection(nameof(SharedDatabaseFixture))]
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
        var expectedErrors = new List<string> { "The title cannot be null nor empty." };
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
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, Constants.BasePostEndpoint)
        {
            Content = new StringContent(
                "{\"title\": \"Title\", \"content\": \"Content\", \"imagePath\": \"ImagePath\"}",
                Encoding.UTF8,
                MediaTypeNames.Application.Json)
        };

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        httpResponseMessage.EnsureSuccessStatusCode();
        string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        CreatedPostDto actualCreatedPostDto = JsonSerializer.Deserialize<CreatedPostDto>(responseContent, JsonSerializerOptions)!;
        CreatedPostDto expectedCreatedPostDto = await ApplicationDbContext.Posts
            .Where(post => post.Id == actualCreatedPostDto.Id)
            .Select(post => new CreatedPostDto(
                post.Id,
                post.Title.ToString(),
                post.Content!.ToString(),
                post.ImagePath))
            .SingleAsync();
        actualCreatedPostDto.Should().BeEquivalentTo(expectedCreatedPostDto);
    }
}
