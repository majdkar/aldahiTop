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

namespace FirstCall.Application.Features.Stocks.Queries.GetAll
{
    public class GetAllStocksQuery : IRequest<Result<List<GetAllStocksResponse>>>
    {
        public GetAllStocksQuery()
        {
        }
    }

    internal class GetAllStocksCachedQueryHandler : IRequestHandler<GetAllStocksQuery, Result<List<GetAllStocksResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllStocksCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllStocksResponse>>> Handle(GetAllStocksQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Stock>>> getAllStocks = () => _unitOfWork.Repository<Stock>().GetAllAsync();
            var StockList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllStocksCacheKey, getAllStocks);
            var mappedStocks = _mapper.Map<List<GetAllStocksResponse>>(StockList);
            return await Result<List<GetAllStocksResponse>>.SuccessAsync(mappedStocks);
        }
    }
}