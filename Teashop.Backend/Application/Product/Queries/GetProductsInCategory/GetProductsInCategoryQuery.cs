﻿using MediatR;
using Teashop.Backend.Application.Commons.Models;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductsInCategory
{
    public class GetProductsInCategoryQuery : IRequest<PaginatedList<ProductEntity>>
    {
        public string CategoryName { get; set; }
        public bool PageIndexQueried { get; set; }
        public int PageIndex { get; set; }
        public bool PageSizeQueried { get; set; }
        public int PageSize { get; set; }
    }
}
