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
    public class GetProductComponentByIdQuery : IRequest<Result<GetProductComponentByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductComponentByIdQueryHandler : IRequestHandler<GetProductComponentByIdQuery, Result<GetProductComponentByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductComponentByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetProductComponentByIdResponse>> Handle(GetProductComponentByIdQuery query, CancellationToken cancellationToken)
        {
            var filter = new ProductComponentByIdFilterSpecification(query.Id);
            Expression<Func<ProductCom, GetProductComponentByIdResponse>> expression = e => new GetProductComponentByIdResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                NameEn = e.NameEn,
                DescriptionAboutEn = e.DescriptionAboutEn,

                ProductId = e.ProductId,
                ProductComponentImageUrl = e.ProductComponentImageUrl,
                Product = e.Product,
                   ProductComponentImageUrl1 = e.ProductComponentImageUrl1,
                ProductComponentImageUrl2 = e.ProductComponentImageUrl2,
                ProductComponentImageUrl3 = e.ProductComponentImageUrl3,
                ProductComponentImageUrl4 = e.ProductComponentImageUrl4,
                ProductComponentImageUrl5 = e.ProductComponentImageUrl5,
                Order = e.Order,
            };

                var product =await  _unitOfWork.Repository<ProductCom>().Entities
                .Specify(filter)
                .Select(expression)
                .FirstOrDefaultAsync();

            return await Result<GetProductComponentByIdResponse>.SuccessAsync(product);
        }
    }
}