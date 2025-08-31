using FluentValidation;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Features.Countries.Commands.AddEdit;

namespace FirstCall.Application.Validators.Features.Countries.Commands.AddEdit
{
    public class AddEditCountryCommandValidator : AbstractValidator<AddEditCountryCommand>
    {
        public AddEditCountryCommandValidator(IStringLocalizer<AddEditCountryCommandValidator> localizer)
        {
            RuleFor(request => request.NameAr)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Arabic Name is required!"]);

            RuleFor(request => request.NameEn)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["English Name is required!"]);

            //RuleFor(request => request.Code)
            //    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Code is required!"]);
        }
    }
}
