using Teashop.Backend.Application.Order.Commands.PlaceOrder;
using Teashop.Backend.Domain.Order.Entities;
using Teashop.Backend.UI.Api.Cart.Mappings;
using Teashop.Backend.UI.Api.Order.Models;

namespace Teashop.Backend.UI.Api.Order.Mappings
{
    public class OrderMapper
    {
        private readonly OrderMetaMapper _orderMetaMapper;
        private readonly CartMapper _cartMapper;

        public OrderMapper(OrderMetaMapper orderMetaMapper, CartMapper cartMapper)
        {
            _orderMetaMapper = orderMetaMapper;
            _cartMapper = cartMapper;
        }

        public PresentationalOrder MapToPresentational(OrderEntity order)
        {
            return new PresentationalOrder
            {
                OrderNo = order.OrderNo,
                PlacementDate = order.CreatedAt,
                ContactInfo = MapToPresentational(order.ContactInfo),
                ShippingAddress = MapToPresentational(order.ShippingAddress),
                BillingAddress = MapToPresentational(order.BillingAddress),
                ChosenShippingMethod = _orderMetaMapper.MapToPresentational(order.ChosenShippingMethod),
                ChosenPaymentMethod = _orderMetaMapper.MapToPresentational(order.ChosenPaymentMethod),
                Cart = _cartMapper.MapToPresentational(order.Cart),
                TotalPrice = order.GetTotalPrice(),
                ShippingPrice = order.GetShippingPrice()
            };
        }

        public PresentationalContactInfo MapToPresentational(ContactInfo contactInfo)
        {
            return new PresentationalContactInfo
            {
                Email = contactInfo.Email
            };
        }

        public PresentationalAddress MapToPresentational(Address address)
        {
            return new PresentationalAddress
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                Company = address.Company,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                PostalCode = address.PostalCode,
                City = address.City,
                Country = address.Country,
                Phone = address.Phone
            };
        }

        public PresentationalPaymentCard MapToPresentational(PaymentCard paymentCard)
        {
            return new PresentationalPaymentCard
            {
                Number = paymentCard.Number,
                Name = paymentCard.Name,
                ExpirationDate = paymentCard.ExpirationDate,
                SecurityCode = paymentCard.SecurityCode,
            };
        }

        public ContactInfo MapFromRequest(RequestContactInfo contactInfo)
        {
            return new ContactInfo
            {
                Email = contactInfo.Email
            };
        }

        public Address MapFromRequest(RequestAddress address)
        {
            return new Address
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                Company = address.Company,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                PostalCode = address.PostalCode,
                City = address.City,
                CountryCode = address.CountryCode,
                Phone = address.Phone
            };
        }

        public PaymentCard MapFromRequest(RequestPaymentCard paymentCard)
        {
            return new PaymentCard
            {
                Number = paymentCard.Number,
                Name = paymentCard.Name,
                ExpirationDate = paymentCard.ExpirationDate,
                SecurityCode = paymentCard.SecurityCode,
            };
        }

        public PlaceOrderResponse MapToResponse(PlaceOrderCommandResult result)
        {
            return new PlaceOrderResponse
            {
                OrderId = result.OrderId,
                OrderNo = result.OrderNo
            };
        }
    }
}
