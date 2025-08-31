using AutoMapper;
using FirstCall.Infrastructure.Models.Audit;
using FirstCall.Application.Responses.Audit;

namespace FirstCall.Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}