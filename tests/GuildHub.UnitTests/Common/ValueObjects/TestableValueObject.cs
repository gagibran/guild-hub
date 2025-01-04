namespace GuildHub.UnitTests.Common.ValueObjects;

public sealed class TestableValueObject(int property1, string property2) : ValueObject
{
    public int Property1 { get; } = property1;
    public string Property2 { get; } = property2;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Property1;
        yield return Property2;
    }
}
