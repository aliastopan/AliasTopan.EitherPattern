namespace AliasTopan.EitherPattern.HarnessTest.Tests;

public static class NonGenericSuccessTypeTest
{
    public static void Run()
    {
        Console.WriteLine($"# _NonGenericSuccessTypeTest_");

        Either<SaveError, Success> eitherSave = SaveUserSettings(isSaved: false);

        eitherSave.Match(
            onSuccess: _ =>
            {
                Console.WriteLine($"User setting saved");
            },
            onError: error =>
            {
                Console.WriteLine($"Error: {error.Message}");
            }
        );
    }

    private static Either<SaveError, Success> SaveUserSettings(bool isSaved)
    {
        if (isSaved)
        {
            return Either<SaveError, Success>.Success(Success.Default);
        }
        else
        {
            SaveError error = new SaveError("Unable to save user settings");
            return Either<SaveError, Success>.Error(error);
        }
    }

}

public record SaveError(string Message);
