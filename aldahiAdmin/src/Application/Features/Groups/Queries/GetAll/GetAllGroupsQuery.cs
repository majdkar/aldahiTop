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

namespace FirstCall.Application.Features.Groups.Queries.GetAll
{
    public class GetAllGroupsQuery : IRequest<Result<List<GetAllGroupsResponse>>>
    {
        public GetAllGroupsQuery()
        {
        }
    }

    internal class GetAllGroupsCachedQueryHandler : IRequestHandler<GetAllGroupsQuery, Result<List<GetAllGroupsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllGroupsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllGroupsResponse>>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Group>>> getAllGroups = () => _unitOfWork.Repository<Group>().GetAllAsync();
            var GroupList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllGroupsCacheKey, getAllGroups);
            var mappedGroups = _mapper.Map<List<GetAllGroupsResponse>>(GroupList);
            return await Result<List<GetAllGroupsResponse>>.SuccessAsync(mappedGroups);
        }
    }
}