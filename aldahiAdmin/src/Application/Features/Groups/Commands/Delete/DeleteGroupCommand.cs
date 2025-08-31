using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using FirstCall.Shared.Constants.Application;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Groups.Commands.Delete
{
    public class DeleteGroupCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Result<int>>
    {
        //private readonly IGroupRepository _GroupRepository;
        private readonly IStringLocalizer<DeleteGroupCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteGroupCommandHandler(IUnitOfWork<int> unitOfWork,  IStringLocalizer<DeleteGroupCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            //_GroupRepository = GroupRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteGroupCommand command, CancellationToken cancellationToken)
        {
            var Group = await _unitOfWork.Repository<Group>().GetByIdAsync(command.Id);
            if (Group != null)
            {
                Group.IsDeleted = true;
                await _unitOfWork.Repository<Group>().UpdateAsync(Group);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllGroupsCacheKey);
                return await Result<int>.SuccessAsync(Group.Id, _localizer["Group Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Group Not Found!"]);
            }
        }
    }
}