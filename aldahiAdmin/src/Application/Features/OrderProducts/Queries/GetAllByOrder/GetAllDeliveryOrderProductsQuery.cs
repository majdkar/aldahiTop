using MediatR;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Application.Extensions;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Specifications.OrderProducts;
using FirstCall.Core.Entities;
using FirstCall.Domain.Entities.Clients;
using FirstCall.Domain.Entities.Orders;
using FirstCall.Domain.Entities.Products;
using FirstCall.Shared.Constants.Orders;
using FirstCall.Shared.Wrapper;

namespace FirstCall.Application.Features.OrderProducts.Queries.GetAllByOrder
{
    public class GetAllDeliveryOrderProductsQuery : IRequest<PaginatedResult<GetAllDeliveryOrderProductsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...
        public int DeliveryOrderId { get; set; }


        public GetAllDeliveryOrderProductsQuery(int pageNumber, int pageSize, string searchString, string orderBy, int deliveryOrderId)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
            DeliveryOrderId = deliveryOrderId;
        }
    }

    internal class GetAllDeliveryOrderProductsQueryHandler : IRequestHandler<GetAllDeliveryOrderProductsQuery, PaginatedResult<GetAllDeliveryOrderProductsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllDeliveryOrderProductsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllDeliveryOrderProductsResponse>> Handle(GetAllDeliveryOrderProductsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<DeliveryOrderProduct, GetAllDeliveryOrderProductsResponse>> expression = e => new GetAllDeliveryOrderProductsResponse
            {

                Id = e.Id,
                UnitPrice = e.UnitPrice,
                Quantity = e.Quantity,
                TotalPrice = e.TotalPrice,
                DeliveryOrder = e.DeliveryOrder.OrderNumber,
                DeliveryOrderId = e.DeliveryOrderId,
             ProductId = e.ProductId,

            };
            var DeliveryOrderProductFilterSpec = new DeliveryOrderProductFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<DeliveryOrderProduct>().Entities
                    .Where(x => x.DeliveryOrderId == request.DeliveryOrderId)
                   .Specify(DeliveryOrderProductFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<DeliveryOrderProduct>().Entities
                   .Where(x => x.DeliveryOrderId == request.DeliveryOrderId)
                                            .Specify(DeliveryOrderProductFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                                            .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
