using GuildHub.Api.Posts.PostReplies.GetPostReplies;
using GuildHub.Common;

namespace GuildHub.IntegrationTests.Api.Posts.PostReplies.GetPostReplies;

[Collection(nameof(SharedDatabaseFixture))]
public sealed class GetPostRepliesHandlerTests : IntegrationTest
{
    private readonly IRequestHandler<GetPostRepliesRequest, RetrievedPostRepliesDto> _getPostRepliesHandler;

    private static readonly Post s_post = Post.Build("Title", It.IsAny<string>(), It.IsAny<string>()).Value!;
    private static readonly List<PostReply> s_postReplies =
    [
        PostReply.Build(s_post, "Content1", It.IsAny<string>()).Value!,
        PostReply.Build(s_post, "Content2", It.IsAny<string>()).Value!,
        PostReply.Build(s_post, "Content3", It.IsAny<string>()).Value!
    ];

    public GetPostRepliesHandlerTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
        : base(integrationTestsWebApplicationFactory)
    {
        _getPostRepliesHandler = ServiceProvider.GetRequiredService<IRequestHandler<GetPostRepliesRequest, RetrievedPostRepliesDto>>();
    }

    public static TheoryData<SortPostRepliesByType, List<RetrievedPostReplyByIdDto>> HandleAsyncWhenSortByIsValidShouldReturnPostRepliesSortedTestDate()
    {
        return new()
        {
            {
                SortPostRepliesByType.None,
                [
                    MapPostReplyToRetrievedPostReplyByIdDto(s_postReplies[0]),
                    MapPostReplyToRetrievedPostReplyByIdDto(s_postReplies[1]),
                    MapPostReplyToRetrievedPostReplyByIdDto(s_postReplies[2])
                ]
            },
            {
                SortPostRepliesByType.Date,
                [
                    MapPostReplyToRetrievedPostReplyByIdDto(s_postReplies[0]),
                    MapPostReplyToRetrievedPostReplyByIdDto(s_postReplies[1]),
                    MapPostReplyToRetrievedPostReplyByIdDto(s_postReplies[2])
                ]
            },
            {
                SortPostRepliesByType.DateAsc,
                [
                    MapPostReplyToRetrievedPostReplyByIdDto(s_postReplies[2]),
                    MapPostReplyToRetrievedPostReplyByIdDto(s_postReplies[1]),
                    MapPostReplyToRetrievedPostReplyByIdDto(s_postReplies[0])
                ]
            }
        };
    }

    [Fact]
    public async Task HandleAsync_WhenPostIsNull_ShouldReturnFailedResult()
    {
        // Arrange:
        Guid postId = Guid.NewGuid();
        Result<RetrievedPostRepliesDto> expectedResult = Result<RetrievedPostRepliesDto>.Fail($"Post with ID '{postId}' not found.");

        // Act:
        Result<RetrievedPostRepliesDto> actualResult = await _getPostRepliesHandler.HandleAsync(
            new(postId, It.IsAny<QueryParameters>()),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenSearchIsNotNullOrWhiteSpace_ShouldReturnPostRepliesWithSearch()
    {
        // Arrange:
        Post post = Post.Build("Look at my staff", null, "Staff.png").Value!;
        var postReplies = new List<PostReply>
        {
            PostReply.Build(post, "Cool staff, bro.", null).Value!,
            PostReply.Build(post, "I got a similar staff in a dungeon.", "SimilarStaff.png").Value!,
            PostReply.Build(post, "Didn't like it.", null).Value!
        };
        await ApplicationDbContext.Posts.AddAsync(post);
        await ApplicationDbContext.PostReplies.AddRangeAsync(postReplies);
        await ApplicationDbContext.SaveChangesAsync();
        var queryParameters = new QueryParameters("staff", It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string?>());
        Result<RetrievedPostRepliesDto> expectedResult = Result<RetrievedPostRepliesDto>.Succeed(new(
            [
                new(postReplies[0].Id,
                    postReplies[0].Content.ToString(),
                    postReplies[0].ImagePath,
                    postReplies[0].CreatedAtUtc,
                    postReplies[0].UpdatedAtUtc),
                new(postReplies[1].Id,
                    postReplies[1].Content.ToString(),
                    postReplies[1].ImagePath,
                    postReplies[1].CreatedAtUtc,
                    postReplies[1].UpdatedAtUtc)
            ],
            1,
            50,
            2,
            1));

        // Act:
        Result<RetrievedPostRepliesDto> actualResult = await _getPostRepliesHandler.HandleAsync(
            new(post.Id, queryParameters),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult
            .Should()
            .BeEquivalentTo(expectedResult, options => options
                .Using<DateTime>(assertionContext => assertionContext.Subject
                    .Should()
                    .BeCloseTo(assertionContext.Expectation, TimeSpan.FromMilliseconds(1)))
                .When(dto => dto.Path.EndsWith("CreatedAtUtc") || dto.Path.EndsWith("UpdatedAtUtc")));
    }

    [Fact]
    public async Task HandleAsync_WhenSortByIsNotValid_ShouldReturnFailedResult()
    {
        // Arrange:
        Post post = Post.Build("Title", It.IsAny<string>(), It.IsAny<string>()).Value!;
        await ApplicationDbContext.Posts.AddAsync(post);
        await ApplicationDbContext.SaveChangesAsync();
        var queryParameters = new QueryParameters(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), "InvalidSortBy");
        Result<RetrievedPostRepliesDto> expectedResult = Result<RetrievedPostRepliesDto>.Fail(
            $"Cannot sort by 'InvalidSortBy'. The valid options are: [{string.Join(", ", Enum.GetNames<SortPostRepliesByType>())}].");

        // Act:
        Result<RetrievedPostRepliesDto> actualResult = await _getPostRepliesHandler.HandleAsync(
            new(post.Id, queryParameters),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Theory]
    [MemberData(nameof(HandleAsyncWhenSortByIsValidShouldReturnPostRepliesSortedTestDate))]
    public async Task HandleAsync_WhenSortByIsValid_ShouldReturnPostRepliesSorted(
        SortPostRepliesByType sortBy,
        List<RetrievedPostReplyByIdDto> expectedPostReplies)
    {
        // Arrange:
        await ApplicationDbContext.Posts.AddAsync(s_post);
        await ApplicationDbContext.PostReplies.AddRangeAsync(s_postReplies);
        await ApplicationDbContext.SaveChangesAsync();
        var queryParameters = new QueryParameters(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), sortBy.ToString());
        Result<RetrievedPostRepliesDto> expectedResult = Result<RetrievedPostRepliesDto>.Succeed(new(
            expectedPostReplies,
            1,
            50,
            3,
            1));

        // Act:
        Result<RetrievedPostRepliesDto> actualResult = await _getPostRepliesHandler.HandleAsync(
            new(s_post.Id, queryParameters),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult
            .Should()
            .BeEquivalentTo(expectedResult, options => options
                .Using<DateTime>(assertionContext => assertionContext.Subject
                    .Should()
                    .BeCloseTo(assertionContext.Expectation, TimeSpan.FromMilliseconds(1)))
                .When(dto => dto.Path.EndsWith("CreatedAtUtc") || dto.Path.EndsWith("UpdatedAtUtc")));
    }

    private static RetrievedPostReplyByIdDto MapPostReplyToRetrievedPostReplyByIdDto(PostReply postReply)
    {
        return new(
            postReply.Id,
            postReply.Content!.ToString(),
            postReply.ImagePath,
            postReply.CreatedAtUtc,
            postReply.UpdatedAtUtc);
    }
}
