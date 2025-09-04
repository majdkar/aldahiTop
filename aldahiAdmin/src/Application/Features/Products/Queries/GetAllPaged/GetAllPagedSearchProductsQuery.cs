using MediatR;
using FirstCall.Application.Extensions;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Specifications.Catalog;
using FirstCall.Application.Specifications.Products;

using FirstCall.Domain.Entities.Products;
using FirstCall.Shared.Wrapper;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Products.Queries.GetAllPaged
{
    public class GetAllPagedSearchProductsQuery : IRequest<PaginatedResult<GetAllPagedProductsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        
        public string[] OrderBy { get; set; }

        public string ProductName { get; set; }

        public decimal FromPrice { get; set; }

        public decimal ToPrice { get; set; }


        public GetAllPagedSearchProductsQuery(int pageNumber, int pageSize, string searchString, string orderBy,string productname,decimal fromprice,decimal toprice)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
            ProductName = productname;
            FromPrice = fromprice;
            ToPrice = toprice;
        }
    }

    internal class GetAllPagedSearchProductsQueryHandler : IRequestHandler<GetAllPagedSearchProductsQuery, PaginatedResult<GetAllPagedProductsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedSearchProductsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedProductsResponse>> Handle(GetAllPagedSearchProductsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Product, GetAllPagedProductsResponse>> expression = e => new GetAllPagedProductsResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                NameEn = e.NameEn,
                Price = e.Price,
                Code = e.Code,

                Order = e.Order,

                ProductImageUrl = e.ProductImageUrl,
                ProductImageUrl2 = e.ProductImageUrl2,
                ProductImageUrl3 = e.ProductImageUrl3,
                ProductImageUrl4 = e.ProductImageUrl4,
                ProductCategoryId = e.ProductCategoryId,
                PackageNumber = e.PackageNumber,
                Warehouses = e.Warehouses,
                WarehousesId = e.WarehousesId,
                Qty = e.Qty,
                Kind = e.Kind,
                ProductCategory = e.ProductCategory,
                Sizes = e.Sizes,
                Colors = e.Colors,
                KindId = e.KindId,
                SeasonId = e.SeasonId,
                Season = e.Season,
                GroupId = e.GroupId,
                Group = e.Group,
                KindNameAr = e.Kind.NameAr,
                KindNameEn = e.Kind.NameEn,
            };
            var productFilterSpec = new ProductSearchFilterSpecification(request.SearchString,request.ProductName,request.FromPrice,request.ToPrice);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Product>().Entities
                   .Specify(productFilterSpec).OrderBy(x => x.Order)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Product>().Entities
                   .Specify(productFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}