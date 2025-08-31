using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using FirstCall.Shared.Constants.Application;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Seasons.Commands.AddEdit
{
    public partial class AddEditSeasonCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }

    internal class AddEditSeasonCommandHandler : IRequestHandler<AddEditSeasonCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditSeasonCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditSeasonCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditSeasonCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditSeasonCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var Season = _mapper.Map<Season>(command);
                await _unitOfWork.Repository<Season>().AddAsync(Season);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllSeasonsCacheKey);
                return await Result<int>.SuccessAsync(Season.Id, _localizer["Season Saved"]);
            }
            else
            {
                var Season = await _unitOfWork.Repository<Season>().GetByIdAsync(command.Id);
                if (Season != null) 
                {
                    Season.NameAr = command.NameAr ?? Season.NameAr;
                    Season.NameEn = command.NameEn ?? Season.NameEn;                        ;
                    await _unitOfWork.Repository<Season>().UpdateAsync(Season);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllSeasonsCacheKey);
                    return await Result<int>.SuccessAsync(Season.Id, _localizer["Season Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Season Not Found!"]);
                }
            }
        }
    }
}