using AliasTopan.EitherPattern.UnitTests.Models;

namespace AliasTopan.EitherPattern.UnitTests;

public sealed class PeekEitherTests
{
    [Test]
    public void Either_WithPeek_ShouldUnwrapResult()
    {
        // Arrange, Act, & Assert
        var result = Either<Error, string>.Success("200")
            .Peek(async success =>
            {
                await Assert.That(success).IsEqualTo("200");
            });
    }

    [Test]
    public void Either_WithPeekError_ShouldUnwrapErrorResult()
    {
        // Arrange, Act, & Assert
        var result = Either<Error, Success>.Error(Error.Create("404"))
            .PeekError(async error =>
            {
                await Assert.That(error.ErrorMessage).IsEqualTo("404");
            });
    }
}
