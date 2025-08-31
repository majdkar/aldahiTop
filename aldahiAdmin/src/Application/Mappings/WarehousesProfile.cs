using AutoMapper;
using FirstCall.Application.Features.Warehousess.Commands.AddEdit;
using FirstCall.Application.Features.Warehousess.Queries.GetAll;
using FirstCall.Application.Features.Warehousess.Queries.GetById;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Mappings
{
    public class WarehousesProfile : Profile
    {
        public WarehousesProfile()
        {
            CreateMap<AddEditWarehousesCommand, Warehouses>().ReverseMap();
            CreateMap<GetWarehousesByIdResponse, Warehouses>().ReverseMap();
            CreateMap<GetAllWarehousessResponse, Warehouses>().ReverseMap();
        }
    }
}
