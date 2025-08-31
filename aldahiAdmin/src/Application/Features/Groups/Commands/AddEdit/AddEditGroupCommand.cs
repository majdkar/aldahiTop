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

namespace FirstCall.Application.Features.Groups.Commands.AddEdit
{
    public partial class AddEditGroupCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }

    internal class AddEditGroupCommandHandler : IRequestHandler<AddEditGroupCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditGroupCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditGroupCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditGroupCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditGroupCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var Group = _mapper.Map<Group>(command);
                await _unitOfWork.Repository<Group>().AddAsync(Group);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllGroupsCacheKey);
                return await Result<int>.SuccessAsync(Group.Id, _localizer["Group Saved"]);
            }
            else
            {
                var Group = await _unitOfWork.Repository<Group>().GetByIdAsync(command.Id);
                if (Group != null) 
                {
                    Group.NameAr = command.NameAr ?? Group.NameAr;
                    Group.NameEn = command.NameEn ?? Group.NameEn;                        ;
                    await _unitOfWork.Repository<Group>().UpdateAsync(Group);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllGroupsCacheKey);
                    return await Result<int>.SuccessAsync(Group.Id, _localizer["Group Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Group Not Found!"]);
                }
            }
        }
    }
}