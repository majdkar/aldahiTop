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
using FirstCall.Shared.Constants.Products;

namespace FirstCall.Application.Features.Products.Queries.GetAllPaged
{
    public class GetAllPagedSearchAdvancedProductsQuery : IRequest<PaginatedResult<GetAllPagedProductsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        
        public string[] OrderBy { get; set; }

        public string Code { get; set; }
        public string ProductType { get; set; }
        public int SeasonId { get; set; }
        public int KindId { get; set; }
        public int GroupId { get; set; }
        public int WarehousesId { get; set; }
        public int ProductCategoryId { get; set; }

        public int FromQty { get; set; }
        public int ToQty { get; set; }


        public GetAllPagedSearchAdvancedProductsQuery(int pageNumber, int pageSize, string searchString, string orderBy,int seasonId, int kindId, int groupId, int warehousesId,int productCategoryId,string code, int fromqty,int toqty,string productType)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
            SeasonId = seasonId;
            GroupId = groupId;
            WarehousesId = warehousesId;
            ProductType = productType;
            Code = code;
            FromQty = fromqty;
            ToQty = toqty;
            KindId = kindId;
            ProductCategoryId = productCategoryId;
        }
    }

    internal class GetAllPagedSearchAdvancedProductsQueryHandler : IRequestHandler<GetAllPagedSearchAdvancedProductsQuery, PaginatedResult<GetAllPagedProductsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedSearchAdvancedProductsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedProductsResponse>> Handle(GetAllPagedSearchAdvancedProductsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Product, GetAllPagedProductsResponse>> expression = e => new GetAllPagedProductsResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                Price = e.Price,
                Code = e.Code,
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
                 Type = e.Type,
            };
            var productFilterSpec = new ProductSearchAdvancedFilterSpecification(request.SeasonId,request.KindId,request.GroupId,request.WarehousesId,request.ProductCategoryId,request.FromQty,request.ToQty,request.Code,request.ProductType);
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