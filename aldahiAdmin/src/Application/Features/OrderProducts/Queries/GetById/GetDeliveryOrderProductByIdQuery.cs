using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Application.Extensions;
using FirstCall.Application.Features.DeliveryOrders.Queries.GetById;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Specifications.OrderProducts;
using FirstCall.Domain.Entities.Orders;
using FirstCall.Shared.Wrapper;

namespace FirstCall.Application.Features.OrderProducts.Queries.GetById
{
    public class GetDeliveryOrderProductByIdQuery : IRequest<Result<GetDeliveryOrderProductByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetDeliveryOrderProductByIdQueryHandler : IRequestHandler<GetDeliveryOrderProductByIdQuery, Result<GetDeliveryOrderProductByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetDeliveryOrderProductByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetDeliveryOrderProductByIdResponse>> Handle(GetDeliveryOrderProductByIdQuery query, CancellationToken cancellationToken)
        {
            var DeliveryOrdersFilterSpec = new DeliveryOrderProductByIdFilterSpecification(query.Id);
            var deliveryOrder = await _unitOfWork.Repository<DeliveryOrderProduct>().Entities
                .Specify(DeliveryOrdersFilterSpec)
                .FirstOrDefaultAsync();
            var mappeddeliveryOrder = _mapper.Map<GetDeliveryOrderProductByIdResponse>(deliveryOrder);

            return await Result<GetDeliveryOrderProductByIdResponse>.SuccessAsync(mappeddeliveryOrder);
        }
    }
}


