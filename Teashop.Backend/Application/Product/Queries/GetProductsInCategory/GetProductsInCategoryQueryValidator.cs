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
        }

        private async Task<bool> BeNameOfExistingCategory(string categoryName, CancellationToken cancellationToken)
        {
            return await _productRepository.CategoryExistsByName(categoryName);
        }
    }
}
