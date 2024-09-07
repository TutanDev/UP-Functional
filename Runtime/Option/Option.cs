
using System;
using System.Collections.Generic;

using Unit = System.ValueTuple;


namespace TD.Functional
{
	using static TDF;

	public static partial class TDF
	{
		public static NoneType None => default;

		public static Option<T> Some<T>(T value) 
			=> new Option<T>(value ?? throw new ArgumentNullException(nameof(value)));
	}

	public struct NoneType { }

	public struct Option<T> : IEquatable<NoneType>, IEquatable<Option<T>>
	{
		readonly T _value;
		readonly bool _isSome;
		bool _isNone => !_isSome;

		internal Option(T value) => (_isSome, _value) = (true, value);

		public R Match<R>(Func<R> None, Func<T, R> Some)
			=> _isSome ? Some(_value!) : None();

		public Unit Match(Action None, Action<T> Some)
			=> Match(None.ToFunc(), Some.ToFunc());

		public IEnumerable<T> AsEnumerable()
		{
			if (_isSome) yield return _value!;
		}

		public static implicit operator Option<T>(NoneType _) => default;

		public static implicit operator Option<T>(T value)
			=> value is null ? None : Some(value);

		public static bool operator true(Option<T> @this) => @this._isSome;
		public static bool operator false(Option<T> @this) => !@this._isSome;
		public static Option<T> operator |(Option<T> l, Option<T> r) => l._isSome ? l : r;

		#region Equality operators
		public bool Equals(Option<T> other)
				=> this._isSome == other._isSome
				&& (this._isNone || this._value!.Equals(other._value));

		public bool Equals(NoneType _) => _isNone;

		public static bool operator ==(Option<T> @this, Option<T> other) => @this.Equals(other);
		public static bool operator !=(Option<T> @this, Option<T> other) => !(@this == other);

		public override bool Equals(object other)
		   => other is Option<T> option && this.Equals(option);

		public override int GetHashCode()
		   => _isNone ? 0 : _value!.GetHashCode();

		#endregion Equality operators

		public override string ToString() => _isSome ? $"Some({_value})" : "None";
	}
}
