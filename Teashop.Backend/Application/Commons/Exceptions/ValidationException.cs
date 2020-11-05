using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Teashop.Backend.Application.Commons.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            var failureGroups = failures
                .GroupBy(f => f.PropertyName, f => f.ErrorMessage);
            foreach (var failureGroup in failureGroups)
                Errors.Add(failureGroup.Key, failureGroup.ToArray());
        }
    }
}
