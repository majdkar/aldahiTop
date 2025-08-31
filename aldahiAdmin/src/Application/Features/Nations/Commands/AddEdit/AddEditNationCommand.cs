using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using FirstCall.Shared.Constants.Application;

namespace FirstCall.Application.Features.Nations.Commands.AddEdit
{
    public partial class AddEditNationCommand : IRequest<Result<int>>
    {
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		
        public string Description { get; set; }
		
		[Required]
		public string ArabicName { get; set; }
		
		public string Code { get; set; }
	
		public string PhoneCode { get; set; }

        //[Required]
        //public decimal Tax { get; set; }
    }

    internal class AddEditNationCommandHandler : IRequestHandler<AddEditNationCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditNationCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditNationCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditNationCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditNationCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var nation = _mapper.Map<Nation>(command);
                await _unitOfWork.Repository<Nation>().AddAsync(nation);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllNationsCacheKey);
                return await Result<int>.SuccessAsync(nation.Id, _localizer["Nation Saved"]);
            }
            else
            {
                var nation = await _unitOfWork.Repository<Nation>().GetByIdAsync(command.Id);
                if (nation != null)
                {
					nation.Name = command.Name ?? nation.Name;
					nation.Description = command.Description ?? nation.Description;
					
					nation.ArabicName = command.ArabicName ?? nation.ArabicName;
					nation.Code = command.Code ?? nation.Code;
					nation.PhoneCode = command.PhoneCode ?? nation.PhoneCode;
                    //nation.Tax = (command.Tax == 0) ? nation.Tax : command.Tax;
                    await _unitOfWork.Repository<Nation>().UpdateAsync(nation);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllNationsCacheKey);
                    return await Result<int>.SuccessAsync(nation.Id, _localizer["Nation Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Nation Not Found!"]);
                }
            }
        }
    }
}