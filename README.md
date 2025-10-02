![Nuget](https://img.shields.io/nuget/v/AliasTopan.EitherPattern?style=flat-square)
![Nuget](https://img.shields.io/nuget/dt/AliasTopan.EitherPattern?style=flat-square)
![GitHub](https://img.shields.io/github/license/aliastopan/AliasTopan.EitherPattern?style=flat-square)


A dead-simple, functional-inspired result library for .NET featuring a robust `Either` type. This library provides a clean, type-safe, and intuitive way to handle the success or failure of operations without relying on exceptions for control flow or nullable return types.

## Philosophy

This library is built on a "dead-simple" philosophy. Business logic is complex enough; the tools we use to build it shouldn't be.

*   **Explicit over Implicit:** The library's behavior is transparent. There is no hidden magic.
*   **Simple, Composable Tools:** It provides a small set of simple, powerful methods (`Map`, `Then`, `Peek`) that can be composed to build complex, resilient workflows.
*   **No Over-Engineering:** The `Either` type does one thing and does it well: it represents one of two possible outcomes. It does not try to be a kitchen-sink solution.

## Installation

Install the package from the .NET CLI:

```sh
dotnet add package AliasTopan.EitherPattern --version 1.0.0-preview.4
```

## Core Concepts

The library provides two central types: `Either<TError, TSuccess>` and `Success`.

### `Either<TError, TSuccess>`

This is the core of the library. It's a container that holds one of two possible values:
*   An `Error` of type `TError`.
*   A `Success` of type `TSuccess`.

An `Either` can never hold both at the same time. This forces you to handle both the success and failure paths, preventing a whole class of bugs.

By convention, the "happy" or success path is the **right**-hand type parameter (`TSuccess`), and the alternative or error path is the **left**-hand type parameter (`TError`).

### `Success` (struct)

A simple, lightweight struct used as the `TSuccess` type for operations that don't return a value (i.e., methods that would normally return `void`). Its presence signifies a successful operation that produced no data.

## Getting Started: Basic Usage

Let's create a function that can fail, like parsing a string to an integer.

```csharp
using AliasTopan.EitherPattern;

public record ParseError(string Message);

public class Parser
{
    public static Either<ParseError, int> ParseInteger(string input)
    {
        if (int.TryParse(input, out int number))
        {
            // The operation succeeded. Return a Success containing the number.
            return Either<ParseError, int>.Success(number);
        }
        else
        {
            // The operation failed. Return an Error containing a descriptive error object.
            var error = new ParseError($"Input '{input}' is not a valid integer.");
            return Either<ParseError, int>.Error(error);
        }
    }
}
```

To get the value out safely, you use the `.Match()` method, which forces you to provide a handler for both outcomes.

```csharp
var successfulResult = Parser.ParseInteger("123");
var failedResult = Parser.ParseInteger("abc");

string message = successfulResult.Match(
    onSuccess: number => $"Parsed number is: {number}",
    onError:   error  => $"Failed to parse. Reason: {error.Message}"
);
// message is "Parsed number is: 123"

string errorMessage = failedResult.Match(
    onSuccess: number => $"Parsed number is: {number}",
    onError:   error  => $"Failed to parse. Reason: {error.Message}"
);
// errorMessage is "Failed to parse. Reason: Input 'abc' is not a valid integer."
```

## Key Features

### Transforming a Success Value with `.Map()`

Use `.Map()` to apply a simple, non-failable transformation to a success value. If the `Either` is an `Error`, `Map` does nothing.

```csharp
Either<ParseError, int> eitherInt = Parser.ParseInteger("123"); // Success(123)

// Transform the int to a string message
Either<ParseError, string> eitherMessage = eitherInt
    .Map(num => num * 10) // Becomes Success(1230)
    .Map(num => $"The final value is {num}"); // Becomes Success("The final value is 1230")

// eitherMessage.ToString() -> "Success: The final value is 1230"
```

### Chaining Failable Operations with `.Then()`

This is the most powerful feature. Use `.Then()` to chain multiple operations where each step can fail. If any step returns an `Error`, the entire rest of the chain is skipped.

```csharp
// Step 1: Can fail
Either<IError, string> ReadFile() { /* ... */ }
// Step 2: Can fail
Either<IError, Config> ParseConfig(string text) { /* ... */ }
// Step 3: Can fail
Either<IError, Success> ConnectToDatabase(Config config) { /* ... */ }

// The workflow is a clean, flat chain:
Either<IError, Success> result = ReadFile()
    .Then(text => ParseConfig(text))
    .Then(config => ConnectToDatabase(config));

result.Match(
    onSuccess: _ => Console.WriteLine("Process completed successfully!"),
    onError: err => Console.Error.WriteLine($"Workflow failed: {err.Message}")
);
```

### Inspecting a Value with `.Peek()`

Use `.Peek()` to perform a side-effect, like logging, without breaking the chain. It allows you to "peek" at the value inside and then returns the original `Either` unchanged.

```csharp
var result = ReadFile()
    .Peek(text => // Inspect the success
    {
        Console.WriteLine("DEBUG: File read successfully.")
    })
    .PeekError(err => // Inspect the error
    {
        Console.Error.WriteLine($"ERROR: Could not read file: {err.Message}")
    })
    .Then(text => ParseConfig(text));
```

## Advanced Usage

### Handling Valueless Successes

For an operation that returns `void` on success, use the `Success` struct as your `TSuccess` type.

```csharp
public record SaveError(string Message);

public Either<SaveError, Success> SaveSettings(string settings)
{
    try
    {
        // db.SaveChanges(settings);
        return Either<SaveError, Success>.Success(Success.Default);
    }
    catch (Exception ex)
    {
        return Either<SaveError, Success>.Error(new SaveError(ex.Message));
    }
}
```

### Architectural Pattern: Defining Business Boundaries

The `Either` type encourages good architectural patterns like Domain-Driven Design (DDD) by allowing you to define clear boundaries for your business logic.

You can define a shared error interface for a specific workflow (a "Bounded Context"). All functions within that workflow return an `Either` using that common error interface.

```csharp
// A common interface for all authentication errors
public interface IAuthenticationError { string Message { get; } }

public record UsernameNotFoundError(string Username) : IAuthenticationError { /* ... */ }
public record IncorrectPasswordError() : IAuthenticationError { /* ... */ }

// All functions in the workflow use the common interface in their return type.
public class AuthService
{
    private Either<IAuthenticationError, User> GetUser(string username) { /* ... */ }
    private Either<IAuthenticationError, User> VerifyPassword(User user, string password) { /* ... */ }
    private Either<IAuthenticationError, JwtToken> GenerateJwt(User user) { /* ... */ }

    // This allows for a perfectly clean and readable workflow.
    public Either<IAuthenticationError, JwtToken> Authenticate(string username, string password)
    {
        return GetUser(username)
            .Then(user => VerifyPassword(user, password))
            .Then(GenerateJwt);
    }
}
```
This architectural style keeps the high-level business logic simple and readable, as the error handling policy is enforced by the function signatures within the boundary.
