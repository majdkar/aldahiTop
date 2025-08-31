using AutoMapper;
using LazyCache;
using MediatR;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.Products;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Application.Extensions;
using System.Linq.Expressions;
using FirstCall.Application.Features.Products.Queries.GetAllPaged;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using FirstCall.Domain.Entities.GeneralSettings;
using System.Text.Json;
using FirstCall.Application.Specifications.Products;

namespace FirstCall.Application.Features.Products.Queries.GetAll
{
    public class GetAllProductComponentsQuery : IRequest<Result<List<GetAllProductComponentsResponse>>>
    {
        public GetAllProductComponentsQuery()
        {

        }
    }
    public class GetAllProductComponentsCachedQueryHandler : IRequestHandler<GetAllProductComponentsQuery, Result<List<GetAllProductComponentsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllProductComponentsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllProductComponentsResponse>>> Handle(GetAllProductComponentsQuery request, CancellationToken cancellationToken)
        {
            var filter = new ProductComponentFilterSpecification();
            Expression<Func<ProductCom, GetAllProductComponentsResponse>> expression =  e => new GetAllProductComponentsResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                NameEn = e.NameEn,
   DescriptionAboutEn = e.DescriptionAboutEn,
               
      ProductId = e.ProductId,
                ProductComponentImageUrl = e.ProductComponentImageUrl,
                ProductComponentImageUrl1 = e.ProductComponentImageUrl1,
                ProductComponentImageUrl2 = e.ProductComponentImageUrl2,
                ProductComponentImageUrl3 = e.ProductComponentImageUrl3,
                ProductComponentImageUrl4 = e.ProductComponentImageUrl4,
                ProductComponentImageUrl5 = e.ProductComponentImageUrl5,
      Product =e.Product


            };

            var  getAllProducts = await _unitOfWork.Repository<ProductCom>().Entities
                .Specify(filter)
                .Select(expression)
                .ToListAsync();

            return await Result<List<GetAllProductComponentsResponse>>.SuccessAsync(getAllProducts);

        }

 



    }
}
