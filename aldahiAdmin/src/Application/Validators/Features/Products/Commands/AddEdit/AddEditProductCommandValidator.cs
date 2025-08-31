using FirstCall.Application.Features.Products.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FirstCall.Application.Validators.Features.Products.Commands.AddEdit
{
    public class AddEditProductCommandValidator : AbstractValidator<AddEditCompanyProductCommand>
    {
        public AddEditProductCommandValidator(IStringLocalizer<AddEditProductCommandValidator> localizer)
        {
            RuleFor(request => request.NameAr)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Arabic Name is required!"]);

                RuleFor(request => request.Code)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Code is required!"]);

            RuleFor(request => request.ProductCategoryId).NotNull().NotEmpty()
            .WithMessage(x => localizer["Category is required!"]);


            RuleFor(request => request.KindId).NotNull().NotEmpty()
            .WithMessage(x => localizer["Kind is required!"]);
        }
    }
}