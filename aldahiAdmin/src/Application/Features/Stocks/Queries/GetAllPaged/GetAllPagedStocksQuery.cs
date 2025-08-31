using MediatR;
using FirstCall.Application.Extensions;
using FirstCall.Application.Features.Stocks.Queries.GetAll;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Specifications;
using FirstCall.Core.Entities;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Wrapper;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Application.Specifications.GeneralSettings;

namespace FirstCall.Application.Features.Stocks.Queries.GetAllPaged
{
    public class GetAllPagedStocksQuery : IRequest<PaginatedResult<GetAllStocksResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; }

        public GetAllPagedStocksQuery(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }
    internal class GetAllStocksQueryHandler : IRequestHandler<GetAllPagedStocksQuery, PaginatedResult<GetAllStocksResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllStocksQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllStocksResponse>> Handle(GetAllPagedStocksQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Stock, GetAllStocksResponse>> expression = e => new GetAllStocksResponse
            {
                Id = e.Id,
                NameAr=e.NameAr,
                NameEn=e.NameEn,
                 WarehousesId = e.WarehousesId,
                 Product = e.Product,
                 ProductId = e.ProductId,
                 Quantity = e.Quantity,
                 Warehouses = e.Warehouses
                 

            };
            var StockFilterSpec = new StockFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Stock>().Entities
                   .Specify(StockFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Stock>().Entities
                   .Specify(StockFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
