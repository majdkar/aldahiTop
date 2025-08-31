using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using FirstCall.Shared.Constants.Application;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Stocks.Commands.Delete
{
    public class DeleteStockCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteStockCommandHandler : IRequestHandler<DeleteStockCommand, Result<int>>
    {
        //private readonly IStockRepository _StockRepository;
        private readonly IStringLocalizer<DeleteStockCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteStockCommandHandler(IUnitOfWork<int> unitOfWork,  IStringLocalizer<DeleteStockCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            //_StockRepository = StockRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteStockCommand command, CancellationToken cancellationToken)
        {
            var Stock = await _unitOfWork.Repository<Stock>().GetByIdAsync(command.Id);
            if (Stock != null)
            {
                Stock.IsDeleted = true;
                await _unitOfWork.Repository<Stock>().UpdateAsync(Stock);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllStocksCacheKey);
                return await Result<int>.SuccessAsync(Stock.Id, _localizer["Stock Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Stock Not Found!"]);
            }
        }
    }
}