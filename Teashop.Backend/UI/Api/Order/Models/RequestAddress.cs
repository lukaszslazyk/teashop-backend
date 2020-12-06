namespace Teashop.Backend.UI.Api.Order.Models
{
    public class RequestAddress
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string Phone { get; set; }
    }
}
