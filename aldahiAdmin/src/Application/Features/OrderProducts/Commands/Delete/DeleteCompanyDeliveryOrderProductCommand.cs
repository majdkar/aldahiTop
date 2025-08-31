using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.Orders;
using FirstCall.Shared.Wrapper;
using static FirstCall.Shared.Constants.Permission.Permissions;

namespace FirstCall.Application.Features.Orders.Commands.Delete
{
    public class DeleteDeliveryOrderProductCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteDeliveryOrderProductCommandHandler : IRequestHandler<DeleteDeliveryOrderProductCommand, Result<int>>
    {
        private readonly IStringLocalizer<DeleteDeliveryOrderProductCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteDeliveryOrderProductCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteDeliveryOrderProductCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteDeliveryOrderProductCommand command, CancellationToken cancellationToken)
        {
            var deliveryOrderProduct = await _unitOfWork.Repository<DeliveryOrderProduct>().GetByIdAsync(command.Id);
            if (deliveryOrderProduct != null)
            {
                var deliveryOrderId = deliveryOrderProduct.DeliveryOrderId;

                await _unitOfWork.Repository<DeliveryOrderProduct>().DeleteAsync(deliveryOrderProduct);
                await _unitOfWork.Commit(cancellationToken);
                var order = _unitOfWork.Repository<DeliveryOrder>().GetAllAsync().Result.Where(s => s.Id == deliveryOrderProduct.DeliveryOrderId).FirstOrDefault();
                if (order != null)
                {
                    //var price = _unitOfWork.Repository<DeliveryOrderProduct>().GetAllAsync().Result.Where(q => q.DeliveryOrderId == deliveryOrderId).Sum(s => s.Quantity * s.UnitPrice);
                    //order.TotalPrice = price;
                    await _unitOfWork.Repository<DeliveryOrder>().UpdateAsync(order);
                    await _unitOfWork.Commit(cancellationToken);
                    //var bill = _unitOfWork.Repository<Bill>().GetAllAsync().Result.Where(s => s.DeliveryOrderId == deliveryOrderId).FirstOrDefault();
                    //if (bill != null)
                    //{
                    //    bill.ToltalPrice = price;
                    //    await _unitOfWork.Repository<Bill>().UpdateAsync(bill);
                    //    await _unitOfWork.Commit(cancellationToken);
                    //}
                }
                return await Result<int>.SuccessAsync(deliveryOrderProduct.Id, _localizer["DeliveryOrderProduct Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["DeliveryOrderProduct Not Found!"]);
            }
        }
    }

}

