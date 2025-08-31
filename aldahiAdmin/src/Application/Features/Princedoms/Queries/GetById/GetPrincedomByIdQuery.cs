using System;
using AutoMapper;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.Princedoms.Queries.GetById
{
    public class GetPrincedomByIdQuery : IRequest<Result<GetPrincedomByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductByIdQueryHandler : IRequestHandler<GetPrincedomByIdQuery, Result<GetPrincedomByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetPrincedomByIdResponse>> Handle(GetPrincedomByIdQuery query, CancellationToken cancellationToken)
        {
            var princedom = await _unitOfWork.Repository<Princedom>().GetByIdAsync(query.Id);
            var mappedPrincedom = _mapper.Map<GetPrincedomByIdResponse>(princedom);
            return await Result<GetPrincedomByIdResponse>.SuccessAsync(mappedPrincedom);
        }
    }
}