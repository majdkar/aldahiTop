using MediatR;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.Products;
using FirstCall.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.Products.Commands.Delete
{
    public class DeleteProductComponentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteProductComponentCommandHandler : IRequestHandler<DeleteProductComponentCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteProductComponentCommandHandler> _localizer;

        public DeleteProductComponentCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteProductComponentCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteProductComponentCommand command, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<ProductCom>().GetByIdAsync(command.Id);
            if (product != null)
            {
                product.IsDeleted = true;
                await _unitOfWork.Repository<ProductCom>().UpdateAsync(product);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(product.Id, _localizer["Product Component Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Product Component Not Found!"]);
            }
        }
    }
}