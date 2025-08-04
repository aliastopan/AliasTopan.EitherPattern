namespace AliasTopan.EitherPattern.ManualTest.Tests;

public static class ThenPatternTest
{
    public static void Run()
    {
        Console.WriteLine("# _ThenPatternTest_");

        Either<IAuthenticationError, JwtToken> authenticationResult = AuthenticateUser("erasmus", "password123");

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

    private static Either<IAuthenticationError, JwtToken> AuthenticateUser(string username, string loginPassword)
    {
        return GetUser(username)
                .Peek(user =>
                {
                    Console.WriteLine($"LOG: User.Email [{user.Email}]");
                })
            .Then(user => VerifyPassword(user, loginPassword))
            .Then(GenerateJwt)
                .Peek(token =>
                {
                    Console.WriteLine($"LOG: Jwt lifetime [{token.duration.Hours} hours]");
                });
    }

    private static Either<IAuthenticationError, User> GetUser(string username)
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

    private static Either<IAuthenticationError, JwtToken> GenerateJwt(User user)
    {
        if (user.Id > 0)
        {
            var token = new JwtToken($"jwt_for_user_{user.Id}", TimeSpan.FromHours(6));
            return Either<IAuthenticationError, JwtToken>.Success(token);
        }
        else
        {
            var jwtGenerationErr = new JwtGenerationError(Reason: "User has invalid Id");
            return Either<IAuthenticationError, JwtToken>.Error(jwtGenerationErr);
        }
    }
}

public record User(int Id, string Username, string HashedPassword, string Email);
public record JwtToken(string Value, TimeSpan duration);

public interface IAuthenticationError
{
    string Message { get; }
}

public record UsernameNotFoundError(string Username) : IAuthenticationError
{
    public string Message => $"User `{Username} not found.`";
}

public record IncorrectPasswordError() : IAuthenticationError
{
    public string Message => $"The provided password was incorrect.";
}

public record JwtGenerationError(string Reason) : IAuthenticationError
{
    public string Message => $"Could not generate token: {Reason}";
}
