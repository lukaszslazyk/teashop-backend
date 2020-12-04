using System;

namespace Teashop.Backend.Domain.Order.Entities
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public Country Country { get; set; }
        public string Phone { get; set; }
    }
}
