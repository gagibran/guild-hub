namespace GuildHub.Common.ValueObjects;

public sealed class Content : ValueObject
{
    public string ContentAdded { get; }

    private Content(string contentAdded)
    {
        ContentAdded = contentAdded;
    }

    public static Result<Content?> Build(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return Result<Content?>.Succeed(null);
        }
        string trimmedContent = content.Trim();
        if (trimmedContent.Length > Constants.MaxContentLength)
        {
            return Result<Content?>.Fail($"The post content cannot have more than {Constants.MaxContentLength} characters.");
        }
        return Result<Content?>.Succeed(new Content(trimmedContent));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ContentAdded ?? string.Empty;
    }

    public override string ToString()
    {
        return ContentAdded ?? string.Empty;
    }
}
