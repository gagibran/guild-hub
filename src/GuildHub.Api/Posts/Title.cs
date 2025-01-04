namespace GuildHub.Api.Posts;

public sealed class Title : ValueObject
{
    public string TitleName { get; }

    private Title(string titleName)
    {
        TitleName = titleName;
    }

    public static Result<Title> Build(string titleName)
    {
        string? trimmedTitleName = titleName?.Trim();
        if (string.IsNullOrWhiteSpace(trimmedTitleName))
        {
            return Result<Title>.Fail("The post title cannot be empty.");
        }
        if (trimmedTitleName.Length > PostConstants.MaxTitleLength)
        {
            return Result<Title>.Fail($"The post title cannot have more than {PostConstants.MaxTitleLength} characters.");
        }
        return Result<Title>.Succeed(new Title(trimmedTitleName));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return TitleName;
    }

    public override string ToString()
    {
        return TitleName;
    }
}
