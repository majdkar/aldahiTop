using AutoMapper;
using FirstCall.Application.Features.DocumentTypes.Commands.AddEdit;
using FirstCall.Application.Features.DocumentTypes.Queries.GetAll;
using FirstCall.Application.Features.DocumentTypes.Queries.GetById;
using FirstCall.Domain.Entities.Misc;

namespace FirstCall.Application.Mappings
{
    public class DocumentTypeProfile : Profile
    {
        public DocumentTypeProfile()
        {
            CreateMap<AddEditDocumentTypeCommand, DocumentType>().ReverseMap();
            CreateMap<GetDocumentTypeByIdResponse, DocumentType>().ReverseMap();
            CreateMap<GetAllDocumentTypesResponse, DocumentType>().ReverseMap();
        }
    }
}