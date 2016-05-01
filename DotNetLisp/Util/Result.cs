using System;
using System.Diagnostics.Contracts;

namespace DotNetLisp.Util
{
    public struct Result<TOk, TError> 
    {
        readonly internal TOk ok;
        readonly internal bool isOk;
        readonly internal TError error;

        private Result(bool isOk, TOk ok = default(TOk), TError error = default(TError))
        {
            this.isOk = isOk;
            if (isOk)
            {
                this.ok = ok;
                this.error = default(TError);
            }
            else
            {
                this.ok = default(TOk);
                this.error = error;
            }
        }

        public static Result<TOk, TError> Ok(TOk value)
        {
            Contract.Requires<ArgumentNullException>(value != null);
            return new Result<TOk, TError>(true, ok: value);
        }

        public static Result<TOk, TError> Error(TError error)
        {
            Contract.Requires<ArgumentNullException>(error != null);
            return new Result<TOk, TError>(false, error: error);
        }

        public Result<TOkOutput, TError> Select<TOkOutput>(Func<TOk, TOkOutput> selector)
        {
            Contract.Requires<ArgumentNullException>(selector != null);
            if (!this.isOk)
                return Result<TOkOutput, TError>.Error(this.error);
            return Result<TOkOutput, TError>.Ok(selector(this.ok));
        }

        public Result<TOk, TErrorOutput> SelectError<TErrorOutput>(Func<TError, TErrorOutput> selector)
        {
            Contract.Requires<ArgumentNullException>(selector != null);
            if (this.isOk)
                return Result<TOk, TErrorOutput>.Ok(this.ok);
            return Result<TOk, TErrorOutput>.Error(selector(this.error));
        }

        public Result<TOk, Exception> SelectAsException()
        {
            return this.SelectError(ex => ex as Exception);
        }

        public TOutput Match<TOutput>(Func<TOk, TOutput> Ok = null, Func<TError, TOutput> Error = null)
        {
            Contract.Requires<ArgumentNullException>(Ok != null);
            Contract.Requires<ArgumentNullException>(Error != null);

            return this.isOk ? Ok(this.ok) : Error(this.error);
        }

        public void Match(Action<TOk> Ok = null, Action<TError> Error = null)
        {
            Contract.Requires<ArgumentNullException>(Ok != null);
            Contract.Requires<ArgumentNullException>(Error != null);

            if (this.isOk)
                Ok(this.ok);
            else
                Error(this.error);
        }

        public override string ToString()
        {
            return isOk ? $"Result.Ok<{typeof(TOk).Name}>: {ok.ToString()}"
                        : $"Result.Error<{typeof(TError).Name}>: {error.ToString()}";
        }

        public static implicit operator Result<TOk, TError>(TOk value)
        {
            Contract.Requires<ArgumentNullException>(value != null);
            return new Result<TOk, TError>(true, ok: value);
        }

        public static implicit operator Result<TOk, TError>(TError error)
        {
            Contract.Requires<ArgumentNullException>(error != null);
            return new Result<TOk, TError>(false, error: error);
        }
    }

    public static class Result
    {
        public static Result<TOk, Exception> Try<TOk>(Func<TOk> func)
        {
            Contract.Requires<ArgumentNullException>(func != null);
            try
            {
                return Result<TOk, Exception>.Ok(func());
            }
            catch (Exception e)
            {
                return Result<TOk, Exception>.Error(e);
            }
        }
    }
}
