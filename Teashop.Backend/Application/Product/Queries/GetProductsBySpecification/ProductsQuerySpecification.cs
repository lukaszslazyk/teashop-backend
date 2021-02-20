namespace Teashop.Backend.Application.Product.Queries.GetProductsBySpecification
{
    public class ProductsQuerySpecification
    {
        public bool CategoryNameQueried { get; set; }
        public string CategoryName { get; set; }
        public bool SearchPhraseQueried { get; set; }
        public string SearchPhrase { get; set; }
        public bool PageIndexQueried { get; set; }
        public int PageIndex { get; set; }
        public bool PageSizeQueried { get; set; }
        public int PageSize { get; set; }
        public SortOption SortOption { get; set; }
    }
}
