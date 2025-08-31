using MediatR;
using Microsoft.EntityFrameworkCore;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.Brands.Queries.GetBrandImage
{
    public class GetBrandImageQuery : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public GetBrandImageQuery(int brandId)
        {
            Id = brandId;
        }
    }

    internal class GGetBrandImageQueryHandler : IRequestHandler<GetBrandImageQuery, Result<string>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GGetBrandImageQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(GetBrandImageQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<Brand>().Entities.Where(p => p.Id == request.Id).Select(a => a.ImageDataURL).FirstOrDefaultAsync(cancellationToken);
            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
