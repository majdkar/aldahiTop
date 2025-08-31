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
    public class GetAllPagedProductComponentsByProductIdQuery : IRequest<PaginatedResult<GetAllPagedProductComponentsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int ProductId { get; set; }
        public string SearchString { get; set; }
        
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllPagedProductComponentsByProductIdQuery(int pageNumber, int pageSize, string searchString, string orderBy,int productId)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
            ProductId = productId;
           
        }
    }

    internal class GetAllProductComponentsByProductIdQueryHandler : IRequestHandler<GetAllPagedProductComponentsByProductIdQuery, PaginatedResult<GetAllPagedProductComponentsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllProductComponentsByProductIdQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedProductComponentsResponse>> Handle(GetAllPagedProductComponentsByProductIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ProductCom, GetAllPagedProductComponentsResponse>> expression = e => new GetAllPagedProductComponentsResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                NameEn = e.NameEn,
                DescriptionAboutEn = e.DescriptionAboutEn,

                ProductId = e.ProductId,
                ProductComponentImageUrl = e.ProductComponentImageUrl,
                Product = e.Product,
                   ProductComponentImageUrl1 = e.ProductComponentImageUrl1,
                ProductComponentImageUrl2 = e.ProductComponentImageUrl2,
                ProductComponentImageUrl3 = e.ProductComponentImageUrl3,
                ProductComponentImageUrl4 = e.ProductComponentImageUrl4,
                ProductComponentImageUrl5 = e.ProductComponentImageUrl5,
                Order = e.Order

            };
            var productFilterSpec = new ProductComponentByCompanyFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<ProductCom>().Entities
                    .Where(x => x.ProductId == request.ProductId)
                   .Specify(productFilterSpec).OrderBy(x=> x.Order)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<ProductCom>().Entities
                                        .Where(x => x.ProductId == request.ProductId)

                   .Specify(productFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}