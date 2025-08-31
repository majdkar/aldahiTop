using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Wrapper;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.Countries.Commands.AddEdit
{
    public partial class AddEditCountryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public string PhoneCode { get; set; }
    }

    internal class AddEditCountryCommandHandler : IRequestHandler<AddEditCountryCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditCountryCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditCountryCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditCountryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCountryCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var country = _mapper.Map<Country>(command);
                await _unitOfWork.Repository<Country>().AddAsync(country);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCountriesCacheKey);
                return await Result<int>.SuccessAsync(country.Id, _localizer["Country Saved"]);
            }
            else
            {
                var country = await _unitOfWork.Repository<Country>().GetByIdAsync(command.Id);
                if (country != null)
                {
                    country.NameAr = command.NameAr ?? country.NameAr;
                    country.NameEn = command.NameEn ?? country.NameEn;
                    country.Code = command.Code ?? country.Code;

                    await _unitOfWork.Repository<Country>().UpdateAsync(country);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCountriesCacheKey);
                    return await Result<int>.SuccessAsync(country.Id, _localizer["Country Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Country Not Found!"]);
                }
            }
        }
    }
}