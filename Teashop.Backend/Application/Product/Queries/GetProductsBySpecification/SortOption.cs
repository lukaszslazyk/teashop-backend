using System.Collections.Generic;
using System.Linq;

namespace Teashop.Backend.Application.Product.Queries.GetProductsBySpecification
{
    public enum SortOption
    {
        PriceAsc,
        PriceDesc,
        NameAsc,
        NameDesc,
        Default,
    }

    public interface ISortOptionNameParser
    {
        string[] GetAvailableSortOptionNames();
        bool IsNameOfExistingSortOption(string sortOptionName);
        SortOption GetSortOptionFor(string sortOptionName);
    }

    public class SortOptionNameParser : ISortOptionNameParser
    {
        private readonly IDictionary<string, SortOption> _sortOptionNamesToSortOptions = new Dictionary<string, SortOption>
        {
            { "priceAsc", SortOption.PriceAsc },
            { "priceDesc", SortOption.PriceDesc },
            { "nameAsc", SortOption.NameAsc },
            { "nameDesc", SortOption.NameDesc },
        };

        public string[] GetAvailableSortOptionNames()
        {
            return _sortOptionNamesToSortOptions.Keys.ToArray();
        }

        public bool IsNameOfExistingSortOption(string sortOptionName)
        {
            return _sortOptionNamesToSortOptions.ContainsKey(sortOptionName);
        }

        public SortOption GetSortOptionFor(string sortOptionName)
        {
            return IsNameOfExistingSortOption(sortOptionName)
                ? _sortOptionNamesToSortOptions[sortOptionName]
                : SortOption.Default;
        }
    }
}
