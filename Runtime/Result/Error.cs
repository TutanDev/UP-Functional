
namespace TD.Functional
{
	public static partial class TDF
	{
		public static Error Error(string message) => new Error(message);
		public static Error Error(Error error) => error;
	}

	public record Error(string Message)
	{
		public override string ToString() => Message;

		public static implicit operator Error(string m) => new(m);
	}
}
