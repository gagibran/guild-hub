using GuildHub.Api.Posts.GetPosts;

namespace GuildHub.IntegrationTests.Api.Posts.GetPosts;

[Collection(nameof(SharedDatabaseFixture))]
public sealed class GetPostsEndpointTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
    : IntegrationTest(integrationTestsWebApplicationFactory)
{
    private static readonly List<Post> s_posts =
    [
        Post.Build("Mages are very OP", "Mages should be nerfed.", "OpMage.jpeg").Value!,
        Post.Build("Mages, mages, mages!", "I'm sick of this OP class!", "KilledByMage.jpeg").Value!,
        Post.Build("Warrior vs. thief?", "What are some good builds for thieves against warriors?", null).Value!,
        Post.Build("Warrior mage killer build", "You guys cry too much, check out this build.", "MageKillerBuild.png").Value!
    ];

    public static TheoryData<string, List<RetrievedPostByIdDto>> GetPostsAsyncWhenMultipleRecordsArePagedShouldReturnPagedRecordsTestData()
    {
        return new()
        {
            {
                "currentPageIndex=2&postsPerPage=2",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[2]),
                    MapPostToRetrievedPostByIdDto(s_posts[3])
                ]
            },
            {
                "currentPageIndex=1&postsPerPage=3",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[0]),
                    MapPostToRetrievedPostByIdDto(s_posts[1]),
                    MapPostToRetrievedPostByIdDto(s_posts[2])
                ]
            },
            {
                "currentPageIndex=2&postsPerPage=3",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[3])
                ]
            },
            {
                "currentPageIndex=1&postsPerPage=4",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[0]),
                    MapPostToRetrievedPostByIdDto(s_posts[1]),
                    MapPostToRetrievedPostByIdDto(s_posts[2]),
                    MapPostToRetrievedPostByIdDto(s_posts[3])
                ]
            },
            {
                "currentPageIndex=100&postsPerPage=4",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[0]),
                    MapPostToRetrievedPostByIdDto(s_posts[1]),
                    MapPostToRetrievedPostByIdDto(s_posts[2]),
                    MapPostToRetrievedPostByIdDto(s_posts[3])
                ]
            },
            {
                "currentPageIndex=1&postsPerPage=1",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[0])
                ]
            },
            {
                "currentPageIndex=200&postsPerPage=1",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[3])
                ]
            },
            {
                "currentPageIndex=-2&postsPerPage=57",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[0]),
                    MapPostToRetrievedPostByIdDto(s_posts[1]),
                    MapPostToRetrievedPostByIdDto(s_posts[2]),
                    MapPostToRetrievedPostByIdDto(s_posts[3])
                ]
            },
            {
                "currentPageIndex=0&postsPerPage=3",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[0]),
                    MapPostToRetrievedPostByIdDto(s_posts[1]),
                    MapPostToRetrievedPostByIdDto(s_posts[2])
                ]
            },
            {
                "currentPageIndex=2&postsPerPage=99",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[0]),
                    MapPostToRetrievedPostByIdDto(s_posts[1]),
                    MapPostToRetrievedPostByIdDto(s_posts[2]),
                    MapPostToRetrievedPostByIdDto(s_posts[3])
                ]
            },
            {
                "postsPerPage=-4",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[0])
                ]
            },
            {
                "postsPerPage=0",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[0])
                ]
            },
            {
                "postsPerPage=2",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[0]),
                    MapPostToRetrievedPostByIdDto(s_posts[1])
                ]
            },
            {
                "postsPerPage=242",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[0]),
                    MapPostToRetrievedPostByIdDto(s_posts[1]),
                    MapPostToRetrievedPostByIdDto(s_posts[2]),
                    MapPostToRetrievedPostByIdDto(s_posts[3])
                ]
            }
        };
    }

    public static TheoryData<string, List<RetrievedPostByIdDto>> GetPostsAsyncWhenValidSortByIsAppliedShouldReturnSortedRecordsData()
    {
        return new()
        {
            {
                "sortBy=none",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[0]),
                    MapPostToRetrievedPostByIdDto(s_posts[1]),
                    MapPostToRetrievedPostByIdDto(s_posts[2]),
                    MapPostToRetrievedPostByIdDto(s_posts[3])
                ]
            },
            {
                "sortBy=date",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[3]),
                    MapPostToRetrievedPostByIdDto(s_posts[2]),
                    MapPostToRetrievedPostByIdDto(s_posts[1]),
                    MapPostToRetrievedPostByIdDto(s_posts[0])
                ]
            },
            {
                "sortBy=dateasc",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[0]),
                    MapPostToRetrievedPostByIdDto(s_posts[1]),
                    MapPostToRetrievedPostByIdDto(s_posts[2]),
                    MapPostToRetrievedPostByIdDto(s_posts[3])
                ]
            },
            {
                "sortBy=relevance&query=mage",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[1]),
                    MapPostToRetrievedPostByIdDto(s_posts[0]),
                    MapPostToRetrievedPostByIdDto(s_posts[3])
                ]
            },
            {
                "sortBy=relevanceasc&query=mage",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[3]),
                    MapPostToRetrievedPostByIdDto(s_posts[0]),
                    MapPostToRetrievedPostByIdDto(s_posts[1])
                ]
            },
            {
                "sortBy=relevanceasc&query=warrior",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[3]),
                    MapPostToRetrievedPostByIdDto(s_posts[2])
                ]
            },
            {
                "sortBy=relevance&query=warrior",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[2]),
                    MapPostToRetrievedPostByIdDto(s_posts[3])
                ]
            },
            {
                "sortBy=hot&query=mage",
                [
                    MapPostToRetrievedPostByIdDto(s_posts[3]),
                    MapPostToRetrievedPostByIdDto(s_posts[1]),
                    MapPostToRetrievedPostByIdDto(s_posts[0])
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
        await ApplicationDbContext.Posts.AddAsync(s_posts[0]);
        await ApplicationDbContext.SaveChangesAsync();
        List<RetrievedPostByIdDto> expectedRetrievedPostByIdDtos = [MapPostToRetrievedPostByIdDto(s_posts[0])];

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(new(HttpMethod.Get, Constants.BasePostEndpoint));

        // Assert:
        await AssertModelWithoutDateAuditsAsync(httpResponseMessage, expectedRetrievedPostByIdDtos);
    }

    [Theory]
    [MemberData(nameof(GetPostsAsyncWhenMultipleRecordsArePagedShouldReturnPagedRecordsTestData))]
    public async Task GetPostsAsync_WhenMultipleRecordsArePaged_ShouldReturnPagedRecords(
        string paginationQuery,
        List<RetrievedPostByIdDto> expectedRetrievedPostByIdDtos)
    {
        // Arrange:
        foreach (Post post in s_posts)
        {
            await ApplicationDbContext.Posts.AddAsync(post);
            await ApplicationDbContext.SaveChangesAsync();
        }
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Constants.BasePostEndpoint}?{paginationQuery}");

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        await AssertModelWithoutDateAuditsAsync(httpResponseMessage, expectedRetrievedPostByIdDtos);
    }

    [Theory]
    [MemberData(nameof(GetPostsAsyncWhenValidSortByIsAppliedShouldReturnSortedRecordsData))]
    public async Task GetPostsAsync_WhenValidSortByIsApplied_ShouldReturnSortedRecords(string sortQuery, List<RetrievedPostByIdDto> expectedRetrievedPostByIdDtos)
    {
        // Arrange:
        await ApplicationDbContext.Posts.AddRangeAsync(s_posts);
        await ApplicationDbContext.SaveChangesAsync();
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Constants.BasePostEndpoint}?{sortQuery}");

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        await AssertModelWithoutDateAuditsAsync(httpResponseMessage, expectedRetrievedPostByIdDtos);
    }

    [Fact]
    public async Task GetPostsAsync_WhenInValidSortByIsApplied_ShouldReturnProblemHttpResult()
    {
        // Arrange:
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
        await AssertProblemDetailsAsync(httpResponseMessage, expectedErrors, expectedProblemHttpResult);
    }

    [Theory]
    [InlineData("Relevance")]
    [InlineData("RelevanceAsc")]
    [InlineData("Hot")]
    public async Task GetPostsAsync_WhenSortingByRankWithoutQuery_ShouldReturnProblemHttpResult(string sortByType)
    {
        // Arrange:
        ProblemHttpResult expectedProblemHttpResult = TypedResults.Problem(
            title: "One or more validation errors occurred.",
            statusCode: (int)HttpStatusCode.UnprocessableEntity);
        var expectedErrors = new List<string> { $"Cannot sort by '{sortByType}' without a search query." };
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Constants.BasePostEndpoint}?sortBy={sortByType}");

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        await AssertProblemDetailsAsync(httpResponseMessage, expectedErrors, expectedProblemHttpResult);
    }

    private static RetrievedPostByIdDto MapPostToRetrievedPostByIdDto(Post post)
    {
        return new(
            post.Id,
            post.Title.ToString(),
            post.Content!.ToString(),
            post.ImagePath,
            [
                .. post.PostReplies.Select(postReply => new RetrievedPostReplyForPostDto(
                    postReply.Content.ToString(),
                    postReply.ImagePath,
                    postReply.CreatedAtUtc))
            ],
            post.CreatedAtUtc,
            post.UpdatedAtUtc);
    }
}
