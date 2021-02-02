using FluentValidation;

namespace Teashop.Backend.Application.Product.Queries.GetProductsBySpecification
{
    public class GetProductsBySpecificationQueryValidator : AbstractValidator<GetProductsBySpecificationQuery>
    {
        public GetProductsBySpecificationQueryValidator()
        {
            SetupRules();
        }

        private void SetupRules()
        {
            When(q => q.Specification != null, () =>
            {
                SetupRulesForSpecification();
            });
        }

        private void SetupRulesForSpecification()
        {
            When(q => q.Specification.CategoryNameQueried, () =>
            {
                RuleFor(q => q.Specification.CategoryName)
                    .NotEmpty().WithMessage("Category name was queried but is empty.");
            });

            When(q => q.Specification.SearchPhraseQueried, () =>
            {
                RuleFor(q => q.Specification.SearchPhrase)
                    .NotEmpty().WithMessage("Search phrase was queried but is empty.");
            });

            When(q => q.Specification.PageIndexQueried, () =>
            {
                RuleFor(q => q.Specification.PageIndex)
                    .GreaterThanOrEqualTo(0).WithMessage("Page index must be greater than or equal to 0.");
                RuleFor(q => q.Specification.PageSizeQueried)
                    .Equal(true).WithMessage("Page index was queried but page size is missing.");
            });

            When(q => q.Specification.PageSizeQueried, () =>
            {
                RuleFor(q => q.Specification.PageSize)
                    .GreaterThanOrEqualTo(0).WithMessage("Page size must be greater than or equal to 0.");
                RuleFor(q => q.Specification.PageIndexQueried)
                    .Equal(true).WithMessage("Page size was queried but page index is missing.");
            });
        }
    }
}
