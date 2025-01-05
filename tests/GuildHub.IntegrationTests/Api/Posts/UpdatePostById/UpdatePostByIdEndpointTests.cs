namespace GuildHub.IntegrationTests.Api.Posts.UpdatePostById;

public sealed class UpdatePostByIdEndpointTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
    : IntegrationTest(integrationTestsWebApplicationFactory)
{
    public static TheoryData<string, RetrievedPostByIdDto> UpdatePostByIdAsyncWhenPostExistsShouldUpdatePostTestData()
    {
        return new TheoryData<string, RetrievedPostByIdDto>
        {
            {
                "{\"title\": \"New Title\", \"content\": \"New Content\", \"imagePath\": \"New ImagePath\"}",
                new(It.IsAny<Guid>(), "New Title", "New Content", "New ImagePath", [], It.IsAny<DateTime>())
            },
            {
                "{\"title\": \"New Title\", \"imagePath\": \"ImagePath\"}",
                new(It.IsAny<Guid>(), "New Title", null, "ImagePath", [], It.IsAny<DateTime>())
            },
            {
                "{\"title\": \"New Title\", \"content\": \"New Content\"}",
                new(It.IsAny<Guid>(), "New Title", "New Content", null, [], It.IsAny<DateTime>())
            },
            {
                "{\"title\": \"New Title\"}",
                new(It.IsAny<Guid>(), "New Title", null, null, [], It.IsAny<DateTime>())
            }
        };
    }

    [Theory]
    [MemberData(nameof(UpdatePostByIdAsyncWhenPostExistsShouldUpdatePostTestData))]
    public async Task UpdatePostByIdAsync_WhenPostExists_ShouldUpdatePost(string body, RetrievedPostByIdDto expectedRetrievedPostByIdDto)
    {
        // Arrange:
        Guid postId = (await CreateAsync<CreatedPostDto>(
            "{\"title\": \"Title\", \"content\": \"Content\", \"imagePath\": \"ImagePath\"}",
            Constants.BasePostEndpoint)).Id;
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"{Constants.BasePostEndpoint}/{postId}")
        {
            Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        httpResponseMessage.EnsureSuccessStatusCode();
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
        RetrievedPostByIdDto actualRetrievedPostByIdDto = await GetAsync<RetrievedPostByIdDto>($"{Constants.BasePostEndpoint}/{postId}");
        actualRetrievedPostByIdDto
            .Should()
            .BeEquivalentTo(
                expectedRetrievedPostByIdDto,
                options => options
                    .Excluding(retrievedPostByIdDtos => retrievedPostByIdDtos.CreatedAt)
                    .Excluding(retrievedPostByIdDtos => retrievedPostByIdDtos.Id));
    }

    [Fact]
    public async Task UpdatePostByIdAsync_WhenPostDoesNotExist_ShouldReturnProblemHttpResult()
    {
        // Arrange:
        Guid postId = Guid.NewGuid();
        ProblemHttpResult expectedProblemHttpResult = TypedResults.Problem(
            title: "One or more validation errors occurred.",
            statusCode: (int)HttpStatusCode.NotFound);
        var expectedErrors = new List<string> { $"No post with the ID '{postId}' was found." };
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"{Constants.BasePostEndpoint}/{postId}")
        {
            Content = new StringContent(
                "{\"title\": \"Title\", \"content\": \"Content\", \"imagePath\": \"ImagePath\"}",
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
    public async Task UpdatePostByIdAsync_WhenUpdateFails_ShouldReturnProblemHttpResult()
    {
        // Arrange:
        Guid postId = (await CreateAsync<CreatedPostDto>(
            "{\"title\": \"Title\", \"content\": \"Content\", \"imagePath\": \"ImagePath\"}",
            Constants.BasePostEndpoint)).Id;        ProblemHttpResult expectedProblemHttpResult = TypedResults.Problem(
            title: "One or more validation errors occurred.",
            statusCode: (int)HttpStatusCode.UnprocessableEntity);
        var expectedErrors = new List<string> { "The title cannot be empty." };
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"{Constants.BasePostEndpoint}/{postId}")
        {
            Content = new StringContent(
                "{\"content\": \"Content\", \"imagePath\": \"ImagePath\"}",
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
}
