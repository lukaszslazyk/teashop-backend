using System;

namespace Teashop.Backend.Domain.Order.Entities
{
    public class PaymentCard
    {
        public Guid PaymentCardId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
    }
}
