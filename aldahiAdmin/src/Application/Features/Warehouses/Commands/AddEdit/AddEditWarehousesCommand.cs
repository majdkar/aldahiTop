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

namespace FirstCall.Application.Features.Warehousess.Commands.AddEdit
{
    public partial class AddEditWarehousesCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }

    internal class AddEditWarehousesCommandHandler : IRequestHandler<AddEditWarehousesCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditWarehousesCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditWarehousesCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditWarehousesCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditWarehousesCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var Warehouses = _mapper.Map<Warehouses>(command);
                await _unitOfWork.Repository<Warehouses>().AddAsync(Warehouses);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWarehousessCacheKey);
                return await Result<int>.SuccessAsync(Warehouses.Id, _localizer["Warehouses Saved"]);
            }
            else
            {
                var Warehouses = await _unitOfWork.Repository<Warehouses>().GetByIdAsync(command.Id);
                if (Warehouses != null) 
                {
                    Warehouses.NameAr = command.NameAr ?? Warehouses.NameAr;
                    Warehouses.NameEn = command.NameEn ?? Warehouses.NameEn;                        ;
                    await _unitOfWork.Repository<Warehouses>().UpdateAsync(Warehouses);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWarehousessCacheKey);
                    return await Result<int>.SuccessAsync(Warehouses.Id, _localizer["Warehouses Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Warehouses Not Found!"]);
                }
            }
        }
    }
}