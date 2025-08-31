using FirstCall.Application.Features.Documents.Commands.AddEdit;
using FirstCall.Application.Features.Documents.Queries.GetAll;
using FirstCall.Application.Requests.Documents;
using FirstCall.Shared.Wrapper;
using System.Threading.Tasks;
using FirstCall.Application.Features.Documents.Queries.GetById;

namespace FirstCall.Client.Infrastructure.Managers.Misc.Document
{
    public interface IDocumentManager : IManager
    {
        Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request);

        Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(GetDocumentByIdQuery request);

        Task<IResult<int>> SaveAsync(AddEditDocumentCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}