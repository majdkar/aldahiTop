using AutoMapper;

using FirstCall.Application.Features.DeliveryOrders.Queries.GetById;

using FirstCall.Application.Features.Orders.Commands.AddEdit;
using FirstCall.Application.Features.Orders.Queries.GetAll;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Mappings
{
    public class DeliveryOrderProfile:Profile
    {
        public DeliveryOrderProfile()
        {
            CreateMap<AddEditDeliveryOrderCommand, DeliveryOrder>().ReverseMap();
            CreateMap<GetDeliveryOrderByIdResponse, DeliveryOrder>().ReverseMap();
            CreateMap<GetAllDeliveryOrdersResponse, DeliveryOrder>().ReverseMap();
        }
    }
}
