using MediatR;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.Clients;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Features.Orders.Commands.Delete
{
    public class DeleteDeliveryOrderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    internal class DeleteDeliveryOrderCommandHandler : IRequestHandler<DeleteDeliveryOrderCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteDeliveryOrderCommandHandler> _localizer;

        public DeleteDeliveryOrderCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteDeliveryOrderCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteDeliveryOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Repository<DeliveryOrder>().GetByIdAsync(command.Id);
            
            if (order != null)
            {
                order.IsDeleted = true;
               
                await _unitOfWork.Repository<DeliveryOrder>().UpdateAsync(order);
               
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDeliveryOrdersCacheKey);
                return await Result<int>.SuccessAsync(order.Id, _localizer["DeliveryOrder Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["DeliveryOrder Not Found!"]);
            }
        }
    }
}
