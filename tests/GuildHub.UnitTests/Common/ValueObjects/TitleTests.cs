using FluentAssertions;
using GuildHub.Api.Posts;
using GuildHub.Api.Posts.ValueObjects;
using GuildHub.Common.ResultHandler;

namespace GuildHub.UnitTests.Common.ValueObjects;

public class TitleTests
{
    [Fact]
    public void CreateTitle_WhenTitleNameIsGreaterThan300Characters_ShouldReturnFailure()
    {
        // Arrange:
        const string TitleName = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec p";

        // Act:
        Result<Title> titleResult = Title.Build(TitleName);

        // Assert:
        titleResult.Should().BeEquivalentTo(Result.Fail($"The post title cannot have a more than {Constants.MaxTitleLength}."));
    }
}
