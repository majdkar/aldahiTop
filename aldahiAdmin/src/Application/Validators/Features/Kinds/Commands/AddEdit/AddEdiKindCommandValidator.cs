using FluentValidation;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Features.Kinds.Commands.AddEdit;

namespace FirstCall.Application.Validators.Features.Kinds.Commands.AddEdit
{
    public class AddEditKindCommandValidator : AbstractValidator<AddEditKindCommand>
    {
        public AddEditKindCommandValidator(IStringLocalizer<AddEditKindCommandValidator> localizer)
        {
			RuleFor(request => request.NameAr)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Arabic Name is required!"]);
           // RuleFor(request => request.NameEn)
           //.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["English Name is required!"]);
          
        }
    }
}