using System;

namespace Teashop.Backend.Domain.Product.Entities
{
    public class BrewingInfo
    {
        public Guid BrewingInfoId { get; set; }
        public string WeightInfo { get; set; }
        public string TemperatureInfo { get; set; }
        public string TimeInfo { get; set; }
        public string NumberOfBrewingsInfo { get; set; }
        public Guid ProductId { get; set; }
    }
}
