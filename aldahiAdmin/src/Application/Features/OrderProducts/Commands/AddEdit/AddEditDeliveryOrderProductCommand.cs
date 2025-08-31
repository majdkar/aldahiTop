using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.Orders;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Constants.Orders;
using FirstCall.Shared.Wrapper;

namespace FirstCall.Application.Features.OrderProducts.Commands.AddEdit
{
    public class AddEditDeliveryOrderProductCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int DeliveryOrderId { get; set; }
        public int ProductId { get; set; }

    }
    internal class AddEditDeliveryOrderProductCommandHandler : IRequestHandler<AddEditDeliveryOrderProductCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditDeliveryOrderProductCommandHandler> _localizer;

        public AddEditDeliveryOrderProductCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditDeliveryOrderProductCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditDeliveryOrderProductCommand command, CancellationToken cancellationToken)
        {

            if (command.Id == 0)
            {
                var service = _mapper.Map<DeliveryOrderProduct>(command);


                await _unitOfWork.Repository<DeliveryOrderProduct>().AddAsync(service);
                await _unitOfWork.Commit(cancellationToken);
                var order = _unitOfWork.Repository<DeliveryOrder>().GetAllAsync().Result.Where(s => s.Id == command.DeliveryOrderId).FirstOrDefault();
                if (order != null)
                {
                    await _unitOfWork.Repository<DeliveryOrder>().UpdateAsync(order);
                    await _unitOfWork.Commit(cancellationToken);
                    //var bill = _unitOfWork.Repository<Bill>().GetAllAsync().Result.Where(s => s.DeliveryOrderId == command.DeliveryOrderId).FirstOrDefault();
                    //if (bill != null)
                    //{
                    //    bill.ToltalPrice = price + price2;
                    //    await _unitOfWork.Repository<Bill>().UpdateAsync(bill);
                    //    await _unitOfWork.Commit(cancellationToken);
                    //}
                }



                return await Result<int>.SuccessAsync(service.Id, _localizer["DeliveryOrderProduct Saved"]);
            }
            else
            {
                var service = await _unitOfWork.Repository<DeliveryOrderProduct>().GetByIdAsync(command.Id);
                if (service != null)
                {
                    service.Quantity = (command.Quantity == 0) ? service.Quantity : command.Quantity;
                    service.UnitPrice = (command.UnitPrice == 0) ? service.UnitPrice : command.UnitPrice;
                    service.TotalPrice = (command.TotalPrice == 0) ? service.TotalPrice : command.TotalPrice;
                    service.DeliveryOrderId = (command.DeliveryOrderId == 0) ? service.DeliveryOrderId : command.DeliveryOrderId;
               

                    await _unitOfWork.Repository<DeliveryOrderProduct>().UpdateAsync(service);
                    //await _unitOfWork.Commit(cancellationToken);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken);
                    var order = _unitOfWork.Repository<DeliveryOrder>().GetAllAsync().Result.Where(s => s.Id == command.DeliveryOrderId).FirstOrDefault();
                    if (order != null)
                    {
                     
                        await _unitOfWork.Repository<DeliveryOrder>().UpdateAsync(order);
                        //await _unitOfWork.Commit(cancellationToken);
                        await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDeliveryOrdersCacheKey);

                        //var bill = _unitOfWork.Repository<Bill>().GetAllAsync().Result.Where(s => s.DeliveryOrderId == command.DeliveryOrderId).FirstOrDefault();
                        //if (bill != null)
                        //{
                        //    bill.ToltalPrice = price + price2;
                        //    await _unitOfWork.Repository<Bill>().UpdateAsync(bill);
                        //    //await _unitOfWork.Commit(cancellationToken);
                        //    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBillsCacheKey);

                        //}
                    }
                    return await Result<int>.SuccessAsync(service.Id, _localizer["DeliveryOrderProduct Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["DeliveryOrderProduct Not Found!"]);
                }
            }
        }
    }
}

