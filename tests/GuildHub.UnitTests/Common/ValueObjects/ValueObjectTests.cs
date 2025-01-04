namespace GuildHub.UnitTests.Common.ValueObjects;

public class ValueObjectTests
{
    public static TheoryData<TestableValueObject?, TestableValueObject?> OperatorEqualsWhenOneOfTheObjectsIsNullShouldReturnFalseTestData()
    {
        return new TheoryData<TestableValueObject?, TestableValueObject?>
        {
            { null, new TestableValueObject(1, "property2") },
            { new TestableValueObject(1, "property2"), null }
        };
    }

    [Fact]
    public void Equals_WhenPassedObjectIsNull_ShouldReturnFalse()
    {
        // Act:
        bool actualResult = new TestableValueObject(1, "property2").Equals(null);

        // Assert:
        actualResult.Should().BeFalse();
    }

    [Fact]
    public void Equals_WhenPassedObjectIsOfDifferentType_ShouldReturnFalse()
    {
        // Act:
        bool actualResult = new TestableValueObject(1, "property2").Equals(It.IsAny<int>());

        // Assert:
        actualResult.Should().BeFalse();
    }

    [Fact]
    public void Equals_WhenPropertiesAreEqual_ShouldReturnTrue()
    {
        // Arrange:
        var valueObject1 = new TestableValueObject(1, "property2");
        var valueObject2 = new TestableValueObject(1, "property2");

        // Act:
        bool actualResult = valueObject1.Equals(valueObject2);

        // Assert:
        actualResult.Should().BeTrue();
    }

    [Fact]
    public void Equals_WhenPropertiesAreNotEqual_ShouldReturnFalse()
    {
        // Arrange:
        var valueObject1 = new TestableValueObject(1, "property2");
        var valueObject2 = new TestableValueObject(2, "property2");

        // Act:
        bool actualResult = valueObject1.Equals(valueObject2);

        // Assert:
        actualResult.Should().BeFalse();
    }

    [Fact]
    public void OperatorEquals_WhenObjectsAreNull_ShouldReturnTrue()
    {
        // Arrange:
        TestableValueObject? valueObject1 = null;
        TestableValueObject? valueObject2 = null;

        // Act:
        bool actualResult = valueObject1 == valueObject2;

        // Assert:
        actualResult.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(OperatorEqualsWhenOneOfTheObjectsIsNullShouldReturnFalseTestData))]
    public void OperatorEquals_WhenOneOfTheObjectsIsNull_ShouldReturnFalse(TestableValueObject? valueObject1, TestableValueObject? valueObject2)
    {
        // Act:
        bool actualResult = valueObject1 == valueObject2;

        // Assert:
        actualResult.Should().BeFalse();
    }

    [Fact]
    public void OperatorEquals_WhenPropertiesAreEqual_ShouldReturnTrue()
    {
        // Arrange:
        var valueObject1 = new TestableValueObject(1, "property2");
        var valueObject2 = new TestableValueObject(1, "property2");

        // Act:
        bool actualResult = valueObject1 == valueObject2;

        // Assert:
        actualResult.Should().BeTrue();
    }

    [Fact]
    public void OperatorNotEquals_WhenPropertiesAreEqual_ShouldReturnFalse()
    {
        // Arrange:
        var valueObject1 = new TestableValueObject(1, "property2");
        var valueObject2 = new TestableValueObject(1, "property2");

        // Act:
        bool actualResult = valueObject1 != valueObject2;

        // Assert:
        actualResult.Should().BeFalse();
    }

    [Fact]
    public void OperatorNotEqual_WhenPropertiesAreNotEqual_ShouldReturnTrue()
    {
        // Arrange:
        var valueObject1 = new TestableValueObject(1, "property2");
        var valueObject2 = new TestableValueObject(2, "property2");

        // Act:
        bool actualResult = valueObject1 != valueObject2;

        // Assert:
        actualResult.Should().BeTrue();
    }
}
