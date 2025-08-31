using AutoMapper;
using FirstCall.Application.Interfaces.Repositories;
 
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Seasons.Queries.GetAll
{
    public class GetAllSeasonsQuery : IRequest<Result<List<GetAllSeasonsResponse>>>
    {
        public GetAllSeasonsQuery()
        {
        }
    }

    internal class GetAllSeasonsCachedQueryHandler : IRequestHandler<GetAllSeasonsQuery, Result<List<GetAllSeasonsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllSeasonsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllSeasonsResponse>>> Handle(GetAllSeasonsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Season>>> getAllSeasons = () => _unitOfWork.Repository<Season>().GetAllAsync();
            var SeasonList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllSeasonsCacheKey, getAllSeasons);
            var mappedSeasons = _mapper.Map<List<GetAllSeasonsResponse>>(SeasonList);
            return await Result<List<GetAllSeasonsResponse>>.SuccessAsync(mappedSeasons);
        }
    }
}