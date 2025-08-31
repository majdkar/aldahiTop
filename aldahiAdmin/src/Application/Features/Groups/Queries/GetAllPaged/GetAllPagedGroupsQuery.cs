using MediatR;
using FirstCall.Application.Extensions;
using FirstCall.Application.Features.Groups.Queries.GetAll;
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

namespace FirstCall.Application.Features.Groups.Queries.GetAllPaged
{
    public class GetAllPagedGroupsQuery : IRequest<PaginatedResult<GetAllGroupsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; }

        public GetAllPagedGroupsQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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
    internal class GetAllGroupsQueryHandler : IRequestHandler<GetAllPagedGroupsQuery, PaginatedResult<GetAllGroupsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllGroupsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllGroupsResponse>> Handle(GetAllPagedGroupsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Group, GetAllGroupsResponse>> expression = e => new GetAllGroupsResponse
            {
                Id = e.Id,
                NameAr=e.NameAr,
                NameEn=e.NameEn
              

            };
            var GroupFilterSpec = new GroupFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Group>().Entities
                   .Specify(GroupFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Group>().Entities
                   .Specify(GroupFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
