using FirstCall.Application.Features.Products.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Features.OrderProducts.Commands.AddEdit;

namespace FirstCall.Application.Validators.Features.Products.Commands.AddEdit
{
    public class AddEditDeliveryOrderProductCommandValidator : AbstractValidator<AddEditDeliveryOrderProductCommand>
    {
        public AddEditDeliveryOrderProductCommandValidator(IStringLocalizer<AddEditDeliveryOrderProductCommandValidator> localizer)
        {
             RuleFor(request => request.UnitPrice)
                .GreaterThanOrEqualTo(1).WithMessage(x => localizer["Price is required!"]);
            RuleFor(request => request.Quantity)
                .GreaterThanOrEqualTo(1).WithMessage(x => localizer["Quantity is required!"]);
        }
    }
}