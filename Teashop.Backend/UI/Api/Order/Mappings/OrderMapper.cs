using System.Linq;
using System.Collections.Generic;
using Teashop.Backend.Application.Order.Commands.PlaceOrder;
using Teashop.Backend.Domain.Order.Entities;
using Teashop.Backend.UI.Api.Order.Models;
using Teashop.Backend.UI.Api.Product.Mappings;

namespace Teashop.Backend.UI.Api.Order.Mappings
{
    public class OrderMapper
    {
        private readonly OrderMetaMapper _orderMetaMapper;
        private readonly ProductMapper _productMapper;

        public OrderMapper(OrderMetaMapper orderMetaMapper, ProductMapper productMapper)
        {
            _orderMetaMapper = orderMetaMapper;
            _productMapper = productMapper;
        }

        public PresentationalOrder MapToPresentational(OrderEntity order)
        {
            return new PresentationalOrder
            {
                OrderNumber = order.OrderNumber,
                PlacementDate = order.CreatedAt,
                ContactInfo = MapToPresentational(order.ContactInfo),
                ShippingAddress = MapToPresentational(order.ShippingAddress),
                BillingAddress = MapToPresentational(order.BillingAddress),
                ChosenShippingMethod = _orderMetaMapper.MapToPresentational(order.ChosenShippingMethod),
                ChosenPaymentMethod = _orderMetaMapper.MapToPresentational(order.ChosenPaymentMethod),
                OrderLines = MapToPresentational(order.OrderLines),
                TotalPrice = order.TotalPrice,
                SubtotalPrice = order.SubtotalPrice,
                ShippingFee = order.ShippingFee,
                PaymentFee = order.PaymentFee
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

        private List<PresentationalOrderLine> MapToPresentational(List<OrderLine> orderLines)
        {
            return orderLines
                .Select(line => new PresentationalOrderLine
                {
                    Product = _productMapper.MapToMinimized(line.Product),
                    Quantity = line.Quantity,
                    Price = line.Price,
                })
                .ToList();
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

        public List<PlaceOrderCommandOrderLine> MapFromRequest(List<RequestOrderLine> orderLines)
        {
            return orderLines?
                .Select(line => new PlaceOrderCommandOrderLine
                {
                    ProductId = line.ProductId,
                    Quantity = line.Quantity
                })
                .ToList();
        }

        public PlaceOrderResponse MapToResponse(PlaceOrderCommandResult result)
        {
            return new PlaceOrderResponse
            {
                OrderId = result.OrderId,
                OrderNumber = result.OrderNumber
            };
        }
    }
}
