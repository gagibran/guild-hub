using GuildHub.Api.Posts.GetPosts;

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
            },
            {
                $"{Constants.BasePostEndpoint}?postsPerPage=1",
                [new($"{ExpectedTitle}1", $"{ExpectedContent}1", $"{ExpectedImagePath}1", [])]

            },
            {
                $"{Constants.BasePostEndpoint}?currentPageIndex=37",
                [
                    new($"{ExpectedTitle}1", $"{ExpectedContent}1", $"{ExpectedImagePath}1", []),
                    new($"{ExpectedTitle}2", $"{ExpectedContent}2", $"{ExpectedImagePath}2", []),
                    new($"{ExpectedTitle}3", $"{ExpectedContent}3", $"{ExpectedImagePath}3", []),
                    new($"{ExpectedTitle}4", $"{ExpectedContent}4", $"{ExpectedImagePath}4", [])
                ]
            }
        };
    }

    public static TheoryData<string, List<RetrievedPostByIdDto>> GetPostsAsyncWhenValidSortByIsAppliedShouldReturnSortedRecordsData()
    {
        return new()
        {
            {
                Constants.BasePostEndpoint,
                [
                    new("Mages are very OP", "Mages should be nerfed.", "OpMage.jpeg", []),
                    new("Mages, mages, mages!", "I'm sick of this OP class!", "KilledByMage.jpeg", []),
                    new("Warrior vs. thief?", "What are some good builds for thieves against warriors?", null, []),
                    new("Warrior mage killer build", "You guys cry too much, check out this build.", "MageKillerBuild.png", [])
                ]
            },
            {
                $"{Constants.BasePostEndpoint}?sortBy=none",
                [
                    new("Mages are very OP", "Mages should be nerfed.", "OpMage.jpeg", []),
                    new("Mages, mages, mages!", "I'm sick of this OP class!", "KilledByMage.jpeg", []),
                    new("Warrior vs. thief?", "What are some good builds for thieves against warriors?", null, []),
                    new("Warrior mage killer build", "You guys cry too much, check out this build.", "MageKillerBuild.png", [])
                ]
            },
            {
                $"{Constants.BasePostEndpoint}?sortBy=date",
                [
                    new("Warrior mage killer build", "You guys cry too much, check out this build.", "MageKillerBuild.png", []),
                    new("Warrior vs. thief?", "What are some good builds for thieves against warriors?", null, []),
                    new("Mages, mages, mages!", "I'm sick of this OP class!", "KilledByMage.jpeg", []),
                    new("Mages are very OP", "Mages should be nerfed.", "OpMage.jpeg", [])
                ]
            },
            {
                $"{Constants.BasePostEndpoint}?sortBy=dateasc",
                [
                    new("Mages are very OP", "Mages should be nerfed.", "OpMage.jpeg", []),
                    new("Mages, mages, mages!", "I'm sick of this OP class!", "KilledByMage.jpeg", []),
                    new("Warrior vs. thief?", "What are some good builds for thieves against warriors?", null, []),
                    new("Warrior mage killer build", "You guys cry too much, check out this build.", "MageKillerBuild.png", [])
                ]
            },
            {
                $"{Constants.BasePostEndpoint}?sortBy=relevance&query=mage",
                [
                    new("Mages, mages, mages!", "I'm sick of this OP class!", "KilledByMage.jpeg", []),
                    new("Mages are very OP", "Mages should be nerfed.", "OpMage.jpeg", []),
                    new("Warrior mage killer build", "You guys cry too much, check out this build.", "MageKillerBuild.png", [])
                ]
            },
            {
                $"{Constants.BasePostEndpoint}?sortBy=relevanceasc&query=mage",
                [
                    new("Warrior mage killer build", "You guys cry too much, check out this build.", "MageKillerBuild.png", []),
                    new("Mages are very OP", "Mages should be nerfed.", "OpMage.jpeg", []),
                    new("Mages, mages, mages!", "I'm sick of this OP class!", "KilledByMage.jpeg", [])
                ]
            },
            {
                $"{Constants.BasePostEndpoint}?sortBy=relevanceasc&query=warrior",
                [
                    new("Warrior mage killer build", "You guys cry too much, check out this build.", "MageKillerBuild.png", []),
                    new("Warrior vs. thief?", "What are some good builds for thieves against warriors?", null, [])
                ]
            },
            {
                $"{Constants.BasePostEndpoint}?sortBy=relevance&query=warrior",
                [
                    new("Warrior vs. thief?", "What are some good builds for thieves against warriors?", null, []),
                    new("Warrior mage killer build", "You guys cry too much, check out this build.", "MageKillerBuild.png", [])
                ]
            },
            {
                $"{Constants.BasePostEndpoint}?sortBy=hot&query=mage",
                [
                    new("Warrior mage killer build", "You guys cry too much, check out this build.", "MageKillerBuild.png", []),
                    new("Mages, mages, mages!", "I'm sick of this OP class!", "KilledByMage.jpeg", []),
                    new("Mages are very OP", "Mages should be nerfed.", "OpMage.jpeg", [])
                ]
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

    [Theory]
    [MemberData(nameof(GetPostsAsyncWhenValidSortByIsAppliedShouldReturnSortedRecordsData))]
    public async Task GetPostsAsync_WhenValidSortByIsApplied_ShouldReturnSortedRecords(string uri, List<RetrievedPostByIdDto> expectedRetrievedPostByIdDtos)
    {
        // Arrange:
        Guid createdPostId1 = (await CreateAsync<CreatedPostDto>(
            "{\"title\": \"Mages are very OP\", \"content\": \"Mages should be nerfed.\", \"imagePath\": \"OpMage.jpeg\"}",
            Constants.BasePostEndpoint)).Id;
        Guid createdPostId2 = (await CreateAsync<CreatedPostDto>(
            "{\"title\": \"Mages, mages, mages!\", \"content\": \"I'm sick of this OP class!\", \"imagePath\": \"KilledByMage.jpeg\"}",
            Constants.BasePostEndpoint)).Id;
        Guid createdPostId4 = (await CreateAsync<CreatedPostDto>(
            "{\"title\": \"Warrior vs. thief?\", \"content\": \"What are some good builds for thieves against warriors?\"}",
            Constants.BasePostEndpoint)).Id;
        Guid createdPostId3 = (await CreateAsync<CreatedPostDto>(
            "{\"title\": \"Warrior mage killer build\", \"content\": \"You guys cry too much, check out this build.\", \"imagePath\": \"MageKillerBuild.png\"}",
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

    [Fact]
    public async Task GetPostsAsync_WhenInValidSortByIsApplied_ShouldReturnProblemHttpResult()
    {
        // Arrange:
        Guid postId = Guid.NewGuid();
        ProblemHttpResult expectedProblemHttpResult = TypedResults.Problem(
            title: "One or more validation errors occurred.",
            statusCode: (int)HttpStatusCode.UnprocessableEntity);
        var expectedErrors = new List<string>
        {
            $"Cannot sort by 'InvalidSortBy'. The valid options are: [{string.Join(", ", Enum.GetNames<SortByType>())}]."
        };
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Constants.BasePostEndpoint}?sortBy=InvalidSortBy");

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

    [Theory]
    [InlineData("Relevance")]
    [InlineData("RelevanceAsc")]
    [InlineData("Hot")]
    public async Task GetPostsAsync_WhenSortingByRankWithoutQuery_ShouldReturnProblemHttpResult(string sortByType)
    {
        // Arrange:
        Guid postId = Guid.NewGuid();
        ProblemHttpResult expectedProblemHttpResult = TypedResults.Problem(
            title: "One or more validation errors occurred.",
            statusCode: (int)HttpStatusCode.UnprocessableEntity);
        var expectedErrors = new List<string> { $"Cannot sort by '{sortByType}' without a search query." };
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Constants.BasePostEndpoint}?sortBy={sortByType}");

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
