 using MediatR;
using FirstCall.Application.Extensions;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using FirstCall.Application.Specifications.Orders;
using FirstCall.Shared.Constants.Clients;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Features.Orders.Queries.GetAll
{
    public class GetAllDeliveryOrdersQuery : IRequest<PaginatedResult<GetAllDeliveryOrdersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 100;
        public string Type { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllDeliveryOrdersQuery(int pageNumber, int pageSize, string searchString, string orderBy,string type)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
            Type = type;

        }
    }

    internal class GetAllDeliveryOrdersQueryHandler : IRequestHandler<GetAllDeliveryOrdersQuery, PaginatedResult<GetAllDeliveryOrdersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;


        public GetAllDeliveryOrdersQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public async Task<PaginatedResult<GetAllDeliveryOrdersResponse>> Handle(GetAllDeliveryOrdersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<DeliveryOrder, GetAllDeliveryOrdersResponse>> expression = e => new GetAllDeliveryOrdersResponse
            {
                Id = e.Id,
                OrderNumber = e.OrderNumber,
                ClientId = e.ClientId,
                   ImageBillLadingUrl = e.ImageBillLadingUrl,
                   OrderDate = e.OrderDate,
                   Status = e.Status,
                   ClientName = e.Client.Person.FullName,
                   TotalPrice = e.TotalPrice,
               
                Products = e.Products,
                Client = e.Client,
                 
            };
            var DeliveryOrdersFilterSpec = new DeliveryOrderFilterSpecification(request.SearchString,request.Type);
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