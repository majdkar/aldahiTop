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

namespace FirstCall.Application.Features.Princedoms.Queries.Export
{
    public class ExportPrincedomsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportPrincedomsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportPrincedomsQueryHandler : IRequestHandler<ExportPrincedomsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportPrincedomsQueryHandler> _localizer;

        public ExportPrincedomsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportPrincedomsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportPrincedomsQuery request, CancellationToken cancellationToken)
        {
            var princedomFilterSpec = new PrincedomFilterSpecification(request.SearchString);
            var princedoms = await _unitOfWork.Repository<Princedom>().Entities
                .Specify(princedomFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(princedoms, mappers: new Dictionary<string, Func<Princedom, object>>
            {
				{ _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description },
				
				{ _localizer["ar_title"],item => item.ar_title },
				{ _localizer["en_title"],item => item.en_title },
				{ _localizer["Code"],item => item.Code },

                //{ _localizer["Tax"], item => item.Tax }
            }, sheetName: _localizer["Princedoms"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
