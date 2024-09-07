using Unit = System.ValueTuple;

namespace TD.Functional
{
	using System;
	using System.Collections.Generic;
	using static TDF;

	public static partial class TDF
	{
		public static Result Success() => new Result(Unit());

		public static Result<T> Success<T>(T value)
			=> new Result<T>(value ?? throw new ArgumentNullException(nameof(value)));
	}

	public struct Result<T> : IEquatable<Result<T>>
	{
		readonly T _value;
		readonly Error _error;

		readonly bool _isSucces;
		bool _isFail => !_isSucces;

		internal Result(T data) 
			=> (_isSucces, _value, _error) 
			= (true, data ?? throw new ArgumentNullException(nameof(data)), default);
		
		internal Result(Error error) 
			=> (_isSucces, _value, _error) 
			= (false, default, error ?? throw new ArgumentNullException(nameof(error)));

		public R Match<R>(Func<Error, R> Fail, Func<T, R> Success)
			=> _isSucces ? Success(_value!) : Fail(_error);

		public Unit Match(Action<Error> Fail, Action<T> Success)
			=> Match(Fail.ToFunc(), Success.ToFunc());

		public IEnumerable<T> AsEnumerable()
		{
			if (_isSucces) yield return _value!;
		}

		public static implicit operator Result<T>(Error error) => new Result<T>(error);
		public static implicit operator Result<T>(T data) => new Result<T>(data);

		public static bool operator true(Result<T> @this) => @this._isSucces;
		public static bool operator false(Result<T> @this) => !@this._isSucces;
		public static Result<T> operator |(Result<T> one, Result<T> two) => one._isSucces ? one : two;

		#region Equality operators
		public bool Equals(Result<T> other)
		=> this._isSucces == other._isSucces
			&& (this._error.Equals(other._error) || this._value!.Equals(other._value));

		public bool Equals(Result other)  => this._isFail && this._error.Equals(other._error);

		public static bool operator ==(Result<T> @this, Result<T> other) => @this.Equals(other);
		public static bool operator !=(Result<T> @this, Result<T> other) => !(@this == other);

		public override bool Equals(object other)
			=> other is Result<T> result && this.Equals(result);

		public override int GetHashCode()
		   => _isFail ? _error.GetHashCode() : _value!.GetHashCode();

		#endregion Equality operators

		public override string ToString() => Match(e => $"Fail({e})", d => $"Success({d})");
	}

	public struct Result : IEquatable<Result>
	{
		internal readonly Error _error;

		readonly bool _isSucces;
		internal bool _isFail => !_isSucces;

		internal Result(Unit _) =>
			(_isSucces, _error) = (true, default);

		internal Result(Error error)
			=> (_isSucces, _error)
			= (false, error ?? throw new ArgumentNullException(nameof(error)));

		public R Match<R>(Func<Error, R> Fail, Func<R> Success)
			=> _isSucces ? Success() : Fail(_error);

		public Unit Match(Action<Error> Fail, Action Success)
			=> Match(Fail.ToFunc(), Success.ToFunc());

		public static implicit operator Result(Error error) => new Result(error);

		public static bool operator true(Result @this) => @this._isSucces;
		public static bool operator false(Result @this) => !@this._isSucces;

		#region Equality operators
		public bool Equals(Result other)
			=> this._isSucces == other._isSucces
			|| (this._isFail && this._error.Equals(other._error));

		public static bool operator ==(Result @this, Result other) => @this.Equals(other);
		public static bool operator !=(Result @this, Result other) => !(@this == other);

		public override bool Equals(object other)
			=> other is Result result && this.Equals(result);

		public override int GetHashCode() => _error.GetHashCode();

		#endregion Equality operators

		public override string ToString() => Match(e => $"Fail({e})", () => $"Success!");
	}
}
