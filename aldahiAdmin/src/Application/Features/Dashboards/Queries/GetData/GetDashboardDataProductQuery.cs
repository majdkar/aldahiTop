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
using System.Collections.Generic;
using FirstCall.Domain.Entities.Products;


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
             var getProduct = await _unitOfWork.Repository<Product>().Entities.Include(p => p.Kind)
        .Include(p => p.Group)
        .Include(p => p.ProductCategory).ToListAsync();
            var response = getProduct.Select(item => new DashboardDataProductResponse
            {
                CategoryName = item.ProductCategory.NameAr,
                KindName = item.Kind.NameAr,
                GroupName = item.Group.NameAr,
                CategoryNameEn = item.ProductCategory.NameEn,
                KindNameEn = item.Kind.NameEn,
                GroupNameEn = item.Group.NameEn,
                ImageProduct = item.ProductImageUrl,
                Qty = item.Qty,
                 Id = item.Id,
                  Code = item.Code,
            }).ToList();
            return await Result<List<DashboardDataProductResponse>>.SuccessAsync(response);
        }
    }
}