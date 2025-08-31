using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Interfaces.Services.Identity;
using FirstCall.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.ExtendedAttributes;
using FirstCall.Domain.Entities.Misc;
using Microsoft.Extensions.Localization;
using FirstCall.Domain.Entities.GeneralSettings;


namespace FirstCall.Application.Features.Dashboards.Queries.GetData
{
    public class GetDashboardDataQuery : IRequest<Result<DashboardDataResponse>>
    {

    }

    internal class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, Result<DashboardDataResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IStringLocalizer<GetDashboardDataQueryHandler> _localizer;

        public GetDashboardDataQueryHandler(IUnitOfWork<int> unitOfWork, IUserService userService, IRoleService roleService, IStringLocalizer<GetDashboardDataQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _roleService = roleService;
            _localizer = localizer;
        }

        public async Task<Result<DashboardDataResponse>> Handle(GetDashboardDataQuery query, CancellationToken cancellationToken)
        {
            var response = new DashboardDataResponse
            {
                
                /*s0004s*/
 

 PrincedomCount = await _unitOfWork.Repository<Princedom>().Entities.CountAsync(cancellationToken),

 NationCount = await _unitOfWork.Repository<Nation>().Entities.CountAsync(cancellationToken),
               
                DocumentCount = await _unitOfWork.Repository<Document>().Entities.CountAsync(cancellationToken),
                DocumentTypeCount = await _unitOfWork.Repository<DocumentType>().Entities.CountAsync(cancellationToken),
                DocumentExtendedAttributeCount = await _unitOfWork.Repository<DocumentExtendedAttribute>().Entities.CountAsync(cancellationToken),
                UserCount = await _userService.GetCountAsync(),
                RoleCount = await _roleService.GetCountAsync()
            };

            var selectedYear = DateTime.Now.Year;
            double[] productsFigure = new double[13];
            double[] brandsFigure = new double[13];
            /*s0015s*/
double[] playersFigure = new double[13];
double[] playerclassificationsFigure = new double[13];
double[] sexsFigure = new double[13];
double[] academyrequestsFigure = new double[13];
double[] academysFigure = new double[13];
double[] clubrequestsFigure = new double[13];
double[] requeststatussFigure = new double[13];
double[] passportsFigure = new double[13];
double[] ownersFigure = new double[13];
double[] princedomsFigure = new double[13];
double[] clubtypesFigure = new double[13];
double[] clubsFigure = new double[13];
double[] championshipsclassificationsFigure = new double[13];
double[] nationsFigure = new double[13];
            double[] documentsFigure = new double[13];
            double[] documentTypesFigure = new double[13];
            double[] documentExtendedAttributesFigure = new double[13];
            for (int i = 1; i <= 12; i++)
            {
                var month = i;
                var filterStartDate = new DateTime(selectedYear, month, 01);
                var filterEndDate = new DateTime(selectedYear, month, DateTime.DaysInMonth(selectedYear, month), 23, 59, 59); // Monthly Based

                /*s0016s*/


princedomsFigure[i - 1] = await _unitOfWork.Repository<Princedom>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);


nationsFigure[i - 1] = await _unitOfWork.Repository<Nation>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentsFigure[i - 1] = await _unitOfWork.Repository<Document>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentTypesFigure[i - 1] = await _unitOfWork.Repository<DocumentType>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentExtendedAttributesFigure[i - 1] = await _unitOfWork.Repository<DocumentExtendedAttribute>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
            }

           
            /*s0017s*/

response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Sexs"],Data = sexsFigure });

response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["RequestStatuss"],Data = requeststatussFigure });
response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Passports"],Data = passportsFigure });
response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Owners"],Data = ownersFigure });
response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Princedoms"],Data = princedomsFigure });
response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Nations"],Data = nationsFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Documents"], Data = documentsFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Document Types"], Data = documentTypesFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Document Extended Attributes"], Data = documentExtendedAttributesFigure });

            return await Result<DashboardDataResponse>.SuccessAsync(response);
        }
    }
}