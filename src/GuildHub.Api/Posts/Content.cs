namespace GuildHub.Api.Posts;

public sealed class Content : ValueObject
{
    public string Message { get; }

    private Content(string message)
    {
        Message = message;
    }

    public static Result<Content?> BuildNullable(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return Result<Content?>.Succeed(null);
        }
        string trimmedContent = content.Trim();
        if (trimmedContent.Length > Constants.MaxContentMessageLength)
        {
            return Result<Content?>.Fail($"The content message cannot have more than {Constants.MaxContentMessageLength} characters.");
        }
        return Result<Content?>.Succeed(new Content(trimmedContent));
    }

    public static Result<Content> Build(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return Result<Content>.Fail($"The content message cannot be null nor empty.");
        }
        return BuildNullable(content)!;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Message ?? string.Empty;
    }

    public override string ToString()
    {
        return Message ?? string.Empty;
    }
}
