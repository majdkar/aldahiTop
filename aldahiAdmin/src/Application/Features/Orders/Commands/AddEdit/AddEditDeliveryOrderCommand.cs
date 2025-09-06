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

                    // 1- تحقق من الكميات أولاً (بدون تعديل)
                    foreach (var item in command.ChargesCommand)
                    {
                        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);

                        if (product == null)
                            return await Result<int>.FailAsync($"المنتج برقم {item.Id} غير موجود");

                        if (product.Qty < item.Quantity)
                            return await Result<int>.FailAsync($"الكمية غير متوفرة للمنتج {product.NameAr} (المتوفر: {product.Qty})");
                    }

                    // 2- بعد التأكد: خصم الكميات فعليًا
                    foreach (var item in command.ChargesCommand)
                    {
                        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                        product.Qty -= item.Quantity;
                        await _unitOfWork.Repository<Product>().UpdateAsync(product);
                    }

                    // 3- حفظ الطلب
                    await _unitOfWork.Repository<DeliveryOrder>().AddAsync(order);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDeliveryOrdersCacheKey);

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
                using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
                try
                {
                    var order = await _unitOfWork.Repository<DeliveryOrder>()
                        .Entities.Include(x => x.Products)
                        .FirstOrDefaultAsync(x => x.Id == command.Id);

                    if (order == null)
                        return await Result<int>.FailAsync(_localizer["DeliveryOrder Not Found!"]);

                    // 1- تحقق من الكميات أولًا (بدون تعديل)
                    foreach (var item in command.ChargesCommand)
                    {
                        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                        var oldQty = order.Products.FirstOrDefault(p => p.ProductId == item.ProductId)?.Quantity ?? 0;

                        if (product == null)
                            return await Result<int>.FailAsync($"المنتج برقم {item.ProductId} غير موجود");

                        if ((product.Qty + oldQty) < item.Quantity)
                            return await Result<int>.FailAsync($"الكمية غير متوفرة للمنتج {product.NameAr} (المتوفر: {product.Qty})");
                    }

                    // 2- إرجاع الكميات القديمة
                    foreach (var oldItem in order.Products)
                    {
                        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(oldItem.ProductId);
                        if (product != null)
                        {
                            product.Qty += oldItem.Quantity;
                            await _unitOfWork.Repository<Product>().UpdateAsync(product);
                        }
                    }

                    // 3- حذف التفاصيل القديمة
                    await _unitOfWork.Repository<DeliveryOrderProduct>().DeleteRangeAsync(order.Products);

                    // 4- إضافة التفاصيل الجديدة + خصم الكميات الجديدة
                    var newCharges = new List<DeliveryOrderProduct>();
                    foreach (var item in command.ChargesCommand)
                    {
                        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);

                        product.Qty -= item.Quantity;
                        await _unitOfWork.Repository<Product>().UpdateAsync(product);

                        newCharges.Add(new DeliveryOrderProduct
                        {
                            DeliveryOrderId = order.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                           TotalPrice = item.TotalPrice,
                            
                        });
                    }

                    await _unitOfWork.Repository<DeliveryOrderProduct>().AddRangeAsync(newCharges);

                    // 5- تحديث بيانات الطلب
                    order.TotalPrice = command.TotalPrice == 0 ? order.TotalPrice : command.TotalPrice;
                    order.Status = command.Status;
                    order.OrderNumber = command.OrderNumber;
                    order.ClientId = command.ClientId;
                    order.Type = command.Type;

                    await _unitOfWork.Repository<DeliveryOrder>().UpdateAsync(order);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDeliveryOrdersCacheKey);

                    await transaction.CommitAsync();

                    return await Result<int>.SuccessAsync(order.Id, _localizer["DeliveryOrder Updated"]);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }

            }
        }
    }
}
