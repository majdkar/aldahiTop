using AutoMapper;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.Nations.Queries.GetById
{
    public class GetNationByIdQuery : IRequest<Result<GetNationByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductByIdQueryHandler : IRequestHandler<GetNationByIdQuery, Result<GetNationByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetNationByIdResponse>> Handle(GetNationByIdQuery query, CancellationToken cancellationToken)
        {
            var nation = await _unitOfWork.Repository<Nation>().GetByIdAsync(query.Id);
            var mappedNation = _mapper.Map<GetNationByIdResponse>(nation);
            return await Result<GetNationByIdResponse>.SuccessAsync(mappedNation);
        }
    }
}