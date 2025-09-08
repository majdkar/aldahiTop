using MediatR;
using FirstCall.Application.Extensions;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Specifications.Catalog;
using FirstCall.Application.Specifications.Products;

using FirstCall.Domain.Entities.Products;
using FirstCall.Shared.Wrapper;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Constants.Products;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using FirstCall.Shared.Models;
using FirstCall.Application.Interfaces.Services;
using Telegram.Bot.Types;

namespace FirstCall.Application.Features.Products.Queries.GetAllPaged
{
    public class GetAllProductsPdfQuery : IRequest<Result<string>>
    {
        public string Code { get; set; }
        public string ProductType { get; set; }
        public int SeasonId { get; set; }
        public int KindId { get; set; }
        public int GroupId { get; set; }
        public int WarehousesId { get; set; }
        public int ProductCategoryId { get; set; }

        public int FromQty { get; set; }
        public int ToQty { get; set; }


        public GetAllProductsPdfQuery(int seasonId, int kindId, int groupId, int warehousesId, int productCategoryId, string code, int fromqty, int toqty, string productType)
        {

            SeasonId = seasonId;
            GroupId = groupId;
            WarehousesId = warehousesId;
            ProductType = productType;
            Code = code;
            FromQty = fromqty;
            ToQty = toqty;
            KindId = kindId;
            ProductCategoryId = productCategoryId;
        }
    }

    internal class GetAllProductsPdfQueryHandler : IRequestHandler<GetAllProductsPdfQuery, Result<string>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IGenericPdfService _GenericPdfService;

        public GetAllProductsPdfQueryHandler(IUnitOfWork<int> unitOfWork, IGenericPdfService GenericPdfService)
        {
            _unitOfWork = unitOfWork;
            _GenericPdfService = GenericPdfService;
        }

        public async Task<Result<string>> Handle(GetAllProductsPdfQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Product, GetAllPagedProductsResponse>> expression = e => new GetAllPagedProductsResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                Price = e.Price,
                Code = e.Code,
                ProductCategoryId = e.ProductCategoryId,
                PackageNumber = e.PackageNumber,
                Warehouses = e.Warehouses,
                WarehousesId = e.WarehousesId,
                Qty = e.Qty,
                Kind = e.Kind,
                ProductCategory = e.ProductCategory,
                Sizes = e.Sizes,
                Colors = e.Colors,
                KindId = e.KindId,
                SeasonId = e.SeasonId,
                Season = e.Season,
                GroupId = e.GroupId,
                Group = e.Group,
                KindNameAr = e.Kind.NameAr,
                Type = e.Type,
            };
            var productFilterSpec = new ProductSearchAdvancedFilterSpecification(request.SeasonId, request.KindId, request.GroupId, request.WarehousesId, request.ProductCategoryId, request.FromQty, request.ToQty, request.Code, request.ProductType);

            var data = await _unitOfWork.Repository<Product>().Entities
               .Specify(productFilterSpec).OrderBy(x => x.Order)
               .Select(expression).ToListAsync();



            // إعداد الأعمدة
            var columns = new List<PdfColumn<GetAllPagedProductsResponse>>
        {
            new() { Header = "Code", Selector = p => p.Code },
            new() { Header = "Name", Selector = p =>   $"{p.ProductCategory.NameAr} {p.KindNameAr} {p.Season.NameAr} {p.Group.NameAr}" },
            new() { Header = "Qty", Selector = p => p.Qty }
        };

            // توليد PDF
            var pdfBytes = _GenericPdfService.GeneratePdf(data, columns, "Products Report");

            // حفظ الملف في wwwroot
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "Reports");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"ProductsReport_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var filePath = Path.Combine(folderPath, fileName);

            await File.WriteAllBytesAsync(filePath, pdfBytes, cancellationToken);

            string PdfUrl = $"/Files/Reports/{fileName}" ;

            var url = PdfUrl.Remove(0, 1);
            return await Result<string>.SuccessAsync(data: url);
        }

    }
}
