using FirstCall.Application.Features.Orders.Commands.AddEdit;
using FirstCall.Application.Features.Orders.Queries.GetAll;
using FirstCall.Application.Features.DeliveryOrders.Queries.GetById;
using FirstCall.Application.Requests.Deliveries.DeliveryOrders;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstCall.Application.Features.OrderProducts.Queries.GetAllByOrder;
using FirstCall.Application.Requests.Deliveries.DeliveryOrderProducts;
using FirstCall.Application.Features.Products.Queries.GetById;
using FirstCall.Application.Features.Products.Commands.AddEdit;
using FirstCall.Application.Features.OrderProducts.Commands.AddEdit;

namespace FirstCall.Client.Infrastructure.Managers.Deliveries.DeliveryOrder
{
    public interface IDeliveryOrderProductManager:IManager
    {
        
        Task<IResult<List<GetAllDeliveryOrderProductsResponse>>> GetAllAsync();
        Task<PaginatedResult<GetAllDeliveryOrderProductsResponse>> GetDeliveryOrderProductsAsync(GetAllDeliveryOrderProductsRequest request);
        Task<IResult<int>> SaveAsync(AddEditDeliveryOrderProductCommand request);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
        Task<IResult<string>> GetDeliveryOrderProductAsync(int id);
    }
}
