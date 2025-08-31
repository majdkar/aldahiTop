using System;
using AutoMapper;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Wrapper;
using LazyCache;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.Princedoms.Queries.GetAll
{
    public class GetAllPrincedomsQuery : IRequest<Result<List<GetAllPrincedomsResponse>>>
    {
        public GetAllPrincedomsQuery()
        {
        }
    }

    internal class GetAllPrincedomsCachedQueryHandler : IRequestHandler<GetAllPrincedomsQuery, Result<List<GetAllPrincedomsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllPrincedomsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllPrincedomsResponse>>> Handle(GetAllPrincedomsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Princedom>>> getAllPrincedoms = () => _unitOfWork.Repository<Princedom>().GetAllAsync();
            var princedomList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllPrincedomsCacheKey, getAllPrincedoms);
            var mappedPrincedoms = _mapper.Map<List<GetAllPrincedomsResponse>>(princedomList);
            return await Result<List<GetAllPrincedomsResponse>>.SuccessAsync(mappedPrincedoms);
        }
    }
}