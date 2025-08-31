using AutoMapper;
using FirstCall.Application.Features.Documents.Commands.AddEdit;
using FirstCall.Application.Features.Documents.Queries.GetById;
using FirstCall.Domain.Entities.Misc;

namespace FirstCall.Application.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<AddEditDocumentCommand, Document>().ReverseMap();
            CreateMap<GetDocumentByIdResponse, Document>().ReverseMap();
        }
    }
}