using FluentValidation;

namespace Teashop.Backend.Application.Cart.Commands.RemoveItemFromCart
{
    public class RemoveItemFromCartCommandValidator : AbstractValidator<RemoveItemFromCartCommand>
    {
        public RemoveItemFromCartCommandValidator()
        {
            RuleFor(c => c.CartId)
                .NotEmpty().WithMessage("Cart id is required.");

            RuleFor(c => c.ProductId)
                .NotEmpty().WithMessage("Product id is required.");
        }
    }
}
