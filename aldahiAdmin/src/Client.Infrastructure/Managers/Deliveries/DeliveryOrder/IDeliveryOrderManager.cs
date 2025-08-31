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

namespace FirstCall.Client.Infrastructure.Managers.Deliveries.DeliveryOrder
{
    public interface IDeliveryOrderManager:IManager
    {
        Task<IResult<List<GetAllDeliveryOrdersResponse>>> GetAllAsync();

        Task<PaginatedResult<GetAllDeliveryOrdersResponse>> GetAllPagedAsync(GetAllPagedDeliveryOrdersRequest request);




        Task<PaginatedResult<GetAllDeliveryOrdersResponse>> GetAllPagedByClientAsync(GetAllPagedDeliveryOrdersRequest request, int clientId);
        Task<PaginatedResult<GetAllDeliveryOrdersResponse>> GetAllByStatusAsync(GetAllPagedDeliveryOrdersRequest request);


        Task<IResult<GetDeliveryOrderByIdResponse>> GetByIdAsync(int orderId);

        Task<IResult<int>> SaveAsync(AddEditDeliveryOrderCommand request);


        Task<IResult<int>> DeleteAsync(int id);


        //Task<IResult<int>> AcceptAsync(AcceptDeliveryOrderRequestCommand request);

        Task<IResult<int>> RefuseAsync(int id);

        Task<IResult<int>> AccceptOrderRequestAsync(int id);

        Task<IResult<int>> OrderCloseRequestAsync(int id);


        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}
