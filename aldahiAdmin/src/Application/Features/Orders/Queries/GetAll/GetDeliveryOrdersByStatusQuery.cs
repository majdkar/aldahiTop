using MediatR;
using FirstCall.Application.Extensions;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Specifications.Orders;
using FirstCall.Shared.Wrapper;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using FirstCall.Shared.Constants.Clients;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Features.Orders.Queries.GetAll
{
    public class GetDeliveryOrdersByStatusQuery : IRequest<PaginatedResult<GetAllDeliveryOrdersResponse>>
    {
        public string status { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 100;
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetDeliveryOrdersByStatusQuery(string status, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }

            this.status = status;
        }
    }

    internal class GetDeliveryOrdersByStatusQueryHandler : IRequestHandler<GetDeliveryOrdersByStatusQuery, PaginatedResult<GetAllDeliveryOrdersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;


        public GetDeliveryOrdersByStatusQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public async Task<PaginatedResult<GetAllDeliveryOrdersResponse>> Handle(GetDeliveryOrdersByStatusQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<DeliveryOrder, GetAllDeliveryOrdersResponse>> expression = e => new GetAllDeliveryOrdersResponse
            {
                Id = e.Id,
                OrderNumber = e.OrderNumber,
                TotalPrice = e.TotalPrice,
                ClientName =e.Client.Person.FullName  ,
               
                ClientId = e.ClientId,
                Status = e.Status,
                OrderDate = e.OrderDate,
                ImageBillLadingUrl = e.ImageBillLadingUrl,
                 Products =e.Products,

            };
            var DeliveryOrdersFilterSpec = new DeliveryOrderByStatusFilterSpecification(request.status, request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<DeliveryOrder>().Entities
                    .Specify(DeliveryOrdersFilterSpec)
                    .Select(expression)
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<DeliveryOrder>().Entities
                   .Specify(DeliveryOrdersFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }

    }
}