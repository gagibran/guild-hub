namespace GuildHub.Api.Posts.ValueObjects;

public sealed class Title : ValueObject
{
    public string TitleName { get; }

    private Title(string titleName)
    {
        TitleName = titleName;
    }

    public static Result<Title> Build(string titleName)
    {
        if (string.IsNullOrWhiteSpace(titleName))
        {
            return Result.Fail<Title>("The post title cannot be empty.");
        }
        if (titleName.Length > Constants.MaxTitleLength)
        {
            return Result.Fail<Title>($"The post title cannot have a more than {Constants.MaxTitleLength}.");
        }
        return Result.Success(new Title(titleName));
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
