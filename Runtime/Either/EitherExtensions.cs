using System;
using Unit = System.ValueTuple;

namespace TD.Functional
{
	using static TDF;

	public static class Either_Monad
	{
		// MAP
		public static Either<L, RR> Map<L, R, RR>(this Either<L, R> @this, Func<R, RR> f)
			=> @this.Match<Either<L, RR>>(
				l => Left(l),
				r => Right(f(r)));

		public static Either<LL, RR> Map<L, LL, R, RR>(this Either<L, R> @this, Func<L, LL> Left, Func<R, RR> Right)
			=> @this.Match<Either<LL, RR>>
				(
					l => TDF.Left(Left(l)),
					r => TDF.Right(Right(r))
				);

		// FOREACH
		public static Either<L, Unit> ForEach<L, R>(this Either<L, R> @this, Action<R> act)
		   => Map(@this, act.ToFunc());

		// BIND
		public static Either<L, RR> Bind<L, R, RR>(this Either<L, R> @this, Func<R, Either<L, RR>> f)
			=> @this.Match(
				l => Left(l),
				r => f(r));
	}

	public static class Either_Applicative
	{
		public static Either<L, RR> Apply<L, R, RR>(this Either<L, Func<R, RR>> @this,Either<L, R> valT)
			=> @this.Match(
				Left: (errF) => Left(errF),
				Right: (f) => valT.Match<Either<L, RR>>(
					Right: (t) => Right(f(t)),
					Left: (err) => Left(err) ));

		public static Either<L, Func<T2, R>> Apply<L, T1, T2, R>
		  (this Either<L, Func<T1, T2, R>> @this, Either<L, T1> arg)
			=> Apply(@this.Map(TDF.Curry), arg);

		public static Either<L, Func<T2, T3, R>> Apply<L, T1, T2, T3, R>
		  (this Either<L, Func<T1, T2, T3, R>> @this, Either<L, T1> arg)
			=> Apply(@this.Map(TDF.CurryFirst), arg);

		public static Either<L, Func<T2, T3, T4, R>> Apply<L, T1, T2, T3, T4, R>
		  (this Either<L, Func<T1, T2, T3, T4, R>> @this, Either<L, T1> arg)
			=> Apply(@this.Map(TDF.CurryFirst), arg);

		public static Either<L, Func<T2, T3, T4, T5, R>> Apply<L, T1, T2, T3, T4, T5, R>
		  (this Either<L, Func<T1, T2, T3, T4, T5, R>> @this, Either<L, T1> arg)
			=> Apply(@this.Map(TDF.CurryFirst), arg);

		public static Either<L, Func<T2, T3, T4, T5, T6, R>> Apply<L, T1, T2, T3, T4, T5, T6, R>
		  (this Either<L, Func<T1, T2, T3, T4, T5, T6, R>> @this, Either<L, T1> arg)
			=> Apply(@this.Map(TDF.CurryFirst), arg);

		public static Either<L, Func<T2, T3, T4, T5, T6, T7, R>> Apply<L, T1, T2, T3, T4, T5, T6, T7, R>
		  (this Either<L, Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Either<L, T1> arg)
			=> Apply(@this.Map(TDF.CurryFirst), arg);

		public static Either<L, Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<L, T1, T2, T3, T4, T5, T6, T7, T8, R>
		  (this Either<L, Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Either<L, T1> arg)
			=> Apply(@this.Map(TDF.CurryFirst), arg);

		public static Either<L, Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<L, T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
		  (this Either<L, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Either<L, T1> arg)
			=> Apply(@this.Map(TDF.CurryFirst), arg);
	}



	public static class Either_LINQ
	{
		public static Either<L, R> Select<L, T, R>(this Either<L, T> @this, Func<T, R> f)
			=> @this.Map(f);

		public static Either<L, RR> SelectMany<L, T, R, RR>(this Either<L, T> @this, Func<T, Either<L, R>> bind, Func<T, R, RR> project)
			=> @this.Match(
				Left: l => Left(l),
				Right: t => bind(t).Match<Either<L, RR>>(
					Left: l => Left(l),
					Right: r => project(t, r)));
	}
}
