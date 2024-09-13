using GuildHub.Api.Common.DispatcherPattern;
using GuildHub.Api.Posts.CreatePost;
using Moq;

namespace GuildHub.UnitTests.Posts;

public class CreatePostEndpointTests
{
    [Fact]
    public async Task TestName()
    {
        // Arrange:
        var dispatcherMock = new Mock<IDispatcher>();
        var expectedCreatePostDto = new CreatePostDto("Title", "Content", "ImagePath");

        // Act:
        await CreatePostEndpoint.CreatePostAsync(dispatcherMock.Object, expectedCreatePostDto);

        // Assert:
        dispatcherMock.Verify(
            dispatcher => dispatcher.DispatchAsync<CreatePostDto, PostCreatedDto>(expectedCreatePostDto, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
