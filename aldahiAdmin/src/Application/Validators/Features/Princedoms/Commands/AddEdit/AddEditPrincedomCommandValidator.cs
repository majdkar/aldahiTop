using System;
using FirstCall.Application.Features.Princedoms.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FirstCall.Application.Validators.Features.Princedoms.Commands.AddEdit
{
    public class AddEditPrincedomCommandValidator : AbstractValidator<AddEditPrincedomCommand>
    {
        public AddEditPrincedomCommandValidator(IStringLocalizer<AddEditPrincedomCommandValidator> localizer)
        {
			//RuleFor(request => request.Name)
   //             .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
   //         RuleFor(request => request.Description)
   //             .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
			
    RuleFor(request => request.ar_title)
    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["ar_title is required!"]); 
    RuleFor(request => request.en_title)
    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["en_title is required!"]); 
//RuleFor(request => request.Code)
//.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Code is required!"]); 
            
            //RuleFor(request => request.Tax)
            //    .GreaterThan(0).WithMessage(x => localizer["Tax must be greater than 0"]);
        }
    }
}