using System.Collections.Generic;

namespace Teashop.Backend.UI.Api.Product.Models
{
    public class PresentationalProductsPagedResponse
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PagesInTotal { get; set; }
        public IEnumerable<PresentationalProduct> Products { get; set; }
    }
}
