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
using System.Linq;
using FirstCall.Domain.Entities.Orders;
using static FirstCall.Shared.Constants.Permission.Permissions;
using System.Collections.Generic;
using FirstCall.Application.Features.OrderProducts.Commands.AddEdit;
using DocumentFormat.OpenXml.InkML;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Features.Orders.Commands.AddEdit
{
    public class AddEditDeliveryOrderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        //order information
        public string OrderNumber { get; set; }
        public int ClientId { get; set; }

        public decimal TotalPrice { get; set; }

        public List<AddEditDeliveryOrderProductCommand> ChargesCommand { get; set; } = new();

        public string Status { get; set; }


        public DateTime? OrderDate { get; set; }

        public string Type { get; set; }


    }

    internal class AddEditDeliveryOrderCommandHandler : IRequestHandler<AddEditDeliveryOrderCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditDeliveryOrderCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditDeliveryOrderCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditDeliveryOrderCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditDeliveryOrderCommand command, CancellationToken cancellationToken)
        {

            if (command.Id == 0)
            {
                using var transaction = await _unitOfWork.BeginTransactionAsync(new CancellationToken());
                try
                {
                    var order = _mapper.Map<DeliveryOrder>(command);
                    Random generator = new Random();

                    //order.OrderNumber = generator.Next(0, 1000000).ToString("D6");

                    foreach (var item in command.ChargesCommand)
                    {
                        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);

                        if (product == null)
                            throw new Exception("المنتج غير موجود");

                        if (product.Qty < item.Quantity)
                            throw new Exception($"الكمية غير متوفرة للمنتج {product.NameAr}");

                        // خصم الكمية
                        product.Qty -= item.Quantity;
                        await _unitOfWork.Repository<Product>().UpdateAsync(product);
                    }



                    await _unitOfWork.Repository<DeliveryOrder>().AddAsync(order);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDeliveryOrdersCacheKey);
                    //if(order.Status == "Pending")
                    //{
                    //    int maxBillNo = 0;
                    //    var bills = await _unitOfWork.Repository<Bill>().GetAllAsync();
                    //    if (bills.Count > 0)
                    //        maxBillNo = bills.Max(e => e.Number);
                    //    var bill = new Bill
                    //    {
                    //        ClientId = order.ClientId,
                    //        DeliveryOrderId = order.Id,
                    //        //UnitPrice = event1.Price,
                    //        ToltalPrice = order.TotalPrice,
                    //        Number = maxBillNo + 1,
                    //        BillNumber = "MC-" + (maxBillNo + 1),
                    //        BillDate = DateTime.Now,
                    //        //IsDeletedByClient = false,
                    //        Status = "unpaid",

                    //    };
                    //    await _unitOfWork.Repository<Bill>().AddAsync(bill);
                    //    await _unitOfWork.Commit(cancellationToken);
                    //}
                    await transaction.CommitAsync();

                    return await Result<int>.SuccessAsync(order.Id, _localizer["DeliveryOrder Saved"]);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }

            }
            else
            {
                var order = await _unitOfWork.Repository<DeliveryOrder>().GetByIdAsync(command.Id);
                if (order != null)
                {
                    order.TotalPrice = command.TotalPrice == 0 ? order.TotalPrice : command.TotalPrice;
                    order.Status = command.Status;
                    order.OrderNumber = command.OrderNumber;
                    order.ClientId = command.ClientId;
                    order.Type = command.Type;


                    await _unitOfWork.Repository<DeliveryOrder>().UpdateAsync(order);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDeliveryOrdersCacheKey);
                    return await Result<int>.SuccessAsync(order.Id, _localizer["DeliveryOrder Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["DeliveryOrder Not Found!"]);
                }
            }
        }
    }
}
