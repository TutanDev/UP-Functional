
using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;


namespace TD.Functional
{
	using static TDF;


	public static class Option_Monad
	{
		// MAP
		public static Option<R> Map<T, R>(this NoneType _, Func<T, R> f) => None;

		public static Option<R> Map<T, R> (this Option<T> optT, Func<T, R> f)
			=> optT.Match(() => None, (t) => Some(f(t)));

		public static Option<Func<T2, R>> Map<T1, T2, R>(this Option<T1> @this, Func<T1, T2, R> func)
			=> @this.Map(func.Curry());

		public static Option<Func<T2, T3, R>> Map<T1, T2, T3, R>(this Option<T1> @this, Func<T1, T2, T3, R> func)
			=> @this.Map(func.CurryFirst());


		// FOREACH
		public static Option<Unit> ForEach<T>(this Option<T> @this, Action<T> action)
			=> Map(@this, action.ToFunc());


		// BIND
		public static Option<R> Bind<T, R>(this Option<T> optT, Func<T, Option<R>> f)
			=> optT.Match(
				() => None,
				(t) => f(t));

		public static IEnumerable<R> Bind<T, R>(this Option<T> @this, Func<T, IEnumerable<R>> func)
			=> @this.AsEnumerable().Bind(func);
	}



	public static class Option_Utils
	{
		internal static bool IsSome<T>(this Option<T> @this)
			=> @this.Match(
				() => false,
				(_) => true);

		internal static T ValueUnsafe<T>(this Option<T> @this)
			=> @this.Match(
				() => { throw new InvalidOperationException(); },
				(t) => t);

		public static T GetOrElse<T>(this Option<T> opt, T defaultValue)
			=> opt.Match(
				() => defaultValue,
				(t) => t);

		public static T GetOrElse<T>(this Option<T> opt, Func<T> fallback)
		   => opt.Match(
			  () => fallback(),
			  (t) => t);

		public static Option<T> OrElse<T>(this Option<T> left, Option<T> right)
			=> left.Match(
				() => right,
				(_) => left);

		public static Option<T> OrElse<T>(this Option<T> left, Func<Option<T>> right)
			=> left.Match(
				() => right(),
				(_) => left);

		public static Validation<T> ToValidation<T>(this Option<T> opt, Error error)
			=> opt.Match(
				() => Invalid(error),
				(t) => Valid(t));

		public static Validation<T> ToValidation<T>(this Option<T> opt, Func<Error> error)
			=> opt.Match(
				() => Invalid(error()),
				(t) => Valid(t));
	}



	public static class Option_Applicative
	{
		public static Option<R> Apply<T, R>(this Option<Func<T, R>> @this, Option<T> arg)
			=> @this.Match(
				() => None,
				(func) => arg.Match(
					() => None,
					(val) => Some(func(val))));

		public static Option<Func<T2, R>> Apply<T1, T2, R>
		 (this Option<Func<T1, T2, R>> @this, Option<T1> arg)
			=> Apply(@this.Map(TDF.Curry), arg);

		public static Option<Func<T2, T3, R>> Apply<T1, T2, T3, R>
		   (this Option<Func<T1, T2, T3, R>> @this, Option<T1> arg)
		   => Apply(@this.Map(TDF.CurryFirst), arg);

		public static Option<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
		   (this Option<Func<T1, T2, T3, T4, R>> @this, Option<T1> arg)
		   => Apply(@this.Map(TDF.CurryFirst), arg);

		public static Option<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
		   (this Option<Func<T1, T2, T3, T4, T5, R>> @this, Option<T1> arg)
		   => Apply(@this.Map(TDF.CurryFirst), arg);

		public static Option<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
		   (this Option<Func<T1, T2, T3, T4, T5, T6, R>> @this, Option<T1> arg)
		   => Apply(@this.Map(TDF.CurryFirst), arg);

		public static Option<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
		   (this Option<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Option<T1> arg)
		   => Apply(@this.Map(TDF.CurryFirst), arg);

		public static Option<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
		   (this Option<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Option<T1> arg)
		   => Apply(@this.Map(TDF.CurryFirst), arg);

		public static Option<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
		   (this Option<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Option<T1> arg)
		   => Apply(@this.Map(TDF.CurryFirst), arg);
	}



	public static class Option_LINQ
	{
		public static Option<R> Select<T, R>(this Option<T> @this, Func<T, R> func)
		   => @this.Map(func);

		public static Option<T> Where<T>(this Option<T> optT, Func<T, bool> predicate)
		   => optT.Match(
			   () => None,
			   (t) => predicate(t) ? optT : None);

		public static Option<RR> SelectMany<T, R, RR>(this Option<T> opt, Func<T, Option<R>> bind, Func<T, R, RR> project)
		   => opt.Match(
			   () => None,
			   (t) => bind(t).Match(
				   () => None,
				   (r) => Some(project(t, r))));
	}
}
