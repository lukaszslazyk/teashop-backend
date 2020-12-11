using FluentValidation;

namespace Teashop.Backend.Application.Order.Queries.GetOrderById
{
    public class GetOrderByOrderNoQueryValidator : AbstractValidator<GetOrderByOrderNo>
    {
        public GetOrderByOrderNoQueryValidator()
        {
            RuleFor(q => q.OrderNo)
                .NotEmpty().WithMessage("Order number is required.");
        }
    }
}
