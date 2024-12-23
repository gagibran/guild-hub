namespace GuildHub.IntegrationTests.Api.Posts.GetPosts;

public sealed class GetPostsEndpointTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
    : IntegrationTest(integrationTestsWebApplicationFactory)
{
    private const string ExpectedTitle = "Title";
    private const string ExpectedContent = "Content";
    private const string ExpectedImagePath = "ImagePath";

    public static TheoryData<string, List<RetrievedPostByIdDto>> GetPostsAsyncWhenMultipleRecordsArePagedShouldReturnPagedRecordsData()
    {
        return new()
        {
            {
                Constants.BasePostEndpoint,
                [
                    new($"{ExpectedTitle}1", $"{ExpectedContent}1", $"{ExpectedImagePath}1", []),
                    new($"{ExpectedTitle}2", $"{ExpectedContent}2", $"{ExpectedImagePath}2", []),
                    new($"{ExpectedTitle}3", $"{ExpectedContent}3", $"{ExpectedImagePath}3", []),
                    new($"{ExpectedTitle}4", $"{ExpectedContent}4", $"{ExpectedImagePath}4", [])
                ]
            },
            {
                $"{Constants.BasePostEndpoint}?currentPageIndex=2&postsPerPage=2",
                [
                    new($"{ExpectedTitle}3", $"{ExpectedContent}3", $"{ExpectedImagePath}3", []),
                    new($"{ExpectedTitle}4", $"{ExpectedContent}4", $"{ExpectedImagePath}4", [])
                ]
            },
            {
                $"{Constants.BasePostEndpoint}?currentPageIndex=1&postsPerPage=3",
                [
                    new($"{ExpectedTitle}1", $"{ExpectedContent}1", $"{ExpectedImagePath}1", []),
                    new($"{ExpectedTitle}2", $"{ExpectedContent}2", $"{ExpectedImagePath}2", []),
                    new($"{ExpectedTitle}3", $"{ExpectedContent}3", $"{ExpectedImagePath}3", [])
                ]
            },
            {
                $"{Constants.BasePostEndpoint}?currentPageIndex=2&postsPerPage=3",
                [new($"{ExpectedTitle}4", $"{ExpectedContent}4", $"{ExpectedImagePath}4", [])]
            },
            {
                $"{Constants.BasePostEndpoint}?currentPageIndex=1&postsPerPage=4",
                [
                    new($"{ExpectedTitle}1", $"{ExpectedContent}1", $"{ExpectedImagePath}1", []),
                    new($"{ExpectedTitle}2", $"{ExpectedContent}2", $"{ExpectedImagePath}2", []),
                    new($"{ExpectedTitle}3", $"{ExpectedContent}3", $"{ExpectedImagePath}3", []),
                    new($"{ExpectedTitle}4", $"{ExpectedContent}4", $"{ExpectedImagePath}4", [])
                ]
            },
            {
                $"{Constants.BasePostEndpoint}?currentPageIndex=100&postsPerPage=4",
                [
                    new($"{ExpectedTitle}1", $"{ExpectedContent}1", $"{ExpectedImagePath}1", []),
                    new($"{ExpectedTitle}2", $"{ExpectedContent}2", $"{ExpectedImagePath}2", []),
                    new($"{ExpectedTitle}3", $"{ExpectedContent}3", $"{ExpectedImagePath}3", []),
                    new($"{ExpectedTitle}4", $"{ExpectedContent}4", $"{ExpectedImagePath}4", [])
                ]
            },
            {
                $"{Constants.BasePostEndpoint}?currentPageIndex=1&postsPerPage=1",
                [new($"{ExpectedTitle}1", $"{ExpectedContent}1", $"{ExpectedImagePath}1", [])]
            },
            {
                $"{Constants.BasePostEndpoint}?currentPageIndex=200&postsPerPage=1",
                [new($"{ExpectedTitle}4", $"{ExpectedContent}4", $"{ExpectedImagePath}4", [])]
            }
        };
    }

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
        var expectedRetrievedPostByIdDtos = new List<RetrievedPostByIdDto>
        {
            new($"{ExpectedTitle}1", $"{ExpectedContent}1", $"{ExpectedImagePath}1", []),
            new($"{ExpectedTitle}2", $"{ExpectedContent}2", $"{ExpectedImagePath}2", []),
            new($"{ExpectedTitle}3", $"{ExpectedContent}3", $"{ExpectedImagePath}3", [])
        };
        var createdPostId1 = (await CreateAsync<CreatedPostDto>(
            $"{{\"title\": \"{ExpectedTitle}1\", \"content\": \"{ExpectedContent}1\", \"imagePath\": \"{ExpectedImagePath}1\"}}",
            Constants.BasePostEndpoint)).Id;
        var createdPostId2 = (await CreateAsync<CreatedPostDto>(
            $"{{\"title\": \"{ExpectedTitle}2\", \"content\": \"{ExpectedContent}2\", \"imagePath\": \"{ExpectedImagePath}2\"}}",
            Constants.BasePostEndpoint)).Id;
        var createdPostId3 = (await CreateAsync<CreatedPostDto>(
            $"{{\"title\": \"{ExpectedTitle}3\", \"content\": \"{ExpectedContent}3\", \"imagePath\": \"{ExpectedImagePath}3\"}}",
            Constants.BasePostEndpoint)).Id;
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, Constants.BasePostEndpoint);

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        httpResponseMessage.EnsureSuccessStatusCode();
        string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        List<RetrievedPostByIdDto>? actualRetrievedPostByIdDtos = JsonSerializer.Deserialize<List<RetrievedPostByIdDto>>(responseContent, JsonSerializerOptions);
        actualRetrievedPostByIdDtos.Should().BeEquivalentTo(expectedRetrievedPostByIdDtos);

        // Clean up:
        await HttpClient.DeleteAsync($"{Constants.BasePostEndpoint}/{createdPostId1}");
        await HttpClient.DeleteAsync($"{Constants.BasePostEndpoint}/{createdPostId2}");
        await HttpClient.DeleteAsync($"{Constants.BasePostEndpoint}/{createdPostId3}");
    }

    [Theory]
    [MemberData(nameof(GetPostsAsyncWhenMultipleRecordsArePagedShouldReturnPagedRecordsData))]
    public async Task GetPostsAsync_WhenMultipleRecordsArePaged_ShouldReturnPagedRecords(string uri, List<RetrievedPostByIdDto> expectedRetrievedPostByIdDtos)
    {
        // Arrange:
        Guid createdPostId1 = (await CreateAsync<CreatedPostDto>(
            $"{{\"title\": \"{ExpectedTitle}1\", \"content\": \"{ExpectedContent}1\", \"imagePath\": \"{ExpectedImagePath}1\"}}",
            Constants.BasePostEndpoint)).Id;
        Guid createdPostId2 = (await CreateAsync<CreatedPostDto>(
            $"{{\"title\": \"{ExpectedTitle}2\", \"content\": \"{ExpectedContent}2\", \"imagePath\": \"{ExpectedImagePath}2\"}}",
            Constants.BasePostEndpoint)).Id;
        Guid createdPostId3 = (await CreateAsync<CreatedPostDto>(
            $"{{\"title\": \"{ExpectedTitle}3\", \"content\": \"{ExpectedContent}3\", \"imagePath\": \"{ExpectedImagePath}3\"}}",
            Constants.BasePostEndpoint)).Id;
        Guid createdPostId4 = (await CreateAsync<CreatedPostDto>(
            $"{{\"title\": \"{ExpectedTitle}4\", \"content\": \"{ExpectedContent}4\", \"imagePath\": \"{ExpectedImagePath}4\"}}",
            Constants.BasePostEndpoint)).Id;
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        httpResponseMessage.EnsureSuccessStatusCode();
        string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        List<RetrievedPostByIdDto>? actualRetrievedPostByIdDtos = JsonSerializer.Deserialize<List<RetrievedPostByIdDto>>(responseContent, JsonSerializerOptions);
        actualRetrievedPostByIdDtos.Should().BeEquivalentTo(expectedRetrievedPostByIdDtos);

        // Clean up:
        await HttpClient.DeleteAsync($"{Constants.BasePostEndpoint}/{createdPostId1}");
        await HttpClient.DeleteAsync($"{Constants.BasePostEndpoint}/{createdPostId2}");
        await HttpClient.DeleteAsync($"{Constants.BasePostEndpoint}/{createdPostId3}");
        await HttpClient.DeleteAsync($"{Constants.BasePostEndpoint}/{createdPostId4}");
    }
}
