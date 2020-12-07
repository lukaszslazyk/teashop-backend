using FluentValidation;
using Teashop.Backend.Application.Cart.Queries.GetCartById;

namespace Teashop.Backend.Application.Cart.Queries.CartExistsById
{
    public class CartExistsByIdQueryValidator : AbstractValidator<GetCartByIdQuery>
    {
        public CartExistsByIdQueryValidator()
        {
            RuleFor(q => q.CartId)
                .NotEmpty().WithMessage("Card id is required.");
        }
    }
}
