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
            
            return await Result<GetDeliveryOrderByIdResponse>.SuccessAsync(mappeddeliveryOrder);
        }
    }
}
