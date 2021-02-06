using FluentValidation;
using System;
using System.Linq;

namespace Teashop.Backend.Application.Product.Queries.GetProductsBySpecification
{
    public class GetProductsBySpecificationQueryValidator : AbstractValidator<GetProductsBySpecificationQuery>
    {
        private readonly string[] _sortOptions = { "priceAsc", "priceDesc", "nameAsc", "nameDesc" };

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

            When(q => q.Specification.OrderByQueried, () =>
            {
                RuleFor(q => q.Specification.OrderBy)
                    .NotEmpty().WithMessage("Order by was queried but is empty.")
                    .Must(BeNameOfExistingSortOption)
                        .WithMessage($"Sort option does not exist. Possible values: {GetSortOptionsText()}");
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

        private bool BeNameOfExistingSortOption(string sortOption)
        {
            return _sortOptions.Contains(sortOption);
        }

        private string GetSortOptionsText()
        {
            return string.Join(", ", _sortOptions);
        }
    }
}
