using FluentValidation;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Features.Seasons.Commands.AddEdit;

namespace FirstCall.Application.Validators.Features.Seasons.Commands.AddEdit
{
    public class AddEditSeasonCommandValidator : AbstractValidator<AddEditSeasonCommand>
    {
        public AddEditSeasonCommandValidator(IStringLocalizer<AddEditSeasonCommandValidator> localizer)
        {
			RuleFor(request => request.NameAr)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Arabic Name is required!"]);
           // RuleFor(request => request.NameEn)
           //.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["English Name is required!"]);
          
        }
    }
}