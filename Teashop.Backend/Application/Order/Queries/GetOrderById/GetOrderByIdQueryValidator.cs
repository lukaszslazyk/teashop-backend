using FluentValidation;

namespace Teashop.Backend.Application.Order.Queries.GetOrderById
{
    public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
    {
        public GetOrderByIdQueryValidator()
        {
            RuleFor(q => q.OrderId)
                .NotEmpty().WithMessage("Order id is required.");
        }
    }
}
