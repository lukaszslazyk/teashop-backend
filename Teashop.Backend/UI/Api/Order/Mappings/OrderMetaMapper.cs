using System.Collections.Generic;
using System.Linq;
using Teashop.Backend.Application.Order.Queries.GetOrderMeta;
using Teashop.Backend.Domain.Order.Entities;
using Teashop.Backend.UI.Api.Order.Models;

namespace Teashop.Backend.UI.Api.Order.Mappings
{
    public class OrderMetaMapper
    {
        public PresentationalOrderMeta MapToPresentational(OrderMeta orderMeta)
        {
            return new PresentationalOrderMeta
            {
                Countries = MapToMultiplePresentationals(orderMeta.Countries),
                ShippingMethods = MapToMultiplePresentationals(orderMeta.ShippingMethods),
                PaymentMethods = MapToMultiplePresentationals(orderMeta.PaymentMethods),
            };
        }

        public PresentationalCountry[] MapToMultiplePresentationals(IEnumerable<Country> countries)
        {
            return countries
                .Select(country => MapToPresentational(country))
                .ToArray();
        }

        public PresentationalCountry MapToPresentational(Country country)
        {
            return new PresentationalCountry
            {
                Code = country.Code,
                Name = country.Name,
            };
        }

        public PresentationalShippingMethod[] MapToMultiplePresentationals(
            IEnumerable<ShippingMethod> shippingMethods)
        {
            return shippingMethods
                .Select(shippingMethod => MapToPresentational(shippingMethod))
                .ToArray();
        }

        public PresentationalShippingMethod MapToPresentational(ShippingMethod shippingMethod)
        {
            return new PresentationalShippingMethod
            {
                Name = shippingMethod.Name,
                DisplayName = shippingMethod.DisplayName,
                Fee = shippingMethod.Fee,
            };
        }

        public PresentationalPaymentMethod[] MapToMultiplePresentationals(
            IEnumerable<PaymentMethod> paymentMethods)
        {
            return paymentMethods
                .Select(paymentMethod => MapToPresentational(paymentMethod))
                .ToArray();
        }

        public PresentationalPaymentMethod MapToPresentational(PaymentMethod paymentMethod)
        {
            return new PresentationalPaymentMethod
            {
                Name = paymentMethod.Name,
                DisplayName = paymentMethod.DisplayName,
                Fee = paymentMethod.Fee,
            };
        }
    }
}
