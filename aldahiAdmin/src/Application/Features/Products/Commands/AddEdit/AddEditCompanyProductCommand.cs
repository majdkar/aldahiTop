using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Interfaces.Services;
using FirstCall.Application.Requests;
using FirstCall.Domain.Entities.Products;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Wrapper;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static FirstCall.Shared.Constants.Permission.Permissions;
using FirstCall.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Products.Commands.AddEdit
{
    public partial class AddEditCompanyProductCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string? Code { get; set; }
        public string? PackageNumber { get; set; }
        public string? Colors { get; set; }
        public string? Sizes { get; set; }
        public decimal Price { get; set; }
        public int Order { get; set; } = 0;
        public string ProductImageUrl { get; set; }
        public string ProductImageUrl2 { get; set; }
        public string ProductImageUrl3 { get; set; }
        public string ProductImageUrl4 { get; set; }
        public int Qty { get; set; }

        public string Type { get; set; }

        public int SeasonId { get; set; }

        public int GroupId { get; set; }
        public int ProductCategoryId { get; set; }

        public int KindId { get; set; }
        public UploadRequest UploadRequest { get; set; }
        public UploadRequest UploadRequest2 { get; set; }
        public UploadRequest UploadRequest3 { get; set; }
        public UploadRequest UploadRequest4 { get; set; }

        public int? WarehousesId { get; set; }
    }

    internal class AddEditProductCommandHandler : IRequestHandler<AddEditCompanyProductCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditProductCommandHandler> _localizer;

        public AddEditProductCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditProductCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCompanyProductCommand command, CancellationToken cancellationToken)
        {
            var uploadRequest = command.UploadRequest;
            var uploadRequest2 = command.UploadRequest2;
            var uploadRequest3 = command.UploadRequest3;
            var uploadRequest4 = command.UploadRequest4;
    
            if (uploadRequest != null)
            {
                uploadRequest.FileName = $"{Path.GetRandomFileName()}{uploadRequest.Extension}";
            }
            if (uploadRequest2 != null)
            {
                uploadRequest2.FileName = $"{Path.GetRandomFileName()}{uploadRequest2.Extension}";
            }
            if (uploadRequest3 != null)
            {
                uploadRequest3.FileName = $"{Path.GetRandomFileName()}{uploadRequest3.Extension}";
            }
            if (uploadRequest4 != null)
            {
                uploadRequest4.FileName = $"{Path.GetRandomFileName()}{uploadRequest4.Extension}";
            }
       
            if (command.Id == 0)
            {
                var product = _mapper.Map<Product>(command);
                if (uploadRequest != null)
                {
                    product.ProductImageUrl = _uploadService.UploadAsync(uploadRequest);
                }          
                if (uploadRequest2 != null)
                {
                    product.ProductImageUrl2 = _uploadService.UploadAsync(uploadRequest2);
                }        
                if (uploadRequest3 != null)
                {
                    product.ProductImageUrl3 = _uploadService.UploadAsync(uploadRequest3);
                }       
                if (uploadRequest4 != null)
                {
                    product.ProductImageUrl4 = _uploadService.UploadAsync(uploadRequest4);
                }
                product.NameEn = command.NameEn;

                await _unitOfWork.Repository<Product>().AddAsync(product);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductsCacheKey);
                return await Result<int>.SuccessAsync(product.Id, _localizer["Product Saved"]);
            }
            else
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(command.Id);
                if (product != null)
                {
                    product.NameAr = command.NameAr ?? product.NameAr;
                    product.NameEn = command.NameEn ?? product.NameEn;
                    product.PackageNumber = command.PackageNumber ?? product.PackageNumber;
                    product.WarehousesId = command.WarehousesId;
                    product.Colors = command.Colors ?? product.Colors;
                    product.Sizes = command.Sizes ?? product.Sizes;
                    product.Sizes = command.Sizes ?? product.Sizes;
                    product.Order = command.Order;
                    product.Qty = command.Qty;
                    product.Code = command.Code;
                    product.ProductCategoryId = command.ProductCategoryId;
                    product.SeasonId = command.SeasonId;
                    product.GroupId = command.GroupId;
                    product.Type = command.Type;

                    product.Price = command.Price == 0 ? product.Price : command.Price;

                    if (uploadRequest != null)
                    {
                        product.ProductImageUrl = _uploadService.UploadAsync(uploadRequest);
                    }
                    if (uploadRequest2 != null)
                    {
                        product.ProductImageUrl2 = _uploadService.UploadAsync(uploadRequest2);
                    }
                    if (uploadRequest3 != null)
                    {
                        product.ProductImageUrl3 = _uploadService.UploadAsync(uploadRequest3);
                    }
                    if (uploadRequest4 != null)
                    {
                        product.ProductImageUrl4 = _uploadService.UploadAsync(uploadRequest4);
                    }
              
             

                    await _unitOfWork.Repository<Product>().UpdateAsync(product);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductsCacheKey);
                    return await Result<int>.SuccessAsync(product.Id, _localizer["Product Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Product Not Found!"]);
                }
            }
        }
    }
}