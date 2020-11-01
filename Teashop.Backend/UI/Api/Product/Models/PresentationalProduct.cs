﻿using System;

namespace Teashop.Backend.UI.Api.Product.Models
{
    public class PresentationalProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int QuantityPerPrice { get; set; }
    }
}
