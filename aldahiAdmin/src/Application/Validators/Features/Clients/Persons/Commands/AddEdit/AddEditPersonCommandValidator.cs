using FluentValidation;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Features.Clients.Persons.Commands.AddEdit;
using FirstCall.Shared.Constants.Clients;
using System.Text.RegularExpressions;

namespace FirstCall.Application.Validators.Features.Clients.Persons.Commands.AddEdit
{
    public class AddEditPersonCommandValidator : AbstractValidator<AddEditPersonCommand>
    {
        public AddEditPersonCommandValidator(IStringLocalizer<AddEditPersonCommandValidator> localizer)
        {
            //RuleFor(request => request.Mobile)
            //   .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Mobile is required!"]);


            RuleFor(request => request.FullName)
               .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Arabic Name is required!"]);


        }
    }
}