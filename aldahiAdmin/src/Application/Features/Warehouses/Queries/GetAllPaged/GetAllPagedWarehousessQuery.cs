using MediatR;
using FirstCall.Application.Extensions;
using FirstCall.Application.Features.Warehousess.Queries.GetAll;
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
using FirstCall.Application.Specifications.GeneralSettings;

namespace FirstCall.Application.Features.Warehousess.Queries.GetAllPaged
{
    public class GetAllPagedWarehousessQuery : IRequest<PaginatedResult<GetAllWarehousessResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; }

        public GetAllPagedWarehousessQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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
    internal class GetAllWarehousessQueryHandler : IRequestHandler<GetAllPagedWarehousessQuery, PaginatedResult<GetAllWarehousessResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllWarehousessQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllWarehousessResponse>> Handle(GetAllPagedWarehousessQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Warehouses, GetAllWarehousessResponse>> expression = e => new GetAllWarehousessResponse
            {
                Id = e.Id,
                NameAr=e.NameAr,
                NameEn=e.NameEn
              

            };
            var WarehousesFilterSpec = new WarehousesFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Warehouses>().Entities
                   .Specify(WarehousesFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Warehouses>().Entities
                   .Specify(WarehousesFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
