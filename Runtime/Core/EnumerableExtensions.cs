using System;
using System.Collections.Generic;
using System.Linq;

namespace TD.Functional
{
	public static class EnumerableExtensions 
    {
		public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> list, Func<T, IEnumerable<R>> func)
		  => list.SelectMany(func);

		public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> list, Func<T, Option<R>> func)
		  => list.Bind(t => func(t).AsEnumerable());

	}
}
