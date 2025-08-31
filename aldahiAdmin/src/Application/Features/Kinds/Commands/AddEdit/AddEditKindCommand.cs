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

namespace FirstCall.Application.Features.Kinds.Commands.AddEdit
{
    public partial class AddEditKindCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }

    internal class AddEditKindCommandHandler : IRequestHandler<AddEditKindCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditKindCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditKindCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditKindCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditKindCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var Kind = _mapper.Map<Kind>(command);
                await _unitOfWork.Repository<Kind>().AddAsync(Kind);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllKindsCacheKey);
                return await Result<int>.SuccessAsync(Kind.Id, _localizer["Kind Saved"]);
            }
            else
            {
                var Kind = await _unitOfWork.Repository<Kind>().GetByIdAsync(command.Id);
                if (Kind != null) 
                {
                    Kind.NameAr = command.NameAr ?? Kind.NameAr;
                    Kind.NameEn = command.NameEn ?? Kind.NameEn;                        ;
                    await _unitOfWork.Repository<Kind>().UpdateAsync(Kind);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllKindsCacheKey);
                    return await Result<int>.SuccessAsync(Kind.Id, _localizer["Kind Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Kind Not Found!"]);
                }
            }
        }
    }
}