using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FirstCall.Application.Extensions;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Specifications.Orders;
using FirstCall.Shared.Wrapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.Orders;
using FirstCall.Application.Features.Orders.Queries.GetAll;
using FirstCall.Application.Features.OrderProducts.Queries.GetAllByOrder;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Features.DeliveryOrders.Queries.GetById
{
    public class GetDeliveryOrderByIdQuery : IRequest<Result<GetDeliveryOrderByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetDeliveryOrderByIdQueryHandler : IRequestHandler<GetDeliveryOrderByIdQuery, Result<GetDeliveryOrderByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetDeliveryOrderByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetDeliveryOrderByIdResponse>> Handle(GetDeliveryOrderByIdQuery query, CancellationToken cancellationToken)
        {
            var DeliveryOrdersFilterSpec = new DeliveryOrderByIdFilterSpecification(query.Id);
            var deliveryOrder = await _unitOfWork.Repository<DeliveryOrder>().Entities
                .Specify(DeliveryOrdersFilterSpec)
                .FirstOrDefaultAsync();
            var mappeddeliveryOrder = _mapper.Map<GetDeliveryOrderByIdResponse>(deliveryOrder);

            mappeddeliveryOrder.Products = await _unitOfWork.Repository<DeliveryOrderProduct>().Entities.Where(x => x.DeliveryOrderId == mappeddeliveryOrder.Id)
                .Select(x => new GetAllDeliveryOrderProductsResponse
                {
                    Id = x.Id,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    TotalPrice = x.TotalPrice,
                    ImageUrl = _unitOfWork.Repository<Product>().Entities.FirstOrDefault(w => w.Id == x.ProductId).ProductImageUrl


                }).ToListAsync();

            return await Result<GetDeliveryOrderByIdResponse>.SuccessAsync(mappeddeliveryOrder);
        }
    }
}
