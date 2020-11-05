using FluentValidation;

namespace Teashop.Backend.Application.Product.Queries.ProductExistsById
{
    public class ProductExistsByIdQueryValidator : AbstractValidator<ProductExistsByIdQuery>
    {
        public ProductExistsByIdQueryValidator()
        {
            RuleFor(q => q.ProductId)
                .NotEmpty().WithMessage("Product id is required.");
        }
    }
}
