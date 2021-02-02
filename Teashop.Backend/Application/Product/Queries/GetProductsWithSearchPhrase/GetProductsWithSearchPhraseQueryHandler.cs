using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Commons.Models;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Application.Product.Services;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductsWithSearchPhrase
{
    public class GetProductsWithSearchPhraseQueryHandler: IRequestHandler<GetProductsWithSearchPhraseQuery, PaginatedList<ProductEntity>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductsSortingService _productsSortingService;
        private GetProductsWithSearchPhraseQuery _request;
        private List<ProductEntity> _products;
        private int _numberOfProductsWithSearchPhrase;

        public GetProductsWithSearchPhraseQueryHandler(
            IProductRepository productRepository,
            IProductsSortingService productsSortingService)
        {
            _productRepository = productRepository;
            _productsSortingService = productsSortingService;
        }

        public async Task<PaginatedList<ProductEntity>> Handle(
            GetProductsWithSearchPhraseQuery request,
            CancellationToken cancellationToken)
        {
            InitOperation(request);
            await LoadNumberOfProductsWithSearchPhrase();
            await LoadProductsWithSearchPhrase();
            SortProducts();

            return PrepareResult();
        }

        private void InitOperation(GetProductsWithSearchPhraseQuery request)
        {
            _request = request;
        }

        private async Task LoadNumberOfProductsWithSearchPhrase()
        {
            _numberOfProductsWithSearchPhrase = await _productRepository
                .CountProductsWithSearchPhrase(_request.Phrase);
        }

        private async Task LoadProductsWithSearchPhrase()
        {
            if (PaginationQueried())
                await LoadProductsWithSearchPhraseWithPagination();
            else
                await LoadAllProductsWithSearchPhrase();
        }

        private bool PaginationQueried()
        {
            return _request.PageIndexQueried && _request.PageSizeQueried;
        }

        private async Task LoadProductsWithSearchPhraseWithPagination()
        {
            _products = await _productRepository
                .GetProductsWithSearchPhraseWithPagination(_request.Phrase, _request.PageIndex, _request.PageSize);
        }

        private async Task LoadAllProductsWithSearchPhrase()
        {
            _products = await _productRepository
                .GetProductsWithSearchPhrase(_request.Phrase);
        }

        private void SortProducts()
        {
            _products = _productsSortingService.SortProductsDefault(_products);
        }

        private PaginatedList<ProductEntity> PrepareResult()
        {
            return PaginationQueried()
                ? new PaginatedList<ProductEntity>(_products, _request.PageIndex, _request.PageSize, _numberOfProductsWithSearchPhrase)
                : new PaginatedList<ProductEntity>(_products);
        }
    }
}
