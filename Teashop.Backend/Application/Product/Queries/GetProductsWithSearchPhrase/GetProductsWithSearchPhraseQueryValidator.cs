using FluentValidation;

namespace Teashop.Backend.Application.Product.Queries.GetProductsWithSearchPhrase
{
    public class GetProductsWithSearchPhraseQueryValidator : AbstractValidator<GetProductsWithSearchPhraseQuery>
    {
        public GetProductsWithSearchPhraseQueryValidator()
        {
            SetupRules();
        }

        private void SetupRules()
        {
            RuleFor(q => q.Phrase)
                .NotNull().WithMessage("Phrase is required.");

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
