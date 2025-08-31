using AutoMapper;
using FirstCall.Application.Features.Stocks.Commands.AddEdit;
using FirstCall.Application.Features.Stocks.Queries.GetAll;
using FirstCall.Application.Features.Stocks.Queries.GetById;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Mappings
{
    public class StockProfile : Profile
    {
        public StockProfile()
        {
            CreateMap<AddEditStockCommand, Stock>().ReverseMap();
            CreateMap<GetStockByIdResponse, Stock>().ReverseMap();
            CreateMap<GetAllStocksResponse, Stock>().ReverseMap();
        }
    }
}
