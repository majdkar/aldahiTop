using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Interfaces.Services;
using FirstCall.Domain.Entities.GeneralSettings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Application.Extensions;
using FirstCall.Application.Specifications.Catalog;
using FirstCall.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Features.Products.Queries.Export;
using FirstCall.Core.Entities;
using System.Linq;

namespace FirstCall.Application.Features.ProductCategories.Queries.Export
{
    public class ExportProductCategoriesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportProductCategoriesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportProductCategoriesQueryHandler : IRequestHandler<ExportProductCategoriesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportProductCategoriesQueryHandler> _localizer;

        public ExportProductCategoriesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportProductCategoriesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            var productCategoryFilterSpec = new ProductCategoryFilterSpecification(request.SearchString);
            var productCategories= await _unitOfWork.Repository<ProductCategory>().Entities.Where(x=>!x.IsDeleted)
                .Specify(productCategoryFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(productCategories, mappers: new Dictionary<string, Func<ProductCategory, object>>
            {
                { _localizer["Id"], item => item.Id },
                 { _localizer["Type"], item => item.Type },
                { _localizer["NameEn"], item => item.NameEn },
                 { _localizer["NameAr"], item => item.NameAr },
                { _localizer["DescriptionAr"], item => item.DescriptionAr },
                { _localizer["DescriptionEn"], item => item.DescriptionEn },
                 { _localizer["Order"], item => item.Order },
            }, sheetName: _localizer["ProductCategories"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
