using System;
using static TD.Functional.Validation;
using System.Collections.Generic;
using Unit = System.ValueTuple;


namespace TD.Functional
{
	public static partial class TDF
	{
		public static Exceptional<T> Exceptional<T>(T value) => new (value);
	}

	public struct Exceptional<T>
	{
		private Exception _exception { get; }
		private T _value { get; }

		private bool _isSuccess { get; }
		private bool _isException => !_isSuccess;

		internal Exceptional(Exception ex)
		{
			_isSuccess = false;
			_exception = ex ?? throw new ArgumentNullException(nameof(ex));
			_value = default;
		}

		internal Exceptional(T value)
		{
			_isSuccess = true;
			_value = value ?? throw new ArgumentNullException(nameof(value));
			_exception = default;
		}

		public static implicit operator Exceptional<T>(Exception ex) => new(ex);
		public static implicit operator Exceptional<T>(T t) => new(t);

		public TR Match<TR>(Func<Exception, TR> Exception, Func<T, TR> Success)
		   => _isException ? Exception(_exception!) : Success(_value!);

		public Unit Match(Action<Exception> Exception, Action<T> Success)
		   => Match(Exception.ToFunc(), Success.ToFunc());

		public IEnumerator<T> AsEnumerable()
		{
			if (_isSuccess) yield return _value!;
		}

		public override string ToString() => Match(ex => $"Exception({ex.Message})", t => $"Success({t})");
	}
	public static class Exceptional
	{
		// CREATION
		public static Func<T, Exceptional<T>> Return<T>() => t => t;
		public static Exceptional<R> Of<R>(Exception left) => new Exceptional<R>(left);
		public static Exceptional<R> Of<R>(R right) => new Exceptional<R>(right);

		
		// MAP
		public static Exceptional<RR> Map<R, RR>(this Exceptional<R> @this, Func<R, RR> f)
			=> @this.Match(
				Exception: ex => new Exceptional<RR>(ex),
				Success: r => f(r));

		// FOREACH
		public static Exceptional<Unit> ForEach<R>(this Exceptional<R> @this, Action<R> act)
		   => Map(@this, act.ToFunc());

		// BIND
		public static Exceptional<RR> Bind<R, RR>(this Exceptional<R> @this, Func<R, Exceptional<RR>> f)
			=> @this.Match(
				Exception: ex => new Exceptional<RR>(ex),
				Success: r => f(r));

		#region LINQ query pattern
		public static Exceptional<R> Select<T, R>(this Exceptional<T> @this, Func<T, R> map) 
			=> @this.Map(map);

		public static Exceptional<RR> SelectMany<T, R, RR>(this Exceptional<T> @this, Func<T, Exceptional<R>> bind, Func<T, R, RR> project)
			=> @this.Match(
				Exception: ex => new Exceptional<RR>(ex),
				Success: t => bind(t).Match(
					Exception: ex => new Exceptional<RR>(ex),
					Success: r => project(t, r)));
		#endregion LINQ query pattern

	}
}
