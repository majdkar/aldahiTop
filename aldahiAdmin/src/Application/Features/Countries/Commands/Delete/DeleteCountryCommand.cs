using MediatR;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Features.Countries.Commands.Delete;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.Countries.Commands.Delete
{
    public class DeleteCountryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    internal class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Result<int>>
    {
        private readonly IStringLocalizer<DeleteCountryCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteCountryCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteCountryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteCountryCommand command, CancellationToken cancellationToken)
        {
            var country = await _unitOfWork.Repository<Country>().GetByIdAsync(command.Id);
            if (country != null)
            {
                country.IsDeleted = true;
                await _unitOfWork.Repository<Country>().UpdateAsync(country);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCountriesCacheKey);
                return await Result<int>.SuccessAsync(country.Id, _localizer["Country Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Country Not Found!"]);
            }
        }
    }
}