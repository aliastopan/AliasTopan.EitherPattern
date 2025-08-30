namespace AliasTopan.EitherPattern.ManualTest.Tests;

public sealed class LazyEvaluationPatternTest
{
    public static void RunTest()
    {
        Console.WriteLine("# _LazyEvaluationPatternTest_");

        var error = Either<LazyEvaluationError, LazyEvaluationSuccess>.Error(() =>
        {
            string message = "Despite its being an error, if you reading this, it's a success.";
            return new LazyEvaluationError(message);
        });

        // var success = Either<LazyEvaluationError, LazyEvaluationSuccess>.Success(() =>
        // {
        //     string message = "Success message";
        //     return new LazyEvaluationSuccess(message);
        // });

        error.Match(
            onSuccess: _ => { },
            onError: error =>
            {
                Console.WriteLine($"Error: {error.Message}");
            });

        Console.Write("\n");
    }
}

public record LazyEvaluationError(string Message);
public record LazyEvaluationSuccess(string Message);
