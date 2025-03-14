namespace GuildHub.Api.Posts;

public sealed class Title : ValueObject
{
    public string TitleName { get; }

    private Title(string titleName)
    {
        TitleName = titleName;
    }

    public static Result<Title?> BuildNullable(string? titleName)
    {
        if (titleName is null)
        {
            return Result<Title?>.Succeed(null);
        }
        string trimmedTitle = titleName.Trim();
        if (trimmedTitle == "")
        {
            return Result<Title?>.Fail("The title cannot be empty.");
        }
        if (trimmedTitle.Length > PostConstants.MaxTitleLength)
        {
            return Result<Title?>.Fail($"The title cannot have more than {PostConstants.MaxTitleLength} characters.");
        }
        return Result<Title?>.Succeed(new(trimmedTitle));
    }

    public static Result<Title> Build(string titleName)
    {
        if (string.IsNullOrWhiteSpace(titleName))
        {
            return Result<Title>.Fail("The title cannot be null nor empty.");
        }
        return BuildNullable(titleName)!;
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
