using FirstCall.Application.Features.ProductCategories.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;


namespace FirstCall.Application.Validators.Features.ProductCategories.Commands.AddEdit
{
    public class AddEditProductCategoryCommandValidator : AbstractValidator<AddEditProductCategoryCommand>
    {
        public AddEditProductCategoryCommandValidator(IStringLocalizer<AddEditProductCategoryCommandValidator> localizer)
        {
            RuleFor(request => request.NameEn)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["English Name is required!"]);
            RuleFor(request => request.NameAr)
               .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Arabic Name is required!"]);
            //RuleFor(request => request.Order)
            //    .GreaterThan(0).WithMessage(x => localizer["Order must be greater than 0"]);
        }
    }
}
