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

namespace FirstCall.Application.Features.Kinds.Queries.GetAll
{
    public class GetAllKindsQuery : IRequest<Result<List<GetAllKindsResponse>>>
    {
        public GetAllKindsQuery()
        {
        }
    }

    internal class GetAllKindsCachedQueryHandler : IRequestHandler<GetAllKindsQuery, Result<List<GetAllKindsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllKindsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllKindsResponse>>> Handle(GetAllKindsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Kind>>> getAllKinds = () => _unitOfWork.Repository<Kind>().GetAllAsync();
            var KindList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllKindsCacheKey, getAllKinds);
            var mappedKinds = _mapper.Map<List<GetAllKindsResponse>>(KindList);
            return await Result<List<GetAllKindsResponse>>.SuccessAsync(mappedKinds);
        }
    }
}