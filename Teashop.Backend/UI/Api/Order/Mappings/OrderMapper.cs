using Teashop.Backend.Domain.Order.Entities;
using Teashop.Backend.UI.Api.Order.Models;

namespace Teashop.Backend.UI.Api.Order.Mappings
{
    public class OrderMapper
    {
        public PresentationalContactInfo MapToPresentational(ContactInfo contactInfo)
        {
            return new PresentationalContactInfo
            {
                Email = contactInfo.Email
            };
        }

        public ContactInfo MapFromPresentational(PresentationalContactInfo contactInfo)
        {
            return new ContactInfo
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
                CountryCode = address.CountryCode,
                Phone = address.Phone
            };
        }

        public Address MapFromPresentational(PresentationalAddress address)
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

        public PaymentCard MapFromPresentational(PresentationalPaymentCard paymentCard)
        {
            return new PaymentCard
            {
                Number = paymentCard.Number,
                Name = paymentCard.Name,
                ExpirationDate = paymentCard.ExpirationDate,
                SecurityCode = paymentCard.SecurityCode,
            };
        }
    }
}
