using MediatR;
using FirstCall.Application.Extensions;
using FirstCall.Application.Features.Kinds.Queries.GetAll;
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

namespace FirstCall.Application.Features.Kinds.Queries.GetAllPaged
{
    public class GetAllPagedKindsQuery : IRequest<PaginatedResult<GetAllKindsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; }

        public GetAllPagedKindsQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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
    internal class GetAllKindsQueryHandler : IRequestHandler<GetAllPagedKindsQuery, PaginatedResult<GetAllKindsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllKindsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllKindsResponse>> Handle(GetAllPagedKindsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Kind, GetAllKindsResponse>> expression = e => new GetAllKindsResponse
            {
                Id = e.Id,
                NameAr=e.NameAr,
                NameEn=e.NameEn
              

            };
            var KindFilterSpec = new KindFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Kind>().Entities
                   .Specify(KindFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Kind>().Entities
                   .Specify(KindFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
