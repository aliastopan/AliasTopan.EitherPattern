using System;
using System.Threading.Tasks;

namespace AliasTopan.EitherPattern
{
    public static class EitherAsyncExtensions
    {
        public static async Task<Either<TError, TNewSuccess>> MapAsync<TError, TSuccess, TNewSuccess>(
            this Task<Either<TError, TSuccess>> eitherTask,
            Func<TSuccess, TNewSuccess> transform)
        {
            Either<TError, TSuccess> either = await eitherTask;

            return either.Map(transform);
        }

        public static async Task<Either<TError, TNewSuccess>> ThenAsync<TError, TSuccess, TNewSuccess>(
            this Task<Either<TError, TSuccess>> eitherTask,
            Func<TSuccess, Task<Either<TError, TNewSuccess>>> proceedAsync)
        {
            Either<TError, TSuccess> either = await eitherTask;

            return await either.Match(
                onSuccess: async success => await proceedAsync(success),
                onError: error => Task.FromResult(Either<TError, TNewSuccess>.Error(error))
            );
        }

        public static async Task<Either<TError, TNewSuccess>> Then<TError, TSuccess, TNewSuccess>(
            this Task<Either<TError, TSuccess>> eitherTask,
            Func<TSuccess, Either<TError, TNewSuccess>> proceed)
        {
            Either<TError, TSuccess> either = await eitherTask;

            return either.Then(proceed);
        }

        public static async Task<Either<TError, TSuccess>> Peek<TError, TSuccess>(
            this Task<Either<TError, TSuccess>> eitherTask,
            Action<TSuccess> action)
        {
            Either<TError, TSuccess> either = await eitherTask;
            either.Peek(action);

            return either;
        }

        public static async Task<Either<TError, TSuccess>> PeekError<TError, TSuccess>(
            this Task<Either<TError, TSuccess>> eitherTask,
            Action<TError> action)
        {
            Either<TError, TSuccess> either = await eitherTask;
            either.PeekError(action);

            return either;
        }
    }
}