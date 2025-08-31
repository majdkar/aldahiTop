using AutoMapper;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Seasons.Queries.GetById
{
    public class GetSeasonByIdQuery : IRequest<Result<GetSeasonByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetSeasonByIdQueryHandler : IRequestHandler<GetSeasonByIdQuery, Result<GetSeasonByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetSeasonByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetSeasonByIdResponse>> Handle(GetSeasonByIdQuery query, CancellationToken cancellationToken)
        {
            var Season = await _unitOfWork.Repository<Season>().GetByIdAsync(query.Id);
            var mappedSeason = _mapper.Map<GetSeasonByIdResponse>(Season);
            return await Result<GetSeasonByIdResponse>.SuccessAsync(mappedSeason);
        }
    }
}