using MediatR;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductsInCategory
{
    public class GetProductsInCategoryQueryHandler : IRequestHandler<GetProductsInCategoryQuery, GetProductsInCategoryQueryResult>
    {
        private readonly IProductRepository _productRepository;
        private GetProductsInCategoryQuery _request;
        private IEnumerable<ProductEntity> _products;
        private int _numberOfProductsInCategory;

        public GetProductsInCategoryQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetProductsInCategoryQueryResult> Handle(GetProductsInCategoryQuery request, CancellationToken cancellationToken)
        {
            InitOperation(request);
            await LoadNumberOfProductsInCategory();
            await LoadProductsInCategory();
            SortProducts();

            return PrepareResult();
        }

        private void InitOperation(GetProductsInCategoryQuery request)
        {
            _request = request;
        }

        private async Task LoadNumberOfProductsInCategory()
        {
            _numberOfProductsInCategory = await _productRepository
                .CountProductsInCategory(_request.CategoryName);
        }

        private async Task LoadProductsInCategory()
        {
            if (PaginationQueried())
                await LoadProductsInCategoryWithPagination();
            else
                await LoadAllProductsInCategory();
        }

        private bool PaginationQueried()
        {
            return _request.pageIndexQueried && _request.pageSizeQueried;
        }

        private async Task LoadProductsInCategoryWithPagination()
        {
            _products = await _productRepository
                .GetProductsInCategoryWithPagination(_request.CategoryName, _request.pageIndex, _request.pageSize);
        }

        private async Task LoadAllProductsInCategory()
        {
            _products = await _productRepository
                .GetProductsInCategory(_request.CategoryName);
        }

        private void SortProducts()
        {
            _products = _products
                .OrderBy(CalculateComparedValueForSorting)
                .ThenBy(p => p.Name)
                .ToList();
        }

        private double CalculateComparedValueForSorting(ProductEntity product)
        {
            return product.PricedByWeight()
                ? (product.Price / product.QuantityPerPrice) * 100
                : product.Price;
        }

        private GetProductsInCategoryQueryResult PrepareResult()
        {
            return new GetProductsInCategoryQueryResult
            {
                Products = _products,
                PageIndex = GetPageIndex(),
                PageSize = GetPageSize(),
                PagesInTotal = GetPagesInTotal(),
            };
        }

        private int GetPageIndex()
        {
            return PaginationQueried() ? _request.pageIndex : 0;
        }

        private int GetPageSize()
        {
            if (!PaginationQueried())
                return _numberOfProductsInCategory;

            return _request.pageSize > _numberOfProductsInCategory
                ? _numberOfProductsInCategory
                : _request.pageSize;
        }

        private int GetPagesInTotal()
        {
            if (!PaginationQueried())
                return 1;

            return _numberOfProductsInCategory / _request.pageSize
                    + (_numberOfProductsInCategory % _request.pageSize != 0 ? 1 : 0);
        }
    }
}
