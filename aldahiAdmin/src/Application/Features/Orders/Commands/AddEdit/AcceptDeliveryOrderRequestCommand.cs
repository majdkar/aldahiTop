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
using System.Linq;
using FirstCall.Core.Entities;

namespace FirstCall.Application.Features.Orders.Commands.AddEdit
{
    public class AcceptDeliveryOrderRequestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }


    }

    internal class AcceptDeliveryOrderRequestCommandHandler : IRequestHandler<AcceptDeliveryOrderRequestCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AcceptDeliveryOrderRequestCommandHandler> _localizer;

        public AcceptDeliveryOrderRequestCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<AcceptDeliveryOrderRequestCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AcceptDeliveryOrderRequestCommand command, CancellationToken cancellationToken)
        {
            var Deliveryorder = await _unitOfWork.Repository<DeliveryOrder>().GetByIdAsync(command.Id);
            if (Deliveryorder != null)
            {
                //Deliveryorder.Status = OrderStatusEnum.Accepted.ToString();
                await _unitOfWork.Repository<DeliveryOrder>().UpdateAsync(Deliveryorder);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDeliveryOrdersCacheKey);
                var maxBillNo = 0;
                //var bills = await _unitOfWork.Repository<Bill>().GetAllAsync();
                //if (bills.Count > 0)
                //    maxBillNo = bills.Max(e => e.Number);
                //var bill = new Bill
                //{
                //    ClientId = Deliveryorder.ClientId,
                //    DeliveryOrderId = Deliveryorder.Id,
                //    //UnitPrice = event1.Price,
                //    ToltalPrice = Deliveryorder.TotalPrice,
                //    Number = maxBillNo + 1,
                //    BillNumber = "MC-" + (maxBillNo + 1),
                //    BillDate = DateTime.Now,
                //    //IsDeletedByClient = false,
                //    Status = "unpaid",

                //};
                //await _unitOfWork.Repository<Bill>().AddAsync(bill);
                //await _unitOfWork.Commit(cancellationToken);
                
                return await Result<int>.SuccessAsync(Deliveryorder.Id, _localizer["Delivery Order Accepted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Delivery Order Not Found!"]);
            }
        }
    }
}
