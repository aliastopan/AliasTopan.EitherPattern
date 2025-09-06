namespace AliasTopan.EitherPattern.UnitTests.Models;

public class Error
{
    public string ErrorMessage { get; }

    private Error(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public static Error Create(string errorMessage)
    {
        return new Error(errorMessage);
    }
}
