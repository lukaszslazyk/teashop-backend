using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = Teashop.Backend.Application.Commons.Exceptions.ValidationException;

namespace Teashop.Backend.Application.Commons.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private List<ValidationFailure> _failures;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (AnyValidatorsExist())
            {
                await RunValidations(request, cancellationToken);
                if (AnyFailures())
                    ThrowValidationException();
            }

            return await next();
        }

        private bool AnyValidatorsExist()
        {
            return _validators.Any();
        }

        private async Task RunValidations(TRequest request, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            _failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
        }
        private bool AnyFailures()
        {
            return _failures.Count != 0;
        }

        private void ThrowValidationException()
        {
            throw new ValidationException(_failures);
        }
    }
}
