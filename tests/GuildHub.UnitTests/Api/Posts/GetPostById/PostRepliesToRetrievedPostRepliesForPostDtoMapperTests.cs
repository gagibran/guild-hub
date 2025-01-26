namespace GuildHub.UnitTests.Api.Posts.GetPostById;

public sealed class PostRepliesToRetrievedPostRepliesForPostDtoMapperTests
{
    private readonly PostRepliesToRetrievedPostRepliesForPostDtoMapper _postRepliesToRetrievedPostRepliesForPostDtoMapper;

    public PostRepliesToRetrievedPostRepliesForPostDtoMapperTests()
    {
        _postRepliesToRetrievedPostRepliesForPostDtoMapper = new PostRepliesToRetrievedPostRepliesForPostDtoMapper();
    }

    [Fact]
    public void Map_WhenPostRepliesArePresent_ShouldReturnRetrievedPostReplyForPostDtos()
    {
        // Arrange:
        var postReplies = new List<PostReply>
        {
            PostReply.Build(It.IsAny<Post>(), "Content", "ImagePath").Value!,
            PostReply.Build(It.IsAny<Post>(), "Content2", "ImagePath2").Value!,
            PostReply.Build(It.IsAny<Post>(), "Content3", "ImagePath3").Value!
        };
        var expectedRetrievedPostReplyForPostDto = new List<RetrievedPostReplyForPostDto>
        {
            new("Content", "ImagePath", It.IsAny<DateTime>()),
            new("Content2", "ImagePath2", It.IsAny<DateTime>()),
            new("Content3", "ImagePath3", It.IsAny<DateTime>())
        };

        // Act:
        List<RetrievedPostReplyForPostDto> actualRetrievedPostReplyForPostDto = _postRepliesToRetrievedPostRepliesForPostDtoMapper.Map(postReplies);

        // Assert:
        actualRetrievedPostReplyForPostDto
            .Should()
            .BeEquivalentTo(
                expectedRetrievedPostReplyForPostDto,
                options => options.Excluding(retrievedPostReplyForPostDto  => retrievedPostReplyForPostDto .CreatedAt));
    }

    [Fact]
    public void Map_WhenNoPostRepliesPresent_ShouldReturnNoRetrievedPostReplyForPostDtos()
    {
        // Act:
        List<RetrievedPostReplyForPostDto> actualRetrievedPostReplyForPostDto = _postRepliesToRetrievedPostRepliesForPostDtoMapper.Map([]);

        // Assert:
        actualRetrievedPostReplyForPostDto.Should().BeEmpty();
    }
}
