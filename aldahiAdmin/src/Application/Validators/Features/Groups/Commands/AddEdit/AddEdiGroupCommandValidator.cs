using FluentValidation;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Features.Groups.Commands.AddEdit;

namespace FirstCall.Application.Validators.Features.Groups.Commands.AddEdit
{
    public class AddEditGroupCommandValidator : AbstractValidator<AddEditGroupCommand>
    {
        public AddEditGroupCommandValidator(IStringLocalizer<AddEditGroupCommandValidator> localizer)
        {
			RuleFor(request => request.NameAr)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Arabic Name is required!"]);
           // RuleFor(request => request.NameEn)
           //.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["English Name is required!"]);
          
        }
    }
}