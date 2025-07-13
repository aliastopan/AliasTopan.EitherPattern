namespace AliasTopan.EitherPattern.HarnessTest.Tests;

public static class EitherTest
{
    public static void Run()
    {
        Console.WriteLine("# _EitherTest_");

        Guid targetId = Guid.CreateVersion7();
        Either<LookUpError, LookUpDto> lookUpResult = AccountLookUp(targetId);

        LookUpResponse lookUpResponse = lookUpResult.Match<LookUpResponse>(
            onSuccess: accountFound =>
            {
                return new LookUpResponse(200, accountFound.Username);
            },
            onError: error =>
            {
                return new LookUpResponse(404, error.Message);
            }
        );

        Console.WriteLine($"LookUp: [{lookUpResponse.Code}] {lookUpResponse.Body}");

    }

    private static Either<LookUpError, LookUpDto> AccountLookUp(Guid lookUpId)
    {
        if (lookUpId == Guid.Empty)
        {
            var username = "aliastopan";
            var result = new LookUpDto(username);

            return Either<LookUpError, LookUpDto>.Success(result);
        }
        else
        {
            var message = "not found";
            var error = new LookUpError(message);

            return Either<LookUpError, LookUpDto>.Error(error);
        }
    }
}

public record LookUpError(string Message);
public record LookUpDto(string Username);
public record LookUpResponse(int Code, string Body);