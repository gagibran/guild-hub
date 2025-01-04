namespace GuildHub.Common.ResultHandler;

public sealed class UnsuccessfulResultMustHaveErrorTypeWithErrorMessageException : Exception
{
    public UnsuccessfulResultMustHaveErrorTypeWithErrorMessageException()
        : base("Unsuccessful result must have an error type with an error message.")
    {
    }
}