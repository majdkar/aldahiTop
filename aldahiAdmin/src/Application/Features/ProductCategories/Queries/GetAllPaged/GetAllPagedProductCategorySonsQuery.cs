using MediatR;
using FirstCall.Application.Extensions;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Specifications.GeneralSettings;
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
    public class GetAllPagedProductCategorySonsQuery : IRequest<PaginatedResult<GetAllPagedProductCategoriesResponse>>
    {
        public string Type { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; }
        public int CategoryId { get; set; }

        public GetAllPagedProductCategorySonsQuery(string type,int categoryId,int pageNumber, int pageSize, string searchString, string orderBy)
        {
            Type = type;
            CategoryId = categoryId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }
    internal class GetAllProductCategorySonsQueryHandler : IRequestHandler<GetAllPagedProductCategorySonsQuery, PaginatedResult<GetAllPagedProductCategoriesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllProductCategorySonsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedProductCategoriesResponse>> Handle(GetAllPagedProductCategorySonsQuery request, CancellationToken cancellationToken)
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
                //IconDataURL = e.IconDataURL,
                SonsCount = _unitOfWork.Query<ProductCategory>().Where(x => x.ParentCategoryId == e.Id).Count(),
                ParentCategory = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x=>x.Id==e.ParentCategoryId),
            };
            var productCategoryFilterSpec = new ProductCategorySonsFilterSpecification(request.Type,request.SearchString, request.CategoryId);
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
