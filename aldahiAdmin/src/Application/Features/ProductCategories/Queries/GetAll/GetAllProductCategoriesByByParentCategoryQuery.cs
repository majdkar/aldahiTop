using AutoMapper;
using LazyCache;
using FirstCall.Application.Extensions;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Specifications.Catalog;
using FirstCall.Core.Entities;
using FirstCall.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.ProductCategories.Queries.GetAll
{
    public class GetAllProductCategoriesByByParentCategoryQuery : IRequest<Result<List<GetAllProductCategoriesResponse>>>
    {
        public int ParentCategoryId { get; set; }
    }

    internal class GetAllProductCategoriesByByParentCategoryQueryHandler : IRequestHandler<GetAllProductCategoriesByByParentCategoryQuery, Result<List<GetAllProductCategoriesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllProductCategoriesByByParentCategoryQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetAllProductCategoriesResponse>>> Handle(GetAllProductCategoriesByByParentCategoryQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ProductCategory, GetAllProductCategoriesResponse>> expression = e => new GetAllProductCategoriesResponse
            {
                Id = e.Id,
                NameEn = e.NameEn,
                NameAr = e.NameAr,
                ParentCategoryId = e.ParentCategoryId,
                ImageDataURL = e.ImageDataURL,
                Type = e.Type
            };
            var productCategoryFilterSpec = new ProductCategoryFilterSpecification("");
            var data = await _unitOfWork.Repository<ProductCategory>().Entities
                .Where(x=>x.ParentCategoryId== request.ParentCategoryId)
                .Specify(productCategoryFilterSpec)
                .OrderBy(x => x.Order)
                .Select(expression)
                .ToListAsync();

            return await Result<List<GetAllProductCategoriesResponse>>.SuccessAsync(data);
        }
    }
}