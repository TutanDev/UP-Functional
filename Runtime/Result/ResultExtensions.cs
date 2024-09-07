using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;

namespace TD.Functional
{
	using static TDF;

	public static class Result_Monad
	{
		// MAP
		public static Result<R> Map<T, R>(this Result<T> result, Func<T, R> f)
			=> result.Match(
				Success: s => Success(f(s)),
				Fail: e => e);

		public static Result<Func<T2, R>> Map<T1, T2, R>(this Result<T1> @this, Func<T1, T2, R> func)
			=> @this.Map(func.Curry());

		public static Result<Func<T2, T3, R>> Map<T1, T2, T3, R>(this Result<T1> @this, Func<T1, T2, T3, R> func)
			=> @this.Map(func.CurryFirst());


		// FOREACH
		public static Result<Unit> ForEach<T>(this Result<T> result, Action<T> action)
			=> Map(result, action.ToFunc());

		public static Result ForEach(this Result result, Action action)
			=> result.Match(
				Success: () => { action(); return Success(); },
				Fail: e => e);

		// BIND
		public static Result<R> Bind<T, R>(this Result<T> result, Func<T, Result<R>> f)
			=> result.Match(
				Success: s => f(s),
				Fail: e => e);

		public static IEnumerable<R> Bind<T, R>(this Result<T> @this, Func<T, IEnumerable<R>> func)
			=> @this.AsEnumerable().Bind(func);

		public static Result Bind<T, R>(this Result result, Func<Result> f)
			=> result.Match(
				Success: () => f(),
				Fail: e => e);
	}



    public static class Result_Utils
    {
		internal static bool IsSuccess<T>(this Result<T> @this)
			=> @this.Match(
				(fail) => false,
				(success) => true);

		internal static T ValueUnsafe<T>(this Result<T> @this)
			=> @this.Match(
				(fail) => { throw new InvalidOperationException(); },
				(t) => t);

		public static T GetOrElse<T>(this Result<T> result, T defaultValue)
			=> result.Match(
				(fail) => defaultValue,
				(t) => t);

		public static T GetOrElse<T>(this Result<T> result, Func<T> fallback)
		   => result.Match(
			  (fail) => fallback(),
			  (t) => t);

		public static Result<T> OrElse<T>(this Result<T> left, Result<T> right)
			=> left.Match(
				(fail) => right,
				(_) => left);

		public static Result<T> OrElse<T>(this Result<T> left, Func<Result<T>> right)
			=> left.Match(
				(fail) => right(),
				(_) => left);
	}



    public static class Result_Applicative
	{
		public static Result<R> Apply<T, R>(this Result<Func<T, R>> @this, Result<T> arg)
			=> @this.Match(
				Fail: (errF) => errF,
				Success: (f) => arg.Match(
					Success: (t) => Success(f(t)),
					Fail: (err) => err));

		public static Result<Func<T2, R>> Apply<T1, T2, R>
		 (this Result<Func<T1, T2, R>> @this, Result<T1> arg)
			=> Apply(@this.Map(TDF.Curry), arg);

		public static Result<Func<T2, T3, R>> Apply<T1, T2, T3, R>
		   (this Result<Func<T1, T2, T3, R>> @this, Result<T1> arg)
		   => Apply(@this.Map(TDF.CurryFirst), arg);

		public static Result<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
		   (this Result<Func<T1, T2, T3, T4, R>> @this, Result<T1> arg)
		   => Apply(@this.Map(TDF.CurryFirst), arg);

		public static Result<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
		   (this Result<Func<T1, T2, T3, T4, T5, R>> @this, Result<T1> arg)
		   => Apply(@this.Map(TDF.CurryFirst), arg);

		public static Result<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
		   (this Result<Func<T1, T2, T3, T4, T5, T6, R>> @this, Result<T1> arg)
		   => Apply(@this.Map(TDF.CurryFirst), arg);

		public static Result<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
		   (this Result<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Result<T1> arg)
		   => Apply(@this.Map(TDF.CurryFirst), arg);

		public static Result<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
		   (this Result<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Result<T1> arg)
		   => Apply(@this.Map(TDF.CurryFirst), arg);

		public static Result<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
		   (this Result<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Result<T1> arg)
		   => Apply(@this.Map(TDF.CurryFirst), arg);
	}



	public static class Result_LINQ
	{
		public static Result<R> Select<T, R>(this Result<T> result, Func<T, R> f)
			=> result.Map(f);

		public static Result<RR> SelectMany<T, R, RR>(this Result<T> result, Func<T, Result<R>> bind, Func<T, R, RR> project)
		   => result.Match(
			   (e) => e,
			   (t) => bind(t).Match(
				   (e) => e,
				   (r) => Success(project(t, r))));

	}
}
