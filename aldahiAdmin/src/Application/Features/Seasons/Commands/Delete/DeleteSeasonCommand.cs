using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using FirstCall.Shared.Constants.Application;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Seasons.Commands.Delete
{
    public class DeleteSeasonCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteSeasonCommandHandler : IRequestHandler<DeleteSeasonCommand, Result<int>>
    {
        //private readonly ISeasonRepository _SeasonRepository;
        private readonly IStringLocalizer<DeleteSeasonCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteSeasonCommandHandler(IUnitOfWork<int> unitOfWork,  IStringLocalizer<DeleteSeasonCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            //_SeasonRepository = SeasonRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteSeasonCommand command, CancellationToken cancellationToken)
        {
            var Season = await _unitOfWork.Repository<Season>().GetByIdAsync(command.Id);
            if (Season != null)
            {
                Season.IsDeleted = true;
                await _unitOfWork.Repository<Season>().UpdateAsync(Season);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllSeasonsCacheKey);
                return await Result<int>.SuccessAsync(Season.Id, _localizer["Season Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Season Not Found!"]);
            }
        }
    }
}