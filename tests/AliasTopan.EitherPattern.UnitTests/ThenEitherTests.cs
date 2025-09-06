using AliasTopan.EitherPattern.UnitTests.Models;

namespace AliasTopan.EitherPattern.UnitTests;

public sealed class ThenEitherTests
{
    [Test]
    public void EitherThen_WithUninterruptedSuccess_ShouldReturnLastChainValue()
    {
        // Arrange
        Either<Error, int> MultiplyByFive(int input) => Either<Error, int>.Success(input * 5);
        Either<Error, int> MultiplyByTen(int input) => Either<Error, int>.Success(input * 10);

        var result = Either<Error, int>.Success(2)  // 2
            .Then(MultiplyByFive)                   // 2 * 5 = 10
            .Then(MultiplyByTen);                   // 10 * 10 = 100

        // Act & Assert
        result.Match(
            onSuccess: async score =>
            {
                await Assert.That(score).IsEqualTo(100);
            },
            onError: _ =>
            {
                Assert.Fail("Should not fail.");
            }
        );
    }

    [Test]
    public void EitherThen_WithErrorInterruption_ShouldReturnError()
    {
        // Arrange
        Either<Error, int> MultiplyByFive(int input) => Either<Error, int>.Error(Error.Create("Interrupt"));
        Either<Error, int> MultiplyByTen(int input) => Either<Error, int>.Success(input * 10);

        var result = Either<Error, int>.Success(2)  // 2
            .Then(MultiplyByFive)                   // Interrupt
            .Then(MultiplyByTen);                   // 10 * 10 = 100


        // Act & Assert
        result.Match(
            onSuccess: _ =>
            {
                Assert.Fail("Should not success.");
            },
            onError: async error =>
            {
                await Assert.That(error.ErrorMessage).IsEqualTo("Interrupt");
            }
        );
    }
}
