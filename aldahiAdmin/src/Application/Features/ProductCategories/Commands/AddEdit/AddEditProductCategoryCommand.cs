using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Interfaces.Services;
using FirstCall.Application.Requests;
using FirstCall.Core.Entities;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Wrapper;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.ProductCategories.Commands.AddEdit
{
    public class AddEditProductCategoryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        public int ParentCategoryId { get; set; } = 0;
        public int Order { get; set; }
        public string? ImageDataURL { get; set; }
        public UploadRequest UploadRequest { get; set; }

        public string Type { get; set; }
    }

    internal class AddEditProductCategoryCommandHandler : IRequestHandler<AddEditProductCategoryCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditProductCategoryCommandHandler> _localizer;

        public AddEditProductCategoryCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditProductCategoryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditProductCategoryCommand command, CancellationToken cancellationToken)
        {


            var uploadRequest = command.UploadRequest;
            if (uploadRequest != null)
            {
                uploadRequest.FileName = $"{Path.GetRandomFileName()}{uploadRequest.Extension}";
            }

            if (command.Id == 0)
            {
                var productCategory = _mapper.Map<ProductCategory>(command);
                if (uploadRequest != null)
                {
                    productCategory.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                }
                productCategory.ParentCategoryId = productCategory.ParentCategoryId == 0 ? null : productCategory.ParentCategoryId;
                await _unitOfWork.Repository<ProductCategory>().AddAsync(productCategory);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductCategoriesCacheKey);
                return await Result<int>.SuccessAsync(productCategory.Id, _localizer["ProductCategory Saved"]);
            }
            else
            {
                var productCategory = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(command.Id);
                if (productCategory != null)
                {
                    productCategory.NameEn = command.NameEn ?? productCategory.NameEn;
                    productCategory.NameAr = command.NameAr ?? productCategory.NameAr;
                    productCategory.DescriptionEn = command.DescriptionEn ?? productCategory.DescriptionEn;
                    productCategory.DescriptionAr = command.DescriptionAr ?? productCategory.DescriptionAr;
                    if (uploadRequest != null)
                    {
                        productCategory.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                    }
                    productCategory.ParentCategoryId = (command.ParentCategoryId == 0) ? productCategory.ParentCategoryId : command.ParentCategoryId;
                    productCategory.Order = (command.Order == 0) ? productCategory.Order : command.Order;
                    productCategory.Type = command.Type ?? productCategory.Type;
                    await _unitOfWork.Repository<ProductCategory>().UpdateAsync(productCategory);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(productCategory.Id, _localizer["ProductCategory Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["ProductCategory Not Found!"]);
                }
            }
        }
    }
}
