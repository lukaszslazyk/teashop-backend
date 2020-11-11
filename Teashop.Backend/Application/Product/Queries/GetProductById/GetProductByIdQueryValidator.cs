using FluentValidation;

namespace Teashop.Backend.Application.Product.Queries.GetProductById
{
    public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdQueryValidator()
        {
            RuleFor(q => q.ProductId)
                .NotEmpty().WithMessage("Product id is required.");
        }
    }
}
