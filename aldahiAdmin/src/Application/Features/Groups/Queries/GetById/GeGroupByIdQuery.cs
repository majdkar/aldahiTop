using AutoMapper;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Groups.Queries.GetById
{
    public class GetGroupByIdQuery : IRequest<Result<GetGroupByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, Result<GetGroupByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetGroupByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetGroupByIdResponse>> Handle(GetGroupByIdQuery query, CancellationToken cancellationToken)
        {
            var Group = await _unitOfWork.Repository<Group>().GetByIdAsync(query.Id);
            var mappedGroup = _mapper.Map<GetGroupByIdResponse>(Group);
            return await Result<GetGroupByIdResponse>.SuccessAsync(mappedGroup);
        }
    }
}