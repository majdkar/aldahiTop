using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Interfaces.Services;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Constants.Orders;
using FirstCall.Shared.Wrapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Features.Orders.Commands.AddEdit
{
    public class RefuseDeliveryOrderRequestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class RefuseDeliveryOrderRequestCommandHandler : IRequestHandler<RefuseDeliveryOrderRequestCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<RefuseDeliveryOrderRequestCommandHandler> _localizer;

        public RefuseDeliveryOrderRequestCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<RefuseDeliveryOrderRequestCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(RefuseDeliveryOrderRequestCommand command, CancellationToken cancellationToken)
        {
            var Deliveryorder = await _unitOfWork.Repository<DeliveryOrder>().GetByIdAsync(command.Id);
            if (Deliveryorder != null)
            {
                //Deliveryorder.Status = OrderStatusEnum.Canceled.ToString();
                await _unitOfWork.Repository<DeliveryOrder>().UpdateAsync(Deliveryorder);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCompaniesCacheKey);
                return await Result<int>.SuccessAsync(Deliveryorder.Id, _localizer["Delivery Order Refused"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Delivery Order Not Found!"]);
            }
        }
    }
}
