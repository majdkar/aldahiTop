using AutoMapper;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Stocks.Queries.GetById
{
    public class GetStockByIdQuery : IRequest<Result<GetStockByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetStockByIdQueryHandler : IRequestHandler<GetStockByIdQuery, Result<GetStockByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetStockByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetStockByIdResponse>> Handle(GetStockByIdQuery query, CancellationToken cancellationToken)
        {
            var Stock = await _unitOfWork.Repository<Stock>().GetByIdAsync(query.Id);
            var mappedStock = _mapper.Map<GetStockByIdResponse>(Stock);
            return await Result<GetStockByIdResponse>.SuccessAsync(mappedStock);
        }
    }
}