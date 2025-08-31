using AutoMapper;

using FirstCall.Application.Features.OrderProducts.Commands.AddEdit;
using FirstCall.Application.Features.OrderProducts.Queries.GetAllByOrder;
using FirstCall.Application.Features.OrderProducts.Queries.GetById;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Mappings
{
    public class DeliveryOrderProductProfile : Profile
    {
        public DeliveryOrderProductProfile()
        {
            CreateMap<AddEditDeliveryOrderProductCommand, DeliveryOrderProduct>().ReverseMap();
            CreateMap<GetDeliveryOrderProductByIdResponse, DeliveryOrderProduct>().ReverseMap();
            CreateMap<GetAllDeliveryOrderProductsResponse, DeliveryOrderProduct>().ReverseMap();
        }
    }
}
