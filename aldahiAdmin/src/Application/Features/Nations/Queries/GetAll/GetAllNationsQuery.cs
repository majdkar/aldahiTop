using AutoMapper;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.Nations.Queries.GetAll
{
    public class GetAllNationsQuery : IRequest<Result<List<GetAllNationsResponse>>>
    {
        public GetAllNationsQuery()
        {
        }
    }

    internal class GetAllNationsCachedQueryHandler : IRequestHandler<GetAllNationsQuery, Result<List<GetAllNationsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllNationsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllNationsResponse>>> Handle(GetAllNationsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Nation>>> getAllNations = () => _unitOfWork.Repository<Nation>().GetAllAsync();
            var nationList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllNationsCacheKey, getAllNations);
            var mappedNations = _mapper.Map<List<GetAllNationsResponse>>(nationList);
            return await Result<List<GetAllNationsResponse>>.SuccessAsync(mappedNations);
        }
    }
}