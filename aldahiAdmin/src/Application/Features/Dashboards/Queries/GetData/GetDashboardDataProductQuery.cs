using DocumentFormat.OpenXml.Wordprocessing;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Interfaces.Services.Identity;
using FirstCall.Domain.Entities.ExtendedAttributes;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Domain.Entities.Misc;
using FirstCall.Domain.Entities.Products;
using FirstCall.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace FirstCall.Application.Features.Dashboards.Queries.GetData
{
    public class GetDashboardDataProductQuery : IRequest<Result<List<DashboardDataProductResponse>>>
    {

    }

    internal class GetDashboardDataProductQueryHandler : IRequestHandler<GetDashboardDataProductQuery, Result<List<DashboardDataProductResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IStringLocalizer<GetDashboardDataProductQueryHandler> _localizer;

        public GetDashboardDataProductQueryHandler(IUnitOfWork<int> unitOfWork, IUserService userService, IRoleService roleService, IStringLocalizer<GetDashboardDataProductQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _roleService = roleService;
            _localizer = localizer;
        }

        public async Task<Result<List<DashboardDataProductResponse>>> Handle(GetDashboardDataProductQuery query, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.Repository<Product>().Entities
               .Include(p => p.Kind)
               .Include(p => p.Group)
               .Include(p => p.ProductCategory)
               .ToListAsync(cancellationToken);

            // نجمع المنتجات حسب الكود
            var grouped = products
                .GroupBy(p => p.Code)
                .Select(g =>
                {
                    var first = g.First(); // نأخذ أول منتج كمرجع للبيانات العامة

                    // قائمة الأنواع والكميات لكل كود
                    var kinds = g.GroupBy(x => new
                    {
                        NameAr = x.Kind?.NameAr ?? "بدون نوع",
                        NameEn = x.Kind?.NameEn ?? "No Kind"
                    }).Select(k => new
                                 {
                                    KindNameAr = k.Key.NameAr,
                                    KindNameEn = k.Key.NameEn,
                                    TotalQty = k.Sum(x => x.Qty)
                                 })
                                 .ToList();

                    // نُنشئ نصاً لعرض الأنواع والكميات داخل الـ Dashboard
                    var kindsText = string.Join("", kinds.Select((k, i) =>
                    {
                        var separator = i < kinds.Count - 1 ? "<span style='color:red;'> | </span>" : "";
                        return $"{k.KindNameAr}: {k.TotalQty}{separator}";
                    }));

                    // نُنشئ نصاً لعرض الأنواع والكميات داخل الـ Dashboard
                    var kindsTextEn = string.Join("", kinds.Select((k, i) =>
                    {
                        var separator = i < kinds.Count - 1 ? "<span style='color:red;'> | </span>" : "";
                        return $"{k.KindNameEn}: {k.TotalQty}{separator}";
                    }));

                    return new DashboardDataProductResponse
                    {
                        Id = first.Id,
                        Code = first.Code,
                        Type = first.Type,
                        CategoryName = first.ProductCategory?.NameAr,
                        CategoryNameEn = first.ProductCategory?.NameEn,
                        GroupName = first.Group?.NameAr,
                        GroupNameEn = first.Group?.NameEn,
                        KindNameEn = kindsTextEn,
                        KindName = kindsText, // عرض الكميات لكل نوع بشكل مجمع
                        ImageProduct = first.ProductImageUrl,
                        Qty = g.Sum(x => x.Qty) // مجموع الكمية لكل الكود
                    };
                })
                .ToList();
            return await Result<List<DashboardDataProductResponse>>.SuccessAsync(grouped);
        }
    }
}