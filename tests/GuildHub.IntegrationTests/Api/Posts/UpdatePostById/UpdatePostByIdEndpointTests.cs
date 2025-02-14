using GuildHub.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GuildHub.IntegrationTests.Api.Posts.UpdatePostById;

[Collection(nameof(SharedDatabaseFixture))]
public sealed class UpdatePostByIdEndpointTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
    : IntegrationTest(integrationTestsWebApplicationFactory)
{
    public static TheoryData<string, List<string>> UpdatePostByIdAsyncWhenPostExistsShouldUpdatePostTestData()
    {
        return new TheoryData<string, List<string>>
        {
            {
                "{\"title\": \"New Title\", \"content\": \"New Content\", \"imagePath\": \"New ImagePath\"}",
                ["New Title", "New Content", "New ImagePath"]
            },
            {
                "{\"title\": \"New Title\", \"imagePath\": \"ImagePath\"}",
                ["New Title", null, "ImagePath"]
            },
            {
                "{\"title\": \"New Title\", \"content\": \"New Content\"}",
                ["New Title", "New Content", null]
            },
            {
                "{\"title\": \"New Title\"}",
                ["New Title", null, null]
            }
        };
    }

    [Theory]
    [MemberData(nameof(UpdatePostByIdAsyncWhenPostExistsShouldUpdatePostTestData))]
    public async Task UpdatePostByIdAsync_WhenPostExists_ShouldUpdatePost(string body, List<string> expectedRetrievedPost)
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;
        await ApplicationDbContext.AddAsync(post);
        await ApplicationDbContext.SaveChangesAsync();
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"{Constants.BasePostEndpoint}/{post.Id}")
        {
            Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        httpResponseMessage.EnsureSuccessStatusCode();
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
        ApplicationDbContext.Entry(post).State = EntityState.Detached;
        Post actualRetrievedPost = (await ApplicationDbContext.Posts.FindAsync(post.Id))!;
        actualRetrievedPost.Id.Should().Be(post.Id);
        actualRetrievedPost.Title.ToString().Should().Be(expectedRetrievedPost[0]);
        actualRetrievedPost.Content?.ToString().Should().Be(expectedRetrievedPost[1]);
        actualRetrievedPost.ImagePath.Should().Be(expectedRetrievedPost[2]);
        actualRetrievedPost.CreatedAtUtc.Should().BeCloseTo(post.CreatedAtUtc, TimeSpan.FromMilliseconds(1));
        actualRetrievedPost.UpdatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(100));
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
        await AssertProblemDetailsAsync(httpResponseMessage, expectedErrors, expectedProblemHttpResult);
    }

    [Fact]
    public async Task UpdatePostByIdAsync_WhenUpdateFails_ShouldReturnProblemHttpResult()
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;
        await ApplicationDbContext.AddAsync(post);
        await ApplicationDbContext.SaveChangesAsync();
        ProblemHttpResult expectedProblemHttpResult = TypedResults.Problem(
            title: "One or more validation errors occurred.",
            statusCode: (int)HttpStatusCode.UnprocessableEntity);
        var expectedErrors = new List<string> { "The title cannot be empty." };
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"{Constants.BasePostEndpoint}/{post.Id}")
        {
            Content = new StringContent(
                "{\"content\": \"Content\", \"imagePath\": \"ImagePath\"}",
                Encoding.UTF8,
                MediaTypeNames.Application.Json)
        };

        // Act:
        HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);

        // Assert:
        await AssertProblemDetailsAsync(httpResponseMessage, expectedErrors, expectedProblemHttpResult);
    }
}
