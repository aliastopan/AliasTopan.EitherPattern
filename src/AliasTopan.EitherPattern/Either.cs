using System;
using System.Threading.Tasks;

namespace AliasTopan.EitherPattern
{
    public sealed class Either<TError, TSuccess>
    {
        private readonly TError _error;
        private readonly TSuccess _success;
        private readonly bool _isSuccess;

        public bool IsError => !_isSuccess;
        public bool IsSuccess => _isSuccess;

        private Either(TError error)
        {
            _error = error;
            _success = default;
            _isSuccess = false;
        }

        private Either(TSuccess success)
        {
            _error = default;
            _success = success;
            _isSuccess = true;
        }

        public static Either<TError, TSuccess> Success(TSuccess value)
        {
            return new Either<TError, TSuccess>(value);
        }

        public static Either<TError, TSuccess> Success(Func<TSuccess> onSuccess)
        {
            TSuccess value = onSuccess.Invoke();
            return new Either<TError, TSuccess>(value);
        }

        public static Either<TError, TSuccess> Error(TError value)
        {
            return new Either<TError, TSuccess>(value);
        }

        public static Either<TError, TSuccess> Error(Func<TError> onError)
        {
            TError value = onError.Invoke();
            return new Either<TError, TSuccess>(value);
        }

        public TResult Match<TResult>(Func<TSuccess, TResult> onSuccess, Func<TError, TResult> onError)
        {
            if (onSuccess == null)
                throw new ArgumentNullException(nameof(onSuccess));

            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return _isSuccess ? onSuccess(_success) : onError(_error);
        }

        public void Match(Action<TSuccess> onSuccess, Action<TError> onError)
        {
            if (onSuccess == null)
                throw new ArgumentNullException(nameof(onSuccess));

            if (onError == null)
                throw new ArgumentNullException(nameof(onSuccess));

            if (_isSuccess)
                onSuccess(_success);
            else
                onError(_error);
        }

        public Either<TError, TNewSuccess> Map<TNewSuccess>(Func<TSuccess, TNewSuccess> transfrom)
        {
            return _isSuccess
                ? Either<TError, TNewSuccess>.Success(transfrom(_success))
                : Either<TError, TNewSuccess>.Error(_error);
        }

        public Either<TNewError, TSuccess> MapError<TNewError>(Func<TError, TNewError> transform)
        {
            return _isSuccess
                ? Either<TNewError, TSuccess>.Success(_success)
                : Either<TNewError, TSuccess>.Error(transform(_error));
        }

        public Either<TError, TNewSuccess> Then<TNewSuccess>(Func<TSuccess, Either<TError, TNewSuccess>> proceed)
        {
            return _isSuccess
                ? proceed(_success)
                : Either<TError, TNewSuccess>.Error(_error);
        }

        public Either<TError, TSuccess> Peek(Action<TSuccess> action)
        {
            if (_isSuccess)
                action(_success);

            return this;
        }

        public Either<TError, TSuccess> PeekError(Action<TError> action)
        {
            if (!_isSuccess)
                action(_error);

            return this;
        }

        public override string ToString()
        {
            return _isSuccess
                ? $"Success: {_success?.ToString() ?? "null"}"
                : $"Error: {_error?.ToString() ?? "null"}";
        }
    }
}