using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using FirstCall.Shared.Constants.Application;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Warehousess.Commands.Delete
{
    public class DeleteWarehousesCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteWarehousesCommandHandler : IRequestHandler<DeleteWarehousesCommand, Result<int>>
    {
        //private readonly IWarehousesRepository _WarehousesRepository;
        private readonly IStringLocalizer<DeleteWarehousesCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteWarehousesCommandHandler(IUnitOfWork<int> unitOfWork,  IStringLocalizer<DeleteWarehousesCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            //_WarehousesRepository = WarehousesRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteWarehousesCommand command, CancellationToken cancellationToken)
        {
            var Warehouses = await _unitOfWork.Repository<Warehouses>().GetByIdAsync(command.Id);
            if (Warehouses != null)
            {
                Warehouses.IsDeleted = true;
                await _unitOfWork.Repository<Warehouses>().UpdateAsync(Warehouses);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWarehousessCacheKey);
                return await Result<int>.SuccessAsync(Warehouses.Id, _localizer["Warehouses Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Warehouses Not Found!"]);
            }
        }
    }
}