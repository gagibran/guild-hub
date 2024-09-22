namespace GuildHub.Common.ResultHandler;

public sealed class UnsuccessfulResultMustHaveErrorTypeWithErrorMessageException : Exception
{
    public UnsuccessfulResultMustHaveErrorTypeWithErrorMessageException()
        : base("An unsuccessful result must have an error type with an error message.")
    {
    }
}