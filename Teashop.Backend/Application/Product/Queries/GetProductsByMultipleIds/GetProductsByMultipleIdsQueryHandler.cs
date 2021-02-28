using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductsByMultipleIds
{
    public class GetProductsByMultipleIdsQueryHandler : IRequestHandler<GetProductsByMultipleIdsQuery, List<ProductEntity>>
    {
        private readonly IProductRepository _productRepository;
        private List<ProductEntity> _products;
        private GetProductsByMultipleIdsQuery _request;

        public GetProductsByMultipleIdsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductEntity>> Handle(GetProductsByMultipleIdsQuery request, CancellationToken cancellationToken)
        {
            SetupOperation(request);
            await LoadProducts();
            if (AnyProductMissing())
                ThrowNotFoundException();

            return _products;
        }

        private void SetupOperation(GetProductsByMultipleIdsQuery request)
        {
            _request = request;
        }

        private async Task LoadProducts()
        {
            _products = await _productRepository.GetByMultipleIds(_request.ProductIds);
        }
        private bool AnyProductMissing()
        {
            return _products.Count != _request.ProductIds.Count;
        }

        private void ThrowNotFoundException()
        {
            throw new NotFoundException($"Products with the following ids do not exist: {GetMissingProductIdsText()}");
        }

        private string GetMissingProductIdsText()
        {
            return string.Join($", ", GetMissingProductIds());
        }

        private Guid[] GetMissingProductIds()
        {
            var requestedIds = _request.ProductIds.ToArray();
            var foundIds = _products.Select(p => p.ProductId).ToArray();

            return requestedIds.Except(foundIds).ToArray();
        }
    }
}
