using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Product.Repositories;

namespace Teashop.Backend.Application.Product.Queries.GetProductsInCategory
{
    public class GetProductsInCategoryQueryValidator : AbstractValidator<GetProductsInCategoryQuery>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsInCategoryQueryValidator(IProductRepository productRepository)
        {
            SetupRules();
            _productRepository = productRepository;
        }

        private void SetupRules()
        {
            RuleFor(q => q.CategoryName)
                .NotEmpty().WithMessage("Category name is required.")
                .MustAsync(BeNameOfExistingCategory).WithMessage("Category with given name does not exist.");

            RuleFor(q => q.pageIndexQueried)
                .Equal(true).When(q => q.pageSizeQueried).WithMessage("Page size was queried but page index is missing.");

            RuleFor(q => q.pageSizeQueried)
                .Equal(true).When(q => q.pageIndexQueried).WithMessage("Page index was queried but page size is missing.");

            When(q => q.pageIndexQueried, () =>
            {
                RuleFor(q => q.pageIndex)
                    .GreaterThanOrEqualTo(0).WithMessage("Page index must be greater than or equal to 0.");
            });

            When(q => q.pageSizeQueried, () =>
            {
                RuleFor(q => q.pageSize)
                    .GreaterThanOrEqualTo(0).WithMessage("Page size must be greater than or equal to 0.");
            });
        }

        private async Task<bool> BeNameOfExistingCategory(string categoryName, CancellationToken cancellationToken)
        {
            return await _productRepository.CategoryExistsByName(categoryName);
        }
    }
}
