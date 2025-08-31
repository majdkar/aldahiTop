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
    public class GetAllProductsQuery : IRequest<Result<List<GetAllProductsResponse>>>
    {
        public GetAllProductsQuery()
        {

        }
    }
    public class GetAllProductsCachedQueryHandler : IRequestHandler<GetAllProductsQuery, Result<List<GetAllProductsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllProductsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllProductsResponse>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var filter = new ProductFilterSpecification();
            Expression<Func<Product, GetAllProductsResponse>> expression =  e => new GetAllProductsResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                NameEn = e.NameEn,

                Code = e.Code,


            };

            var  getAllProducts = await _unitOfWork.Repository<Product>().Entities
                .Specify(filter).OrderBy(x => x.Order)
                .Select(expression)
                .ToListAsync();

            return await Result<List<GetAllProductsResponse>>.SuccessAsync(getAllProducts);

        }

 



    }
}
