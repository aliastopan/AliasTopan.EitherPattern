using AliasTopan.EitherPattern.UnitTests.Models;

namespace AliasTopan.EitherPattern.UnitTests;

public class MatchEitherTests
{
    [Test]
    public void EitherMatch_WithSuccessDefault_ShouldReturnSuccessType()
    {
        // Arrange
        var result = Either<Error, Success>.Success(Success.Default);

        // Act & Assert
        result.Match(
            onSuccess: async success =>
            {
                await Assert.That(success).IsEqualTo(Success.Default);
            },
            onError: _ =>
            {
                Assert.Fail("Should not fail.");
            }
        );
    }

    [Test]
    public async Task EitherMatchAsync_WithSuccessDefault_ShouldReturnSuccessType()
    {
        // Arrange
        var result = Either<Error, Success>.Success(Success.Default);

        // Act & Assert
        await result.MatchAsync(
            onSuccess: async success =>
            {
                await Assert.That(success).IsEqualTo(Success.Default);
            },
            onError: _ =>
            {
                Assert.Fail("Should not fail.");
                return Task.FromResult(_);
            }
        );
    }

    [Test]
    public void EitherMatch_WithError_ShouldReturnErrorType()
    {
        // Arrange
        var notFound = Error.Create("404");
        var result = Either<Error, Success>.Error(notFound);

        // Act & Assert
        result.Match(
            onSuccess: _ =>
            {
                Assert.Fail("Should not success.");
            },
            onError: async error =>
            {
                await Assert.That(error).IsEqualTo(notFound);
            }
        );
    }

    [Test]
    public async Task EitherMatchAsync_WithError_ShouldReturnErrorType()
    {
        // Arrange
        var notFound = Error.Create("404");
        var result = Either<Error, Success>.Error(notFound);

        // Act & Assert
        await result.MatchAsync(
            onSuccess: _ =>
            {
                Assert.Fail("Should not success.");
                return Task.FromResult(_);
            },
            onError: async error =>
            {
                await Assert.That(error).IsEqualTo(notFound);
            }
        );
    }
}
