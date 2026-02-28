 using System;
using Cysharp.Threading.Tasks;

namespace Tutan.Functional
{
    public static partial class ResultExtensions
    {
        // ── MapAsync ─────────────────────────────────────────────────────────

        // A: sync result → async func
        public static async UniTask<Result<R>> MapAsync<T, R>(this Result<T> result, Func<T, UniTask<R>> func)
        {
            if (result.IsError) return new Result<R>(result._error);
            return Success(await func(result._value));
        }

        // B: async result → sync func
        public static async UniTask<Result<R>> Map<T, R>(this UniTask<Result<T>> resultTask, Func<T, R> func)
        {
            var result = await resultTask;
            if (result.IsError) return new Result<R>(result._error);
            return Success(func(result._value));
        }

        // C: async result → async func
        public static async UniTask<Result<R>> MapAsync<T, R>(this UniTask<Result<T>> resultTask, Func<T, UniTask<R>> func)
        {
            var result = await resultTask;
            if (result.IsError) return new Result<R>(result._error);
            return Success(await func(result._value));
        }


        // ── BindAsync ────────────────────────────────────────────────────────

        // A: sync result → async func
        public static async UniTask<Result<R>> BindAsync<T, R>(this Result<T> result, Func<T, UniTask<Result<R>>> func)
        {
            if (result.IsError) return new Result<R>(result._error);
            return await func(result._value);
        }

        // B: async result → sync func
        public static async UniTask<Result<R>> Bind<T, R>(this UniTask<Result<T>> resultTask, Func<T, Result<R>> func)
        {
            var result = await resultTask;
            if (result.IsError) return new Result<R>(result._error);
            return func(result._value);
        }

        // C: async result → async func
        public static async UniTask<Result<R>> BindAsync<T, R>(this UniTask<Result<T>> resultTask, Func<T, UniTask<Result<R>>> func)
        {
            var result = await resultTask;
            if (result.IsError) return new Result<R>(result._error);
            return await func(result._value);
        }


        // ── ThenAsync (on Result<T>) ─────────────────────────────────────────

        // map overload
        public static UniTask<Result<R>> ThenAsync<T, R>(this Result<T> result, Func<T, UniTask<R>> func)
            => result.MapAsync(func);

        // bind overload
        public static UniTask<Result<R>> ThenAsync<T, R>(this Result<T> result, Func<T, UniTask<Result<R>>> func)
            => result.BindAsync(func);

        // async side-effect, pass-through
        public static async UniTask<Result<T>> ThenAsync<T>(this Result<T> result, Func<T, UniTask> action)
        {
            if (result.IsSuccess) await action(result._value);
            return result;
        }


        // ── Then (on UniTask<Result<T>>) ─────────────────────────────────────

        // sync map
        public static UniTask<Result<R>> Then<T, R>(this UniTask<Result<T>> resultTask, Func<T, R> func)
            => resultTask.Map(func);

        // sync bind
        public static UniTask<Result<R>> Then<T, R>(this UniTask<Result<T>> resultTask, Func<T, Result<R>> func)
            => resultTask.Bind(func);

        // sync side-effect
        public static async UniTask<Result<T>> Then<T>(this UniTask<Result<T>> resultTask, Action<T> action)
        {
            var result = await resultTask;
            if (result.IsSuccess) action(result._value);
            return result;
        }

        // async map
        public static UniTask<Result<R>> ThenAsync<T, R>(this UniTask<Result<T>> resultTask, Func<T, UniTask<R>> func)
            => resultTask.MapAsync(func);

        // async bind
        public static UniTask<Result<R>> ThenAsync<T, R>(this UniTask<Result<T>> resultTask, Func<T, UniTask<Result<R>>> func)
            => resultTask.BindAsync(func);

        // async side-effect
        public static async UniTask<Result<T>> ThenAsync<T>(this UniTask<Result<T>> resultTask, Func<T, UniTask> action)
        {
            var result = await resultTask;
            if (result.IsSuccess) await action(result._value);
            return result;
        }


        // ── MatchAsync ───────────────────────────────────────────────────────

        // A: sync result → async funcs
        public static async UniTask<R> MatchAsync<T, R>(this Result<T> result, Func<Error, UniTask<R>> onError, Func<T, UniTask<R>> onSuccess)
            => result.IsSuccess ? await onSuccess(result._value) : await onError(result._error);

        // B: async result → sync funcs
        public static async UniTask<R> Match<T, R>(this UniTask<Result<T>> resultTask, Func<Error, R> onError, Func<T, R> onSuccess)
        {
            var result = await resultTask;
            return result.IsSuccess ? onSuccess(result._value) : onError(result._error);
        }

        // C: async result → async funcs
        public static async UniTask<R> MatchAsync<T, R>(this UniTask<Result<T>> resultTask, Func<Error, UniTask<R>> onError, Func<T, UniTask<R>> onSuccess)
        {
            var result = await resultTask;
            return result.IsSuccess ? await onSuccess(result._value) : await onError(result._error);
        }

        // void B: async result → sync actions
        public static async UniTask Match<T>(this UniTask<Result<T>> resultTask, Action<Error> onError, Action<T> onSuccess)
        {
            var result = await resultTask;
            if (result.IsSuccess) onSuccess(result._value);
            else onError(result._error);
        }
    }
}
