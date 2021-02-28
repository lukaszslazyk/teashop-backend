using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Teashop.Backend.Application.Product.Queries.GetProductsByMultipleIds
{
    public class GetProductsByMultipleIdsQueryValidator : AbstractValidator<GetProductsByMultipleIdsQuery>
    {
        public GetProductsByMultipleIdsQueryValidator()
        {
            RuleFor(q => q.ProductIds)
                .NotEmpty().WithMessage("Product ids are required.")
                .Must(BeDistinct).WithMessage("Product ids must not contain duplicates.");
        }

        private bool BeDistinct(List<Guid> productIds)
        {
            return productIds.Distinct().Count() == productIds.Count;
        }
    }
}
