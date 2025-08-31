using System;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using FirstCall.Shared.Constants.Application;

namespace FirstCall.Application.Features.Princedoms.Commands.Delete
{
    public class DeletePrincedomCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeletePrincedomCommandHandler : IRequestHandler<DeletePrincedomCommand, Result<int>>
    {
        private readonly IStringLocalizer<DeletePrincedomCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeletePrincedomCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeletePrincedomCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeletePrincedomCommand command, CancellationToken cancellationToken)
        {
            var isPrincedomUsed = false;// await _productRepository.IsPrincedomUsed(command.Id);
            if (!isPrincedomUsed)
            {
                var princedom = await _unitOfWork.Repository<Princedom>().GetByIdAsync(command.Id);
                if (princedom != null)
                {
                    await _unitOfWork.Repository<Princedom>().DeleteAsync(princedom);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPrincedomsCacheKey);
                    return await Result<int>.SuccessAsync(princedom.Id, _localizer["Princedom Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Princedom Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}