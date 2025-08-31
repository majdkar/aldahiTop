using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using FirstCall.Shared.Constants.Application;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Kinds.Commands.Delete
{
    public class DeleteKindCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteKindCommandHandler : IRequestHandler<DeleteKindCommand, Result<int>>
    {
        //private readonly IKindRepository _KindRepository;
        private readonly IStringLocalizer<DeleteKindCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteKindCommandHandler(IUnitOfWork<int> unitOfWork,  IStringLocalizer<DeleteKindCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            //_KindRepository = KindRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteKindCommand command, CancellationToken cancellationToken)
        {
            var Kind = await _unitOfWork.Repository<Kind>().GetByIdAsync(command.Id);
            if (Kind != null)
            {
                Kind.IsDeleted = true;
                await _unitOfWork.Repository<Kind>().UpdateAsync(Kind);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllKindsCacheKey);
                return await Result<int>.SuccessAsync(Kind.Id, _localizer["Kind Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Kind Not Found!"]);
            }
        }
    }
}