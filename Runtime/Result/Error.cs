using System.Collections.Generic;
using System.Linq;

namespace Tutan.Functional
{
    public static partial class F
    {
        public static Error Error(string message) => new Error(message);
        public static Error Error(IEnumerable<Error> errors) => new Error(errors);
    }

    public record Error(string Message)
    {
        public Error Inner { get; init; }
        public IReadOnlyList<Error> InnerErrors { get; init; } = System.Array.Empty<Error>();

        public Error(IEnumerable<Error> errors)
            : this(string.Join("; ", errors.Select(e => e.Message)))
        {
            InnerErrors = errors.ToArray();
        }

        public static implicit operator Error(string message) => new(message);

        public IEnumerable<Error> AsEnumerable()
            => InnerErrors.Count > 0 ? InnerErrors : new[] { this };

        public override string ToString()
            => InnerErrors.Count > 0
                ? string.Join("; ", InnerErrors.Select(e => e.Message))
                : Message;
    }
}
