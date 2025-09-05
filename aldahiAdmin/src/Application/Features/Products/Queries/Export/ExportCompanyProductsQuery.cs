using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Extensions;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Interfaces.Services;
using FirstCall.Application.Specifications.Catalog;
using FirstCall.Domain.Entities.Products;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Application.Specifications.Products;
using System.Linq;

namespace FirstCall.Application.Features.Products.Queries.Export
{
    public class ExportCompanyProductsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        //public int CompanyId { get; set; }

        public ExportCompanyProductsQuery(string searchString = "")
        {
            SearchString = searchString;
            //CompanyId = companyId;
        }
    }

    internal class ExportProductsQueryHandler : IRequestHandler<ExportCompanyProductsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportProductsQueryHandler> _localizer;

        public ExportProductsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportProductsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCompanyProductsQuery request, CancellationToken cancellationToken)
        {
            var productFilterSpec = new ProductByCompanyFilterSpecification(request.SearchString,"B2B");
            var products = await _unitOfWork.Repository<Product>().Entities
                .Specify(productFilterSpec).OrderBy(x => x.Order)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(products, mappers: new Dictionary<string, Func<Product, object>>
            {
                { _localizer["NameAr"], item => item.NameAr },
                { _localizer["Category"], item => item.ProductCategory.NameAr },
                { _localizer["Kind"], item => item.Kind.NameAr },
                { _localizer["Code"], item => item.Code },
                { _localizer["Qty"], item => item.Qty },
                { _localizer["Package Number"], item => item.PackageNumber },
                { _localizer["Price"], item => item.Price }
            }, sheetName: _localizer["Products"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}