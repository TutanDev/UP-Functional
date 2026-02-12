using System;
using System.Collections.Generic;
using System.Linq;

namespace Tutan.Functional
{
    public delegate Result<T> Validator<T>(T t);

    public static partial class F
    {
        // runs all validators, and fails when the first one fails
        public static Validator<T> FailFast<T>(IEnumerable<Validator<T>> validators)
            => t => validators.Aggregate(
                Success(t),
                (acc, validator) => acc.Bind(_ => validator(t)));

        public static Validator<T> FailFast<T>(params Validator<T>[] validators)
            => FailFast(validators.AsEnumerable());

        // runs all validators, accumulating all validation errors
        public static Validator<T> HarvestErrors<T>(IEnumerable<Validator<T>> validators)
           => t =>
           {
               var errors = validators
                .Map(validate => validate(t))
                .Bind(v => v.Match(
                   onError: err => Some(err.AsEnumerable()),
                   onSuccess: _ => None))
                .Flatten()
                .ToList();

               return errors.Count == 0
                ? Success(t)
                : Error(errors);
           };

        public static Validator<T> HarvestErrors<T>(params Validator<T>[] validators)
            => HarvestErrors(validators.AsEnumerable());
    }
}
