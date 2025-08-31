using AutoMapper;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Warehousess.Queries.GetById
{
    public class GetWarehousesByIdQuery : IRequest<Result<GetWarehousesByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetWarehousesByIdQueryHandler : IRequestHandler<GetWarehousesByIdQuery, Result<GetWarehousesByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetWarehousesByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetWarehousesByIdResponse>> Handle(GetWarehousesByIdQuery query, CancellationToken cancellationToken)
        {
            var Warehouses = await _unitOfWork.Repository<Warehouses>().GetByIdAsync(query.Id);
            var mappedWarehouses = _mapper.Map<GetWarehousesByIdResponse>(Warehouses);
            return await Result<GetWarehousesByIdResponse>.SuccessAsync(mappedWarehouses);
        }
    }
}