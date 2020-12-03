using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Order.Repositories;

namespace Teashop.Backend.Application.Order.Queries.GetOrderMeta
{
    public class GetOrderMetaQueryHandler : IRequestHandler<GetOrderMetaQuery, OrderMeta>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IShippingMethodRepository _shippingMethodRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public GetOrderMetaQueryHandler(
            ICountryRepository countryRepository,
            IShippingMethodRepository shippingMethodRepository,
            IPaymentMethodRepository paymentMethodRepository)
        {
            _countryRepository = countryRepository;
            _shippingMethodRepository = shippingMethodRepository;
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<OrderMeta> Handle(GetOrderMetaQuery request, CancellationToken cancellationToken)
        {
            return new OrderMeta
            {
                Countries = await _countryRepository.GetAll(),
                ShippingMethods = await _shippingMethodRepository.GetAll(),
                PaymentMethods = await _paymentMethodRepository.GetAll(),
            };
        }
    }
}
