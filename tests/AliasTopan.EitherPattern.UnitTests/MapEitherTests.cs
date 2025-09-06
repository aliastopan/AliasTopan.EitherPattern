using AliasTopan.EitherPattern.UnitTests.Models;

namespace AliasTopan.EitherPattern.UnitTests;

public class MapEitherTests
{
    [Test]
    public async Task Either_WithMap_ShouldTransformTheReturnType()
    {
        // Arrange
        var result1 = Either<Error, Success>.Success(Success.Default);
        var result2 = result1.Map(_ => 10);
        var result3 = result1.Map(value => $"{value}");

        // Act & Assert
        await Assert.That(result1.GetType()).IsEqualTo(typeof(Either<Error, Success>));
        await Assert.That(result2.GetType()).IsEqualTo(typeof(Either<Error, int>));
        await Assert.That(result3.GetType()).IsEqualTo(typeof(Either<Error, string>));
    }

    [Test]
    public async Task Either_WithMapAsync_ShouldTransformTheReturnType()
    {
        // Arrange
        async Task<Either<Error, Success>> SimulateSuccessAsync()
        {
            await Task.Delay(150);
            return Either<Error, Success>.Success(Success.Default);
        }

        var result = await SimulateSuccessAsync()
            .MapAsync(_ => 10)
            .MapAsync(value => $"{value}");

        // Act & Assert
        await Assert.That(result.GetType()).IsEqualTo(typeof(Either<Error, string>));
    }

    [Test]
    public async Task Either_WithMapError_ShouldTransformTheErrorType()
    {
        // Arrange
        var notFound = Error.Create("404");
        var result = Either<Error, Success>.Error(notFound)
            .MapError(err => err.ErrorMessage);

        // Act & Assert
        await Assert.That(result.GetType()).IsEqualTo(typeof(Either<string, Success>));
    }

    [Test]
    public async Task Either_WithMapErrorAsync_ShouldTransformTheErrorType()
    {
        // Arrange
        async Task<Either<Error, Success>> SimulateErrorAsync()
        {
            await Task.Delay(150);
            return Either<Error, Success>.Error(Error.Create("404"));
        }

        var result = await SimulateErrorAsync()
            .MapErrorAsync(err => err.ErrorMessage);

        // Act & Assert
        await Assert.That(result.GetType()).IsEqualTo(typeof(Either<string, Success>));
    }
}
