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
using FirstCall.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

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
            var order = await _unitOfWork.Repository<DeliveryOrder>().Entities.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == command.Id);
            // جيب الطلب مع تفاصيل المنتجات

            if (order != null)
            {
                // 1- إرجاع الكميات للمخزون
                foreach (var item in order.Products)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Qty += item.Quantity; // رجع الكمية
                        await _unitOfWork.Repository<Product>().UpdateAsync(product);
                    }
                }

                // 2- عمل Soft Delete للطلب
                order.IsDeleted = true;
                await _unitOfWork.Repository<DeliveryOrder>().UpdateAsync(order);

                // 3- Commit
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
