using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Extensions;
using FirstCall.Application.Features.Countries.Queries.Export;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Interfaces.Services;
using FirstCall.Application.Specifications.GeneralSettings;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.Countries.Queries.Export
{
    public class ExportCountriesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportCountriesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }
    internal class ExportCountriesQueryHandler : IRequestHandler<ExportCountriesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportCountriesQueryHandler> _localizer;

        public ExportCountriesQueryHandler(IExcelService excelService, IUnitOfWork<int> unitOfWork, IStringLocalizer<ExportCountriesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCountriesQuery request, CancellationToken cancellationToken)
        {
            var countryFilterSpec = new CountryFilterSpecification(request.SearchString);
            var countrys = await _unitOfWork.Repository<Country>().Entities
                .Specify(countryFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(countrys, mappers: new Dictionary<string, Func<Country, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["NameAr"], item => item.NameAr },
                { _localizer["NameEn"], item => item.NameEn },
                { _localizer["Code"],item => item.Code },
                { _localizer["PhoneCode"], item => item.PhoneCode }
            }, sheetName: _localizer["Countries"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
