using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using FirstCall.Shared.Constants.Application;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Domain.Entities.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCall.Application.Features.Stocks.Commands.AddEdit
{
    public partial class AddEditStockCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string NameAr { get; set; }
        public string NameEn { get; set; }


        public int Quantity { get; set; }


        public int ProductId { get; set; }


        public int WarehousesId { get; set; }
    }

    internal class AddEditStockCommandHandler : IRequestHandler<AddEditStockCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditStockCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditStockCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditStockCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditStockCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var Stock = _mapper.Map<Stock>(command);
                await _unitOfWork.Repository<Stock>().AddAsync(Stock);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllStocksCacheKey);
                return await Result<int>.SuccessAsync(Stock.Id, _localizer["Stock Saved"]);
            }
            else
            {
                var Stock = await _unitOfWork.Repository<Stock>().GetByIdAsync(command.Id);
                if (Stock != null) 
                {
                    Stock.NameAr = command.NameAr ?? Stock.NameAr;
                    Stock.NameEn = command.NameEn ?? Stock.NameEn;                        ;
                    Stock.Quantity = command.Quantity;                        ;
                    Stock.ProductId = command.ProductId;                        ;
                    Stock.WarehousesId = command.WarehousesId;                        ;
                    await _unitOfWork.Repository<Stock>().UpdateAsync(Stock);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllStocksCacheKey);
                    return await Result<int>.SuccessAsync(Stock.Id, _localizer["Stock Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Stock Not Found!"]);
                }
            }
        }
    }
}