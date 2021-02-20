using FluentValidation;

namespace Teashop.Backend.Application.Product.Queries.GetProductByProductNumber
{
    public class GetProductByProductNumberQueryValidator : AbstractValidator<GetProductByProductNumberQuery>
    {
        public GetProductByProductNumberQueryValidator()
        {
            RuleFor(q => q.ProductNumber)
                .NotEmpty().WithMessage("Product number is required.");
        }
    }
}
