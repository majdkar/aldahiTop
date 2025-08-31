
using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Application.Features.Stocks.Commands.AddEdit;
using FirstCall.Application.Features.Stocks.Queries.GetAll;
using FirstCall.Application.Requests.Brand;
using FirstCall.Application.Requests.Stocks;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Stock
{
    public interface IStockManager:IManager
    {
        Task<PaginatedResult<GetAllStocksResponse>> GetAllPagedAsync(GetAllPagedStockRequest request);
        Task<IResult<List<GetAllStocksResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditStockCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}
