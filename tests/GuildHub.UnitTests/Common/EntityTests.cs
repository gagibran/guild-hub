namespace GuildHub.UnitTests.Common;

public class EntityTests
{
    public static TheoryData<TestableEntity?, TestableEntity?> OperatorEqualsWhenOneOfTheEntitiesIsNullShouldReturnFalseTestData()
    {
        return new TheoryData<TestableEntity?, TestableEntity?>
        {
            { null, new TestableEntity() },
            { new TestableEntity(), null }
        };
    }

    [Fact]
    public void Equals_WhenPassedEntityIsNull_ShouldReturnFalse()
    {
        // Act:
        var actualResult = new TestableEntity().Equals(null);

        // Assert:
        actualResult.Should().BeFalse();
    }

    [Fact]
    public void Equals_WhenPassedEntityIsOfDifferentType_ShouldReturnFalse()
    {
        // Act:
        var actualResult = new TestableEntity().Equals(It.IsAny<int>());

        // Assert:
        actualResult.Should().BeFalse();
    }

    [Fact]
    public void Equals_WhenIdsAreEqual_ShouldReturnTrue()
    {
        // Arrange:
        var entity = new TestableEntity();
        var entity2 = entity;

        // Act:
        var actualResult = entity.Equals(entity2);

        // Assert:
        actualResult.Should().BeTrue();
    }

    [Fact]
    public void Equals_WhenIdsAreNotEqual_ShouldReturnFalse()
    {
        // Arrange:
        var entity1 = new TestableEntity();
        var entity2 = new TestableEntity();

        // Act:
        var actualResult = entity1.Equals(entity2);

        // Assert:
        actualResult.Should().BeFalse();
    }

    [Fact]
    public void OperatorEquals_WhenEntitiesAreNull_ShouldReturnTrue()
    {
        // Arrange:
        TestableEntity? entity1 = null;
        TestableEntity? entity2 = null;

        // Act:
        var actualResult = entity1 == entity2;

        // Assert:
        actualResult.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(OperatorEqualsWhenOneOfTheEntitiesIsNullShouldReturnFalseTestData))]
    public void OperatorEquals_WhenOneOfTheEntitiesIsNull_ShouldReturnFalse(TestableEntity? entity1, TestableEntity? entity2)
    {
        // Act:
        bool actualResult = entity1 == entity2;

        // Assert:
        actualResult.Should().BeFalse();
    }

    [Fact]
    public void OperatorEquals_WhenIdsAreEqual_ShouldReturnTrue()
    {
        // Arrange:
        var entity = new TestableEntity();
        var entity2 = entity;

        // Act:
        var actualResult = entity == entity2;

        // Assert:
        actualResult.Should().BeTrue();
    }

    [Fact]
    public void OperatorNotEquals_WhenIdsAreEqual_ShouldReturnFalse()
    {
        // Arrange:
        var entity = new TestableEntity();
        var entity2 = entity;

        // Act:
        var actualResult = entity != entity2;

        // Assert:
        actualResult.Should().BeFalse();
    }

    [Fact]
    public void OperatorNotEquals_WhenIdsAreNotEqual_ShouldReturnTrue()
    {
        // Arrange:
        var entity1 = new TestableEntity();
        var entity2 = new TestableEntity();

        // Act:
        var actualResult = entity1 != entity2;

        // Assert:
        actualResult.Should().BeTrue();
    }
}
