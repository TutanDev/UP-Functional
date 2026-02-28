using System;
using System.Collections.Generic;
using System.Linq;

namespace Tutan.Functional
{
    public static class EnumerableExt
    {
        public static Optional<T> Head<T>(this IEnumerable<T> list)
        {
            if (list == null) return default;
            using var enumerator = list.GetEnumerator();
            return enumerator.MoveNext() ? Some(enumerator.Current) : default;
        }

        public static Optional<T> FindFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate)
            => source.Where(predicate).Head();

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> list)
            => list.SelectMany(x => x);

        public static R Match<T, R>(this IEnumerable<T> list,
            Func<R> Empty,
            Func<T, IEnumerable<T>, R> Otherwise)
            => list.Head()
                .Match(Empty, head => Otherwise(head, list.Skip(1)));

        public static IEnumerable<T> DropWhile<T>(this IEnumerable<T> @this, Func<T, bool> pred)
        {
            bool clean = true;
            foreach (var item in @this)
            {
                if (!clean || !pred(item))
                {
                    yield return item;
                    clean = false;
                }
            }
        }

        // ── Return ──────────────────────────────────────────────

        public static Func<T, IEnumerable<T>> Return<T>() => t => List(t);

        // ── Map ────────────────────────────────────────────────

        public static IEnumerable<R> Map<T, R>
            (this IEnumerable<T> list, Func<T, R> func)
            => list.Select(func);

        public static IEnumerable<Func<T2, R>> Map<T1, T2, R>
            (this IEnumerable<T1> list, Func<T1, T2, R> func)
            => list.Map(func.Curry());

        public static IEnumerable<Func<T2, Func<T3, R>>> Map<T1, T2, T3, R>
            (this IEnumerable<T1> opt, Func<T1, T2, T3, R> func)
            => opt.Map(func.Curry());

        // ── ForEach ─────────────────────────────────────────────

        public static IEnumerable<Unit> ForEach<T>(this IEnumerable<T> ts, Action<T> action)
            => ts.Map(action.ToFunc());

        // ── Bind ───────────────────────────────────────────────

        public static IEnumerable<R> Bind<T, R>
            (this IEnumerable<T> list, Func<T, IEnumerable<R>> func)
            => list.SelectMany(func);

        public static IEnumerable<R> Bind<T, R>
            (this IEnumerable<T> list, Func<T, Optional<R>> func)
          => list.Bind(t => func(t).AsEnumerable());
    }
}
