using FirstCall.Application.Features.Nations.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FirstCall.Application.Validators.Features.Nations.Commands.AddEdit
{
    public class AddEditNationCommandValidator : AbstractValidator<AddEditNationCommand>
    {
        public AddEditNationCommandValidator(IStringLocalizer<AddEditNationCommandValidator> localizer)
        {
			RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
            			
RuleFor(request => request.ArabicName)
.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["ArabicName is required!"]); 
            
            //RuleFor(request => request.Tax)
            //    .GreaterThan(0).WithMessage(x => localizer["Tax must be greater than 0"]);
        }
    }
}