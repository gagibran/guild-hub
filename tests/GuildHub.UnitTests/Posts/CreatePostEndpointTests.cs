using GuildHub.Api.Posts.CreatePost;
using GuildHub.Common.RequestHandler;
using Microsoft.AspNetCore.Http;

namespace GuildHub.UnitTests.Posts;

public class CreatePostEndpointTests
{
    [Fact]
    public async Task TestName()
    {
        // Arrange:
        var dispatcherMock = new Mock<IRequestDispatcher>();
        var expectedCreatePostDto = new CreatePostDto("Title", "Content", "ImagePath");

        // Act:
        await CreatePostEndpoint.CreatePostAsync(dispatcherMock.Object, It.IsAny<HttpContext>(), expectedCreatePostDto, It.IsAny<CancellationToken>());

        // Assert:
        dispatcherMock.Verify(
            dispatcher => dispatcher.DispatchRequestAsync<CreatePostDto, CreatedPostDto>(expectedCreatePostDto, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
