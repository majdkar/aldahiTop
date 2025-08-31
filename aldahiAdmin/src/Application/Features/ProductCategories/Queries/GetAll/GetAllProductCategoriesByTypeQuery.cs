using AutoMapper;
using LazyCache;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Core.Entities;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.ProductCategories.Queries.GetAll
{
    public class GetAllProductCategoriesByTypeQuery : IRequest<Result<List<GetAllProductCategoriesResponse>>>
    {
        public string Type { get; set; }
    }

    internal class GetAllProductCategoriesByTypeQueryHandler : IRequestHandler<GetAllProductCategoriesByTypeQuery, Result<List<GetAllProductCategoriesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllProductCategoriesByTypeQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllProductCategoriesResponse>>> Handle(GetAllProductCategoriesByTypeQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<ProductCategory>>> getAllProductCategories = () => _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            var categoriesList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllProductCategoriesCacheKey, getAllProductCategories);
            var mappedcategoriess = _mapper.Map<List<GetAllProductCategoriesResponse>>(categoriesList.Where(x => x.Type == request.Type).OrderBy(x => x.Order));
            return await Result<List<GetAllProductCategoriesResponse>>.SuccessAsync(mappedcategoriess);
        }
    }
}