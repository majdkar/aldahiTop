using MediatR;
using FirstCall.Application.Extensions;
using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Specifications;
using FirstCall.Core.Entities;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Wrapper;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.Brands.Queries.GetAllPaged
{
    public class GetAllPagedBrandsQuery : IRequest<PaginatedResult<GetAllBrandsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; }

        public GetAllPagedBrandsQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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
    internal class GetAllBrandsQueryHandler : IRequestHandler<GetAllPagedBrandsQuery, PaginatedResult<GetAllBrandsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllBrandsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllBrandsResponse>> Handle(GetAllPagedBrandsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Brand, GetAllBrandsResponse>> expression = e => new GetAllBrandsResponse
            {
                Id = e.Id,
                Name = e.Name,

                NameEn = e.NameEn,
                Tax = e.Tax,
                ImageDataURL = e.ImageDataURL,
              

            };
            var BrandFilterSpec = new BrandFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Brand>().Entities
                   .Specify(BrandFilterSpec)
                   .Select(expression)
                  
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Brand>().Entities
                   .Specify(BrandFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
