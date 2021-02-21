using FluentValidation;

namespace Teashop.Backend.Application.Product.Queries.GetProductsBySpecification
{
    public class GetProductsBySpecificationQueryValidator : AbstractValidator<GetProductsBySpecificationQuery>
    {
        private readonly ISortOptionNameParser _sortOptionNameParser;

        public GetProductsBySpecificationQueryValidator(ISortOptionNameParser sortOptionNameParser)
        {
            _sortOptionNameParser = sortOptionNameParser;
            SetupRules();
        }

        private void SetupRules()
        {
            When(q => q.CategoryNameQueried, () =>
            {
                RuleFor(q => q.CategoryName)
                    .NotEmpty().WithMessage("Category name was queried but is empty.");
            });

            When(q => q.SearchPhraseQueried, () =>
            {
                RuleFor(q => q.SearchPhrase)
                    .NotEmpty().WithMessage("Search phrase was queried but is empty.")
                    .MaximumLength(32).WithMessage("Maximum search phrase length is 32 characters.");
            });

            When(q => q.OrderByQueried, () =>
            {
                RuleFor(q => q.OrderBy)
                    .NotEmpty().WithMessage("Order by was queried but is empty.")
                    .Must(BeNameOfExistingSortOption)
                        .WithMessage($"Sort option does not exist. Possible values: {GetSortOptionsText()}");
            });

            When(q => q.PageIndexQueried, () =>
            {
                RuleFor(q => q.PageIndex)
                    .GreaterThanOrEqualTo(0).WithMessage("Page index must be greater than or equal to 0.");
                RuleFor(q => q.PageSizeQueried)
                    .Equal(true).WithMessage("Page index was queried but page size is missing.");
            });

            When(q => q.PageSizeQueried, () =>
            {
                RuleFor(q => q.PageSize)
                    .GreaterThanOrEqualTo(0).WithMessage("Page size must be greater than or equal to 0.");
                RuleFor(q => q.PageIndexQueried)
                    .Equal(true).WithMessage("Page size was queried but page index is missing.");
            });
        }

        private bool BeNameOfExistingSortOption(string sortOptionName)
        {
            return _sortOptionNameParser.IsNameOfExistingSortOption(sortOptionName);
        }

        private string GetSortOptionsText()
        {
            return string.Join(", ", _sortOptionNameParser.GetAvailableSortOptionNames());
        }
    }
}
