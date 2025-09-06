using AliasTopan.EitherPattern.UnitTests.Models;

namespace AliasTopan.EitherPattern.UnitTests;

public sealed class BasicEitherTests
{
    [Test]
    public async Task Either_WithSuccessResult_ShouldReturnSuccess()
    {
        // Arrange
        var result = Either<Error, Success>.Success(Success.Default);

        // Act & Assert
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task Either_WithErrorResult_ShouldReturnError()
    {
        // Arrange
        var notFound = Error.Create("404");
        var result = Either<Error, Success>.Error(notFound);

        // Act & Assert
        await Assert.That(result.IsError).IsTrue();
    }

    [Test]
    public void Either_WithDefaultSuccess_ShouldReturnSuccessType()
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
    public void Either_WithError_ShouldReturnErrorType()
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
}
