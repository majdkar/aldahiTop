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
    public class GetAllParentCategoriesByTypeQuery : IRequest<Result<List<GetAllParentCategoriesByTypeResponse>>>
    {
        public string Type { get; set; }
    }

    internal class GetAllParentCategoriesByTypeQueryHandler : IRequestHandler<GetAllParentCategoriesByTypeQuery, Result<List<GetAllParentCategoriesByTypeResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllParentCategoriesByTypeQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllParentCategoriesByTypeResponse>>> Handle(GetAllParentCategoriesByTypeQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<ProductCategory>>> getAllProductCategories = () => _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            var categoriesList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllProductCategoriesCacheKey, getAllProductCategories);
            var mappedcategoriess = new List<GetAllParentCategoriesByTypeResponse>();
            if (String.IsNullOrEmpty(request.Type))
                 mappedcategoriess = _mapper.Map<List<GetAllParentCategoriesByTypeResponse>>(categoriesList.Where(x =>x.ParentCategoryId == null&& !x.IsDeleted));
            else
                mappedcategoriess = _mapper.Map<List<GetAllParentCategoriesByTypeResponse>>(categoriesList.Where(x => x.Type == request.Type && x.ParentCategoryId==null && !x.IsDeleted).OrderBy(x => x.Order));
            return await Result<List<GetAllParentCategoriesByTypeResponse>>.SuccessAsync(mappedcategoriess);
        }
    }

}
