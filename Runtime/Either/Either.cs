using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;

namespace TD.Functional
{
	using static TDF;

	public static partial class TDF
	{
		public static Either.Left<L> Left<L>(L l) => new Either.Left<L>(l);
		public static Either.Right<R> Right<R>(R r) => new Either.Right<R>(r);
	}

	public struct Either<L, R>
	{
		readonly L _left { get; }
		readonly R _right { get; }

		readonly bool _isRight { get; }
		readonly bool _isLeft => !_isRight;

		internal Either(L left)
			=> (_isRight, _left, _right)
			= (false, left ?? throw new ArgumentNullException(nameof(left)), default);

		internal Either(R right)
			=> (_isRight, _left, _right)
			= (true, default, right ?? throw new ArgumentNullException(nameof(right)));

		public TR Match<TR>(Func<L, TR> Left, Func<R, TR> Right)
			=> _isLeft ? Left(this._left!) : Right(this._right!);

		public Unit Match(Action<L> Left, Action<R> Right)
			=> Match(Left.ToFunc(), Right.ToFunc());

		public IEnumerator<R> AsEnumerable()
		{
			if (_isRight) yield return _right!;
		}

		public static implicit operator Either<L, R>(L left) => new Either<L, R>(left);
		public static implicit operator Either<L, R>(R right) => new Either<L, R>(right);

		public static implicit operator Either<L, R>(Either.Left<L> left) => new Either<L, R>(left.Value);
		public static implicit operator Either<L, R>(Either.Right<R> right) => new Either<L, R>(right.Value);

		public static bool operator true(Either<L, R> @this) => @this._isRight;
		public static bool operator false(Either<L, R> @this) => !@this._isRight;
		public static Either<L, R> operator |(Either<L, R> one, Either<L, R> two) => one._isRight ? one : two;

		public override string ToString() => Match(l => $"Left({l})", r => $"Right({r})");
	}

	public static class Either
	{
		public struct Left<L>
		{
			internal L Value { get; }
			internal Left(L value) { Value = value; }

			public override string ToString() => $"Left({Value})";
		}

		public struct Right<R>
		{
			internal R Value { get; }
			internal Right(R value) { Value = value; }

			public override string ToString() => $"Right({Value})";

			public Right<RR> Map<L, RR>(Func<R, RR> f) => Right(f(Value));
			public Either<L, RR> Bind<L, RR>(Func<R, Either<L, RR>> f) => f(Value);
		}
	}
}
