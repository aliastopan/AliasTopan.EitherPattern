namespace AliasTopan.EitherPattern.HarnessTest.Tests;

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
    }

    private static Either<IAuthenticationError, JwtToken> AuthenticateUser(string username, string loginPassword)
    {
        var user = new User(37, username, "hashed_pw_123");

        return GetUser(username)
            .Then(user => VerifyPassword(user, loginPassword))
            .Then(GenerateJwt);
    }

    private static Either<IAuthenticationError, User> GetUser(string username)
    {
        if (username == "erasmus")
        {
            var user = new User(37, username, "hashed_pw_123");
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
            var token = new JwtToken($"jwt_for_user_{user.Id}");
            return Either<IAuthenticationError, JwtToken>.Success(token);
        }
        else
        {
            var jwtGenerationErr = new JwtGenerationError(Reason: "User has invalid Id");
            return Either<IAuthenticationError, JwtToken>.Error(jwtGenerationErr);
        }
    }
}

public record User(int Id, string Username, string HashedPassword);
public record JwtToken(string Value);

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
