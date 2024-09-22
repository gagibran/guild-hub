using GuildHub.Api.Posts.CreatePost;
using GuildHub.Common.RequestHandler;
using GuildHub.Common.ResultHandler;
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
        dispatcherMock
            .Setup(dispatcher => dispatcher.DispatchRequestAsync<CreatePostDto, CreatedPostDto>(It.IsAny<CreatePostDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new CreatedPostDto(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>())));

        // Act:
        await CreatePostEndpoint.CreatePostAsync(dispatcherMock.Object, It.IsAny<HttpContext>(), expectedCreatePostDto, It.IsAny<CancellationToken>());

        // Assert:
        dispatcherMock.Verify(
            dispatcher => dispatcher.DispatchRequestAsync<CreatePostDto, CreatedPostDto>(expectedCreatePostDto, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
