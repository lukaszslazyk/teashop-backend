﻿using FluentValidation;

namespace Teashop.Backend.Application.Product.Queries.GetProductsInCategory
{
    public class GetProductsInCategoryQueryValidator : AbstractValidator<GetProductsInCategoryQuery>
    {
        public GetProductsInCategoryQueryValidator()
        {
            SetupRules();
        }

        private void SetupRules()
        {
            RuleFor(q => q.CategoryName)
                .NotEmpty().WithMessage("Category name is required.");

            RuleFor(q => q.PageIndexQueried)
                .Equal(true).When(q => q.PageSizeQueried).WithMessage("Page size was queried but page index is missing.");

            RuleFor(q => q.PageSizeQueried)
                .Equal(true).When(q => q.PageIndexQueried).WithMessage("Page index was queried but page size is missing.");

            When(q => q.PageIndexQueried, () =>
            {
                RuleFor(q => q.PageIndex)
                    .GreaterThanOrEqualTo(0).WithMessage("Page index must be greater than or equal to 0.");
            });

            When(q => q.PageSizeQueried, () =>
            {
                RuleFor(q => q.PageSize)
                    .GreaterThanOrEqualTo(0).WithMessage("Page size must be greater than or equal to 0.");
            });
        }
    }
}
