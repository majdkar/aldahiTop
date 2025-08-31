using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Extensions;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Interfaces.Services;
using FirstCall.Application.Specifications.GeneralSettings;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Application.Specifications.Orders;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Features.Orders.Queries.Export
{
    public class ExportDeliveryOrdersQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportDeliveryOrdersQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }
    internal class ExportDeliveryOrdersQueryHandler : IRequestHandler<ExportDeliveryOrdersQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportDeliveryOrdersQueryHandler> _localizer;

        public ExportDeliveryOrdersQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportDeliveryOrdersQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportDeliveryOrdersQuery request, CancellationToken cancellationToken)
        {
            var orderFilterSpec = new DeliveryOrderFilterSpecification(request.SearchString);
            var deliveryOrders = await _unitOfWork.Repository<DeliveryOrder>().Entities
                .Specify(orderFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(deliveryOrders, mappers: new Dictionary<string, Func<DeliveryOrder, object>>
            {
                { _localizer["Id"], item => item.Id },
                 { _localizer["Number"], item => item.OrderNumber},
                { _localizer["TotalPrice"], item => item.TotalPrice },
                 { _localizer["FullName"], item => item.Client.Person.FullName },
            }, sheetName: _localizer["DeliveryOrders"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
