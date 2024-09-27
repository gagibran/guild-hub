namespace GuildHub.UnitTests.Common;

public sealed class ValueObjectTests
{
    public static TheoryData<Title?, Title?> EqualsOperatorWhenAtLeastOneValueObjectIsNotNullShouldReturnFalseData()
    {
        return new()
        {
            { Title.Build("Title").Value, null },
            { null, Title.Build("Title").Value },
        };
    }

    [Fact]
    public void Equals_WhenObjIsNull_ShouldReturnFalse()
    {
        // Arrange:
        Title title = Title.Build("Title").Value!;

        // Act:
        bool actualEquals = title.Equals(null);

        // Assert:
        actualEquals.Should().BeFalse();
    }

    [Fact]
    public void Equals_WhenObjTypeIsDifferentFromEntity_ShouldReturnFalse()
    {
        // Arrange:
        Title title = Title.Build("Title").Value!;
        var post = new Post(It.IsAny<Title>(), It.IsAny<string>(), It.IsAny<string>());

        // Act:
        bool actualEquals = title.Equals(post);

        // Assert:
        actualEquals.Should().BeFalse();
    }

    [Fact]
    public void Equals_WhenEqualityComponentsAreEquals_ShouldReturnTrue()
    {
        // Arrange:
        Title title1 = Title.Build("Title").Value!;
        Title title2 = Title.Build("Title").Value!;

        // Act:
        bool actualEquals = title1.Equals(title2);

        // Assert:
        actualEquals.Should().BeTrue();
    }

    [Fact]
    public void EqualsOperator_WhenTheTwoValueObjectsAreNull_ShouldReturnTrue()
    {
        // Arrange:
        Title? title1 = null;
        Title? title2 = null;

        // Act:
        bool actualEquals = title1! == title2!;

        // Assert:
        actualEquals.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(EqualsOperatorWhenAtLeastOneValueObjectIsNotNullShouldReturnFalseData))]
    public void EqualsOperator_WhenAtLeastOneValueObjectIsNotNull_ShouldReturnFalse(Title? title1, Title? title2)
    {
        // Act:
        bool actualEquals = title1! == title2!;

        // Assert:
        actualEquals.Should().BeFalse();
    }

    [Fact]
    public void EqualsOperator_WhenEqualityComponentsAreEquals_ShouldReturnTrue()
    {
        // Arrange:
        Title title1 = Title.Build("Title").Value!;
        Title title2 = Title.Build("Title").Value!;

        // Act:
        bool actualEquals = title1 == title2;

        // Assert:
        actualEquals.Should().BeTrue();
    }
}
