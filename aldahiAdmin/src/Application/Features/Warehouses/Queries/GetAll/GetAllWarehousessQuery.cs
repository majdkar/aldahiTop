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

namespace FirstCall.Application.Features.Warehousess.Queries.GetAll
{
    public class GetAllWarehousessQuery : IRequest<Result<List<GetAllWarehousessResponse>>>
    {
        public GetAllWarehousessQuery()
        {
        }
    }

    internal class GetAllWarehousessCachedQueryHandler : IRequestHandler<GetAllWarehousessQuery, Result<List<GetAllWarehousessResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllWarehousessCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllWarehousessResponse>>> Handle(GetAllWarehousessQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Warehouses>>> getAllWarehousess = () => _unitOfWork.Repository<Warehouses>().GetAllAsync();
            var WarehousesList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllWarehousessCacheKey, getAllWarehousess);
            var mappedWarehousess = _mapper.Map<List<GetAllWarehousessResponse>>(WarehousesList);
            return await Result<List<GetAllWarehousessResponse>>.SuccessAsync(mappedWarehousess);
        }
    }
}