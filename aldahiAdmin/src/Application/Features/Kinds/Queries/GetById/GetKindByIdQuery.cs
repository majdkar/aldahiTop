using AutoMapper;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Kinds.Queries.GetById
{
    public class GetKindByIdQuery : IRequest<Result<GetKindByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetKindByIdQueryHandler : IRequestHandler<GetKindByIdQuery, Result<GetKindByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetKindByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetKindByIdResponse>> Handle(GetKindByIdQuery query, CancellationToken cancellationToken)
        {
            var Kind = await _unitOfWork.Repository<Kind>().GetByIdAsync(query.Id);
            var mappedKind = _mapper.Map<GetKindByIdResponse>(Kind);
            return await Result<GetKindByIdResponse>.SuccessAsync(mappedKind);
        }
    }
}