using FirstCall.Application.Features.Products.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Features.OrderProducts.Commands.AddEdit;
using FirstCall.Application.Features.Orders.Commands.AddEdit;

namespace FirstCall.Application.Validators.Features.Products.Commands.AddEdit
{
    public class AddEditDeliveryOrderCommandValidator : AbstractValidator<AddEditDeliveryOrderCommand>
    {
        public AddEditDeliveryOrderCommandValidator(IStringLocalizer<AddEditDeliveryOrderCommand> localizer, IStringLocalizer<AddEditDeliveryOrderProductCommandValidator> _localizer)
        {
             RuleFor(request => request.OrderNumber).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["OrderNumber is required!"]);
            RuleForEach(request => request.ChargesCommand).SetValidator(new AddEditDeliveryOrderProductCommandValidator(_localizer));

        }
    }
}