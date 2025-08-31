using AutoMapper;
using LazyCache;
using MediatR;
using FirstCall.Application.Features.Countries.Queries.GetAll;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace FirstCall.Application.Features.Countries.Queries.GetAll
{
    public class GetAllCountriesQuery : IRequest<Result<List<GetAllCountriesResponse>>>
    {
        public GetAllCountriesQuery()
        {

        }
    }
    internal class GetAllCountriesCachedQueryHandler : IRequestHandler<GetAllCountriesQuery, Result<List<GetAllCountriesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCountriesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCountriesResponse>>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
        {
            var getAllCountries = _unitOfWork.Repository<Country>().Entities.Where(x => x.IsDeleted == false).OrderBy("Id");
            var mappedCountries = _mapper.Map<List<GetAllCountriesResponse>>(getAllCountries);
            return await Result<List<GetAllCountriesResponse>>.SuccessAsync(mappedCountries);
        }
    }
}