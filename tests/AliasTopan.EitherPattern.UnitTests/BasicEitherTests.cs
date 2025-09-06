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
}
