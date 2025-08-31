using AutoMapper;
using MediatR;
using FirstCall.Application.Features.Countries.Queries.GetById;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.Countries.Queries.GetById
{
    public class GetCountryByIdQuery : IRequest<Result<GetCountryByIdResponse>>
    {
        public int Id { get; set; }
    }
    internal class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, Result<GetCountryByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCountryByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetCountryByIdResponse>> Handle(GetCountryByIdQuery query, CancellationToken cancellationToken)
        {
            var princedom = await _unitOfWork.Repository<Country>().GetByIdAsync(query.Id);
            var mappedCountry = _mapper.Map<GetCountryByIdResponse>(princedom);
            return await Result<GetCountryByIdResponse>.SuccessAsync(mappedCountry);
        }
    }
}