using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Product.Queries.ProductExistsById;

namespace Teashop.Backend.Application.Cart.Commands.AddItemToCart
{
    public class AddTiemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
    {
        private readonly IMediator _mediator;

        public AddTiemToCartCommandValidator(IMediator mediator)
        {
            _mediator = mediator;
            SetupRules();
        }

        private void SetupRules()
        {
            RuleFor(c => c.CartId)
                .NotEmpty().WithMessage("Cart id is required.");

            RuleFor(c => c.ProductId)
                .NotEmpty().WithMessage("Product id is required.")
                .MustAsync(BeIdOfExistingProduct).WithMessage("Product with given id does not exist.");

            RuleFor(c => c.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }

        private async Task<bool> BeIdOfExistingProduct(Guid productId, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new ProductExistsByIdQuery { ProductId = productId });
        }
    }
}
