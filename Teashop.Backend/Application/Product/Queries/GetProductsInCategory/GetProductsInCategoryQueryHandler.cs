using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Domain.Product.Entities;
using NotFoundException = Teashop.Backend.Application.Commons.Exceptions.NotFoundException;
using Teashop.Backend.Application.Product.Services;
using Teashop.Backend.Application.Commons.Models;

namespace Teashop.Backend.Application.Product.Queries.GetProductsInCategory
{
    public class GetProductsInCategoryQueryHandler : IRequestHandler<GetProductsInCategoryQuery, PaginatedList<ProductEntity>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductsSortingService _productsSortingService;
        private GetProductsInCategoryQuery _request;
        private List<ProductEntity> _products;
        private int _numberOfProductsInCategory;

        public GetProductsInCategoryQueryHandler(
            IProductRepository productRepository,
            IProductsSortingService productsSortingService)
        {
            _productRepository = productRepository;
            _productsSortingService = productsSortingService;
        }

        public async Task<PaginatedList<ProductEntity>> Handle(GetProductsInCategoryQuery request, CancellationToken cancellationToken)
        {
            InitOperation(request);
            if (!await CategoryExist())
                ThrowNotFoundException();
            await LoadNumberOfProductsInCategory();
            await LoadProductsInCategory();
            SortProducts();

            return PrepareResult();
        }

        private void InitOperation(GetProductsInCategoryQuery request)
        {
            _request = request;
        }

        private async Task<bool> CategoryExist()
        {
            return await _productRepository.CategoryExistsByName(_request.CategoryName);
        }

        private void ThrowNotFoundException()
        {
            throw new NotFoundException("Category with given name does not exist.");
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
            return _request.PageIndexQueried && _request.PageSizeQueried;
        }

        private async Task LoadProductsInCategoryWithPagination()
        {
            _products = await _productRepository
                .GetProductsInCategoryWithPagination(_request.CategoryName, _request.PageIndex, _request.PageSize);
        }

        private async Task LoadAllProductsInCategory()
        {
            _products = await _productRepository
                .GetProductsInCategory(_request.CategoryName);
        }

        private void SortProducts()
        {
            _products = _productsSortingService.SortProductsDefault(_products);
        }

        private PaginatedList<ProductEntity> PrepareResult()
        {
            return PaginationQueried()
                ? new PaginatedList<ProductEntity>(_products, _request.PageIndex, _request.PageSize, _numberOfProductsInCategory)
                : new PaginatedList<ProductEntity>(_products);
        }
    }
}
