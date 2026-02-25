using System;
using Cysharp.Threading.Tasks;

namespace Tutan.Functional
{
    public static partial class OptionalExtensions
    {
        // ── MapAsync ─────────────────────────────────────────────────────────

        // A: sync optional → async func
        public static async UniTask<Optional<R>> MapAsync<T, R>(this Optional<T> opt, Func<T, UniTask<R>> func)
        {
            if (opt.IsNone) return default;
            return Some(await func(opt._value));
        }

        // B: async optional → sync func
        public static async UniTask<Optional<R>> Map<T, R>(this UniTask<Optional<T>> optTask, Func<T, R> func)
        {
            var opt = await optTask;
            if (opt.IsNone) return default;
            return Some(func(opt._value));
        }

        // C: async optional → async func
        public static async UniTask<Optional<R>> MapAsync<T, R>(this UniTask<Optional<T>> optTask, Func<T, UniTask<R>> func)
        {
            var opt = await optTask;
            if (opt.IsNone) return default;
            return Some(await func(opt._value));
        }


        // ── BindAsync ────────────────────────────────────────────────────────

        // A: sync optional → async func
        public static async UniTask<Optional<R>> BindAsync<T, R>(this Optional<T> opt, Func<T, UniTask<Optional<R>>> func)
        {
            if (opt.IsNone) return default;
            return await func(opt._value);
        }

        // B: async optional → sync func
        public static async UniTask<Optional<R>> Bind<T, R>(this UniTask<Optional<T>> optTask, Func<T, Optional<R>> func)
        {
            var opt = await optTask;
            if (opt.IsNone) return default;
            return func(opt._value);
        }

        // C: async optional → async func
        public static async UniTask<Optional<R>> BindAsync<T, R>(this UniTask<Optional<T>> optTask, Func<T, UniTask<Optional<R>>> func)
        {
            var opt = await optTask;
            if (opt.IsNone) return default;
            return await func(opt._value);
        }


        // ── ThenAsync (on Optional<T>) ───────────────────────────────────────

        // map overload
        public static UniTask<Optional<R>> ThenAsync<T, R>(this Optional<T> opt, Func<T, UniTask<R>> func)
            => opt.MapAsync(func);

        // bind overload
        public static UniTask<Optional<R>> ThenAsync<T, R>(this Optional<T> opt, Func<T, UniTask<Optional<R>>> func)
            => opt.BindAsync(func);

        // async side-effect, pass-through
        public static async UniTask<Optional<T>> ThenAsync<T>(this Optional<T> opt, Func<T, UniTask> action)
        {
            if (opt.IsSome) await action(opt._value);
            return opt;
        }


        // ── Then (on UniTask<Optional<T>>) ───────────────────────────────────

        // sync map
        public static UniTask<Optional<R>> Then<T, R>(this UniTask<Optional<T>> optTask, Func<T, R> func)
            => optTask.Map(func);

        // sync bind
        public static UniTask<Optional<R>> Then<T, R>(this UniTask<Optional<T>> optTask, Func<T, Optional<R>> func)
            => optTask.Bind(func);

        // sync side-effect
        public static async UniTask<Optional<T>> Then<T>(this UniTask<Optional<T>> optTask, Action<T> action)
        {
            var opt = await optTask;
            if (opt.IsSome) action(opt._value);
            return opt;
        }

        // async map
        public static UniTask<Optional<R>> ThenAsync<T, R>(this UniTask<Optional<T>> optTask, Func<T, UniTask<R>> func)
            => optTask.MapAsync(func);

        // async bind
        public static UniTask<Optional<R>> ThenAsync<T, R>(this UniTask<Optional<T>> optTask, Func<T, UniTask<Optional<R>>> func)
            => optTask.BindAsync(func);

        // async side-effect
        public static async UniTask<Optional<T>> ThenAsync<T>(this UniTask<Optional<T>> optTask, Func<T, UniTask> action)
        {
            var opt = await optTask;
            if (opt.IsSome) await action(opt._value);
            return opt;
        }


        // ── MatchAsync ───────────────────────────────────────────────────────

        // A: sync optional → async funcs
        public static async UniTask<R> MatchAsync<T, R>(this Optional<T> opt, Func<UniTask<R>> onNone, Func<T, UniTask<R>> onSome)
            => opt.IsSome ? await onSome(opt._value) : await onNone();

        // B: async optional → sync funcs
        public static async UniTask<R> Match<T, R>(this UniTask<Optional<T>> optTask, Func<R> onNone, Func<T, R> onSome)
        {
            var opt = await optTask;
            return opt.IsSome ? onSome(opt._value) : onNone();
        }

        // C: async optional → async funcs
        public static async UniTask<R> MatchAsync<T, R>(this UniTask<Optional<T>> optTask, Func<UniTask<R>> onNone, Func<T, UniTask<R>> onSome)
        {
            var opt = await optTask;
            return opt.IsSome ? await onSome(opt._value) : await onNone();
        }

        // void B: async optional → sync actions
        public static async UniTask Match<T>(this UniTask<Optional<T>> optTask, Action onNone, Action<T> onSome)
        {
            var opt = await optTask;
            if (opt.IsSome) onSome(opt._value);
            else onNone();
        }
    }
}
