using FluentValidation;

namespace Teashop.Backend.Application.Cart.Commands.UpdateItemQuantity
{
    public class UpdateItemQuantityCommandValidator : AbstractValidator<UpdateItemQuantityCommand>
    {
        public UpdateItemQuantityCommandValidator()
        {
            RuleFor(c => c.CartId)
                .NotEmpty().WithMessage("Cart id is required.");

            RuleFor(c => c.ProductId)
                .NotEmpty().WithMessage("Product id is required.");

            RuleFor(c => c.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
