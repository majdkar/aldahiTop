using System;
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

namespace FirstCall.Application.Features.Princedoms.Commands.AddEdit
{
    public partial class AddEditPrincedomCommand : IRequest<Result<int>>
    {
		public int Id { get; set; }
		
		public string Name { get; set; }
		
        public string Description { get; set; }
		
		[Required]
		public string ar_title { get; set; }
		[Required]
		public string en_title { get; set; }
		
		public string Code { get; set; }

        //[Required]
        //public decimal Tax { get; set; }
    }

    internal class AddEditPrincedomCommandHandler : IRequestHandler<AddEditPrincedomCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditPrincedomCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditPrincedomCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditPrincedomCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditPrincedomCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var princedom = _mapper.Map<Princedom>(command);
                await _unitOfWork.Repository<Princedom>().AddAsync(princedom);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPrincedomsCacheKey);
                return await Result<int>.SuccessAsync(princedom.Id, _localizer["Princedom Saved"]);
            }
            else
            {
                var princedom = await _unitOfWork.Repository<Princedom>().GetByIdAsync(command.Id);
                if (princedom != null)
                {
					princedom.Name = command.Name ?? princedom.Name;
					princedom.Description = command.Description ?? princedom.Description;
					
					princedom.ar_title = command.ar_title ?? princedom.ar_title;
					princedom.en_title = command.en_title ?? princedom.en_title;
					princedom.Code = command.Code ?? princedom.Code;
                    //princedom.Tax = (command.Tax == 0) ? princedom.Tax : command.Tax;
                    await _unitOfWork.Repository<Princedom>().UpdateAsync(princedom);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPrincedomsCacheKey);
                    return await Result<int>.SuccessAsync(princedom.Id, _localizer["Princedom Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Princedom Not Found!"]);
                }
            }
        }
    }
}