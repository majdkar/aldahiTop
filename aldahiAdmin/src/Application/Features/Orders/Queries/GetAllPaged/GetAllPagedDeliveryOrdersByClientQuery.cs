using MediatR;
using FirstCall.Application.Extensions;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Specifications.Orders;
using FirstCall.Shared.Constants.Clients;
using FirstCall.Shared.Wrapper;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using FirstCall.Application.Features.Orders.Queries.GetAll;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Features.DeliveryOrders.Queries.GetAllPaged
{
    public class GetAllPagedDeliveryOrdersByClientQuery : IRequest<PaginatedResult<GetAllDeliveryOrdersResponse>>
    {
        public int ClientId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 100;
        public string SearchString { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllPagedDeliveryOrdersByClientQuery(int clientId, int pageNumber, int pageSize, string searchString,string status, string orderBy,string type)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
            ClientId = clientId;
            Type = type;
            Status = status;
        }
    }

    internal class GetAllPagedDeliveryOrdersByClientQueryHandler : IRequestHandler<GetAllPagedDeliveryOrdersByClientQuery, PaginatedResult<GetAllDeliveryOrdersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;


        public GetAllPagedDeliveryOrdersByClientQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public async Task<PaginatedResult<GetAllDeliveryOrdersResponse>> Handle(GetAllPagedDeliveryOrdersByClientQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<DeliveryOrder, GetAllDeliveryOrdersResponse>> expression = e => new GetAllDeliveryOrdersResponse
            {
                Id = e.Id,
                OrderNumber = e.OrderNumber,
                TotalPrice = e.TotalPrice,
                ClientName = e.Client.Person.FullName,
                 Type = e.Type,
                ClientId = e.ClientId,
                Status = e.Status,
                OrderDate = e.OrderDate,
                ImageBillLadingUrl = e.ImageBillLadingUrl,
                 Products =e.Products,
            };
            var DeliveryOrdersFilterSpec = new DeliveryOrderByClientIdFilterSpecification(request.SearchString,request.ClientId,request.Type,request.Status);
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