namespace GuildHub.IntegrationTests.Api.Posts.GetPosts;

public sealed class GetPostsEndpointTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
    : IntegrationTest(integrationTestsWebApplicationFactory)
{
    [Fact]
    public async Task GetPostsAsync_WhenNoRecordsInTheDatabase_ShouldReturnEmptyList()
    {
        // Arrange:
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Constants.BasePostEndpoint}");

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        httpResponseMessage.EnsureSuccessStatusCode();
        string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        List<RetrievedPostByIdDto>? actualRetrievedPostByIdDtos = JsonSerializer.Deserialize<List<RetrievedPostByIdDto>>(responseContent, JsonSerializerOptions);
        actualRetrievedPostByIdDtos.Should().BeEmpty();
    }

    [Fact]
    public async Task GetPostsAsync_WhenAtLeastOneRecordInTheDatabase_ShouldReturnRecords()
    {
        // Arrange:
        const string ExpectedTitle = "Title";
        const string ExpectedContent = "Content";
        const string ExpectedImagePath = "ImagePath";
        var expectedRetrievedPostByIdDtos = new List<RetrievedPostByIdDto>
        {
            new(ExpectedTitle, ExpectedContent, ExpectedImagePath, []),
            new($"{ExpectedTitle}1", $"{ExpectedContent}1", $"{ExpectedImagePath}1", []),
            new($"{ExpectedTitle}2", $"{ExpectedContent}2", $"{ExpectedImagePath}2", [])
        };
        await CreateAsync<CreatedPostDto>(
            $"{{\"title\": \"{ExpectedTitle}\", \"content\": \"{ExpectedContent}\", \"imagePath\": \"{ExpectedImagePath}\"}}",
            Constants.BasePostEndpoint);
        await CreateAsync<CreatedPostDto>(
            $"{{\"title\": \"{ExpectedTitle}1\", \"content\": \"{ExpectedContent}1\", \"imagePath\": \"{ExpectedImagePath}1\"}}",
            Constants.BasePostEndpoint);
        await CreateAsync<CreatedPostDto>(
            $"{{\"title\": \"{ExpectedTitle}2\", \"content\": \"{ExpectedContent}2\", \"imagePath\": \"{ExpectedImagePath}2\"}}",
            Constants.BasePostEndpoint);
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Constants.BasePostEndpoint}");

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        httpResponseMessage.EnsureSuccessStatusCode();
        string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        List<RetrievedPostByIdDto>? actualRetrievedPostByIdDtos = JsonSerializer.Deserialize<List<RetrievedPostByIdDto>>(responseContent, JsonSerializerOptions);
        actualRetrievedPostByIdDtos.Should().BeEquivalentTo(expectedRetrievedPostByIdDtos);
    }
}
