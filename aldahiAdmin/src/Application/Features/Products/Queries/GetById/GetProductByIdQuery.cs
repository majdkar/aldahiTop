using AutoMapper;
using MediatR;
using FirstCall.Application.Features.Products.Queries.GetById;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.Products;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Application.Specifications.Products;
using FirstCall.Application.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FirstCall.Application.Features.Products.Queries.GetById
{
    public class GetProductByIdQuery : IRequest<Result<GetProductByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<GetProductByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetProductByIdResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var filter = new ProductByIdFilterSpecification(query.Id);
            Expression<Func<Product, GetProductByIdResponse>> expression = e => new GetProductByIdResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                NameEn = e.NameEn,
                Price = e.Price,
                Code = e.Code,

                Order = e.Order,

                ProductImageUrl = e.ProductImageUrl,
                ProductImageUrl2 = e.ProductImageUrl2,
                ProductImageUrl3 = e.ProductImageUrl3,
                ProductImageUrl4 = e.ProductImageUrl4,
                ProductCategoryId = e.ProductCategoryId,
                PackageNumber = e.PackageNumber,
                Qty = e.Qty,
                Kind = e.Kind,
                ProductCategory = e.ProductCategory,
                Sizes = e.Sizes,
                Colors = e.Colors,
                KindId = e.KindId,
                SeasonId = e.SeasonId,
                Season = e.Season,
                Group = e.Group,
                GroupId = e.GroupId,

                 WarehousesId = e.WarehousesId,
                 Warehouses = e.Warehouses,
            };

                var product =await  _unitOfWork.Repository<Product>().Entities
                .Specify(filter)
                .Select(expression)
                .FirstOrDefaultAsync();

            return await Result<GetProductByIdResponse>.SuccessAsync(product);
        }
    }
}