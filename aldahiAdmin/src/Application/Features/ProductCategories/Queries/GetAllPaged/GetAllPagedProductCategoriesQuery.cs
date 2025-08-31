using MediatR;
using FirstCall.Application.Extensions;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Specifications.Catalog;
using FirstCall.Core.Entities;
using FirstCall.Shared.Wrapper;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.ProductCategories.Queries.GetAllPaged
{
    public class GetAllPagedProductCategoriesQuery : IRequest<PaginatedResult<GetAllPagedProductCategoriesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; }

        public GetAllPagedProductCategoriesQuery(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }
    internal class GetAllProductCategoriesQueryHandler : IRequestHandler<GetAllPagedProductCategoriesQuery, PaginatedResult<GetAllPagedProductCategoriesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllProductCategoriesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedProductCategoriesResponse>> Handle(GetAllPagedProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ProductCategory, GetAllPagedProductCategoriesResponse>> expression = e => new GetAllPagedProductCategoriesResponse
            {
                Id = e.Id,
                NameEn = e.NameEn,
                NameAr = e.NameAr,
                DescriptionEn = e.DescriptionEn,
                DescriptionAr = e.DescriptionAr,
                ParentCategoryId = e.ParentCategoryId,
                Order = e.Order,
                ImageDataURL = e.ImageDataURL,
                Type = e.Type 
            };
            var productCategoryFilterSpec = new ProductCategoryFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<ProductCategory>().Entities
                   .Specify(productCategoryFilterSpec)
                   .OrderBy(x => x.Order)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<ProductCategory>().Entities
                   .Specify(productCategoryFilterSpec)
                   .OrderBy(x => x.Order)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
