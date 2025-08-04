namespace AliasTopan.EitherPattern.ManualTest.Tests;

public static class NonGenericSuccessTypeTest
{
    public static void RunTest()
    {
        Console.WriteLine($"# _NonGenericSuccessTypeTest_");

        Either<SaveError, Success> saveUserSettingsResult = SaveUserSettings(isSaved: false);

        saveUserSettingsResult.Match(
            onSuccess: _ =>
            {
                Console.WriteLine($"User setting saved");
            },
            onError: error =>
            {
                Console.WriteLine($"Error: {error.Message}");
            }
        );

        Console.Write("\n");
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
