using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using FirstCall.Shared.Constants.Application;

namespace FirstCall.Application.Features.Nations.Commands.Delete
{
    public class DeleteNationCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteNationCommandHandler : IRequestHandler<DeleteNationCommand, Result<int>>
    {
       
        private readonly IStringLocalizer<DeleteNationCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteNationCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteNationCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
         
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteNationCommand command, CancellationToken cancellationToken)
        {
            var isNationUsed = false;// await _productRepository.IsNationUsed(command.Id);
            if (!isNationUsed)
            {
                var nation = await _unitOfWork.Repository<Nation>().GetByIdAsync(command.Id);
                if (nation != null)
                {
                    await _unitOfWork.Repository<Nation>().DeleteAsync(nation);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllNationsCacheKey);
                    return await Result<int>.SuccessAsync(nation.Id, _localizer["Nation Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Nation Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}