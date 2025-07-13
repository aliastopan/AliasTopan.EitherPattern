using System;

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

        public static Either<TError, TSuccess> Error(TError value)
        {
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

        public override string ToString()
        {
            return _isSuccess
                ? $"Success: {_success?.ToString() ?? "null"}"
                : $"Error: {_error?.ToString() ?? "null"}";
        }
    }
}