namespace AliasTopan.EitherPattern.ManualTest.Tests;

public static class ThenPatternAsyncTest
{
    public static async Task RunTest()
    {
        Console.WriteLine("# _ThenPatternAsyncTest_");

        Either<IAuthenticationError, JwtToken> authenticationResult;

        // authenticationResult = await AuthenticateUserAsync_AsyncFirst("erasmus_async", "password123");
        authenticationResult = await AuthenticateUserAsync_SynchronousFirst("erasmus_async", "password123");

        authenticationResult.Match(
            onSuccess: token =>
            {
                Console.WriteLine($"Login success. [{token.Value}]");
            },
            onError: error =>
            {
                Console.WriteLine($"Login failed. [{error.Message}]");
            }
        );

        Console.Write("\n");
    }

    private static async Task<Either<IAuthenticationError, JwtToken>> AuthenticateUserAsync_AsyncFirst(string username, string loginPassword)
    {
        return await FirstGetUserAsync(username)
            .Then(user => VerifyPassword(user, loginPassword))
            .ThenAsync(GenerateJwtAsync)
                .Peek(token =>
                {
                    Console.WriteLine($"LOG: Jwt lifetime [{token.duration.Hours} hours]");
                })
                .PeekError(error =>
                {
                    Console.WriteLine($"LOG: {error.Message}");
                });
    }

    private static async Task<Either<IAuthenticationError, JwtToken>> AuthenticateUserAsync_SynchronousFirst(string username, string loginPassword)
    {
        return await FirstGetUser(username)
            .Then(user => VerifyPassword(user, loginPassword))
            .ThenAsync(GenerateJwtAsync)
                .Peek(token =>
                {
                    Console.WriteLine($"LOG: Jwt lifetime [{token.duration.Hours} hours]");
                })
                .PeekError(error =>
                {
                    Console.WriteLine($"LOG: {error.Message}");
                });
    }

    private static Either<IAuthenticationError, User> FirstGetUser(string username)
    {
        if (username == "erasmus")
        {
            var user = new User(37, username, "hashed_pw_123", "erasmus@proton.me");
            return Either<IAuthenticationError, User>.Success(user);
        }
        else
        {
            var userNotFoundErr = new UsernameNotFoundError(username);
            return Either<IAuthenticationError, User>.Error(userNotFoundErr);
        }
    }

    private static async Task<Either<IAuthenticationError, User>> FirstGetUserAsync(string username)
    {
        await Task.CompletedTask;

        if (username == "erasmus_async")
        {
            var user = new User(-37, username, "hashed_pw_123", "erasmus@proton.me");
            return Either<IAuthenticationError, User>.Success(user);
        }
        else
        {
            var userNotFoundErr = new UsernameNotFoundError(username);
            return Either<IAuthenticationError, User>.Error(userNotFoundErr);
        }
    }

    private static Either<IAuthenticationError, User> VerifyPassword(User user, string loginPassword)
    {
        if (loginPassword == "password123")
        {
            return Either<IAuthenticationError, User>.Success(user);
        }
        else
        {
            var incorrectPasswordErr = new IncorrectPasswordError();
            return Either<IAuthenticationError, User>.Error(incorrectPasswordErr);
        }
    }

    private static async Task<Either<IAuthenticationError, JwtToken>> GenerateJwtAsync(User user)
    {
        await Task.CompletedTask;

        if (user.Id > 0)
        {
            var token = new JwtToken($"jwt_for_user_{user.Username}", TimeSpan.FromHours(6));
            return Either<IAuthenticationError, JwtToken>.Success(token);
        }
        else
        {
            var jwtGenerationErr = new JwtGenerationError(Reason: "User has invalid Id");
            return Either<IAuthenticationError, JwtToken>.Error(jwtGenerationErr);
        }
    }
}
