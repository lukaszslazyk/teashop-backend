using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Application.Commons.Models;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductsBySpecification
{
    public class GetProductsBySpecificationQueryHandler : IRequestHandler<GetProductsBySpecificationQuery, PaginatedList<ProductEntity>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ISortOptionNameParser _sortOptionNameParser;
        private ProductsQuerySpecification _specification;
        private List<ProductEntity> _products;

        public GetProductsBySpecificationQueryHandler(IProductRepository productRepository, ISortOptionNameParser sortOptionNameParser)
        {
            _productRepository = productRepository;
            _sortOptionNameParser = sortOptionNameParser;
        }

        public async Task<PaginatedList<ProductEntity>> Handle(GetProductsBySpecificationQuery request, CancellationToken cancellationToken)
        {
            InitOperation(request);
            if (CategoryNameQueried())
                await CheckIfCategoryExist();
            await LoadProducts();

            return await PrepareResult();
        }

        private void InitOperation(GetProductsBySpecificationQuery request)
        {
            _specification = new ProductsQuerySpecification
            {
                CategoryNameQueried = request.CategoryNameQueried,
                CategoryName = request.CategoryName,
                SearchPhraseQueried = request.SearchPhraseQueried,
                SearchPhrase = request.SearchPhrase,
                PageIndexQueried = request.PageIndexQueried,
                PageIndex = request.PageIndex,
                PageSizeQueried = request.PageSizeQueried,
                PageSize = request.PageSize,
                SortOption = GetSortOptionFrom(request),
            };
        }

        private SortOption GetSortOptionFrom(GetProductsBySpecificationQuery request)
        {
            return request.OrderByQueried
                ? _sortOptionNameParser.GetSortOptionFor(request.OrderBy)
                : SortOption.Default;
        }

        private bool CategoryNameQueried()
        {
            return _specification.CategoryNameQueried;
        }

        private async Task CheckIfCategoryExist()
        {
            if (!await CategoryExists())
                throw new NotFoundException("Category with given name does not exist.");
        }

        private async Task<bool> CategoryExists()
        {
            return await _productRepository.CategoryExistsByName(_specification.CategoryName);
        }

        private async Task LoadProducts()
        {
            _products = await _productRepository.GetProductsBySpecification(_specification);
        }

        private async Task<PaginatedList<ProductEntity>> PrepareResult()
        {
            return PaginationQueried()
                ? PrepareResultWithPaginatedProducts(await GetProductsTotalCount())
                : PrepareResultWithProducts();
        }
        private bool PaginationQueried()
        {
            return _specification.PageIndexQueried && _specification.PageSizeQueried;
        }

        private async Task<int> GetProductsTotalCount()
        {
            return await _productRepository.CountProductsBySpecification(_specification);
        }

        private PaginatedList<ProductEntity> PrepareResultWithPaginatedProducts(int totalCount)
        {
            return new PaginatedList<ProductEntity>(
                _products,
                _specification.PageIndex,
                _specification.PageSize,
                totalCount);
        }

        private PaginatedList<ProductEntity> PrepareResultWithProducts()
        {
            return new PaginatedList<ProductEntity>(_products);
        }
    }
}
