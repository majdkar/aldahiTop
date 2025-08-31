using AutoMapper;
using LazyCache;
using MediatR;
using FirstCall.Application.Features.ProductCategories.Queries.GetAll;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Core.Entities;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace FirstCall.Application.Features.ProductCategories.Queries.GetAll
{
    public class GetAllProductCategoriesQuery : IRequest<Result<List<GetAllProductCategoriesResponse>>>
    {
        public GetAllProductCategoriesQuery()
        {
        }
    }

    internal class GetAllProductCategoriesCachedQueryHandler : IRequestHandler<GetAllProductCategoriesQuery, Result<List<GetAllProductCategoriesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllProductCategoriesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllProductCategoriesResponse>>> Handle(GetAllProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<ProductCategory>>> getAllProductCategories = () => _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            var categoriesList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllProductCategoriesCacheKey, getAllProductCategories);
            //var categoriesList = await _unitOfWork.Repository<ProductCategory>().Entities.Where(x => !x.IsDeleted).AsTracking().ToListAsync();
            var mappedcategoriess = _mapper.Map<List<GetAllProductCategoriesResponse>>(categoriesList.Where(x => !x.IsDeleted).OrderBy(x => x.Order));
            return await Result<List<GetAllProductCategoriesResponse>>.SuccessAsync(mappedcategoriess);
        }
    }
}