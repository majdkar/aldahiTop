using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Application.Extensions;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Interfaces.Services;
using FirstCall.Application.Specifications.GeneralSettings;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FirstCall.Application.Features.Nations.Queries.Export
{
    public class ExportNationsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportNationsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportNationsQueryHandler : IRequestHandler<ExportNationsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportNationsQueryHandler> _localizer;

        public ExportNationsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportNationsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportNationsQuery request, CancellationToken cancellationToken)
        {
            var nationFilterSpec = new NationFilterSpecification(request.SearchString);
            var nations = await _unitOfWork.Repository<Nation>().Entities
                .Specify(nationFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(nations, mappers: new Dictionary<string, Func<Nation, object>>
            {
				{ _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description },
				
				{ _localizer["ArabicName"],item => item.ArabicName },
				{ _localizer["Code"],item => item.Code },
				{ _localizer["PhoneCode"],item => item.PhoneCode },

                //{ _localizer["Tax"], item => item.Tax }
            }, sheetName: _localizer["Nations"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
