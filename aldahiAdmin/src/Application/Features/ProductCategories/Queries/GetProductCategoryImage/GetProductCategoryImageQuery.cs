using MediatR;
using Microsoft.EntityFrameworkCore;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Core.Entities;
using FirstCall.Shared.Wrapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.ProductCategories.Queries.GetProductCategoryImage
{
    public class GetProductCategoryImageQuery : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public GetProductCategoryImageQuery(int productCategoryId)
        {
            Id = productCategoryId;
        }
    }

    internal class GetProductCategoryImageQueryHandler : IRequestHandler<GetProductCategoryImageQuery, Result<string>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetProductCategoryImageQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(GetProductCategoryImageQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<ProductCategory>().Entities.Where(p => p.Id == request.Id).Select(a => a.ImageDataURL).FirstOrDefaultAsync(cancellationToken);
            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
