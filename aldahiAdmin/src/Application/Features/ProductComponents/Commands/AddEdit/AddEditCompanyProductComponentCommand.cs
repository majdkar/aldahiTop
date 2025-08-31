using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Interfaces.Services;
using FirstCall.Application.Requests;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Wrapper;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Features.ProductComponents.Commands.AddEdit
{
    public partial class AddEditCompanyProductComponentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public string? DescriptionAboutEn { get; set; }
        public int ProductId { get; set; }

        public string ProductComponentImageUrl { get; set; }
        public UploadRequest UploadRequest { get; set; }

        public string ProductComponentImageUrl1 { get; set; }
        public UploadRequest UploadRequest1 { get; set; }

        public string ProductComponentImageUrl2 { get; set; }
        public UploadRequest UploadRequest2 { get; set; }

        public string ProductComponentImageUrl3 { get; set; }
        public UploadRequest UploadRequest3 { get; set; }

        public string ProductComponentImageUrl4 { get; set; }
        public UploadRequest UploadRequest4 { get; set; }

        public string ProductComponentImageUrl5 { get; set; }
        public UploadRequest UploadRequest5 { get; set; }
        public int Order { get; set; } = 0;


    }

    internal class AddEditProductComponentCommandHandler : IRequestHandler<AddEditCompanyProductComponentCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditProductComponentCommandHandler> _localizer;

        public AddEditProductComponentCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditProductComponentCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCompanyProductComponentCommand command, CancellationToken cancellationToken)
        {
            var uploadRequest = command.UploadRequest;
            var uploadRequest1 = command.UploadRequest1;
            var uploadRequest2 = command.UploadRequest2;
            var uploadRequest3 = command.UploadRequest3;
            var uploadRequest4 = command.UploadRequest4;
            var uploadRequest5 = command.UploadRequest5;
    
            if (uploadRequest != null)
            {
                uploadRequest.FileName = $"{Path.GetRandomFileName()}{uploadRequest.Extension}";
            }  
            if (uploadRequest1 != null)
            {
                uploadRequest1.FileName = $"{Path.GetRandomFileName()}{uploadRequest1.Extension}";
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
            if (uploadRequest5 != null)
            {
                uploadRequest5.FileName = $"{Path.GetRandomFileName()}{uploadRequest5.Extension}";
            }  
            
          
       
            if (command.Id == 0)
            {
                var ProductComponent = _mapper.Map<ProductCom>(command);
                if (uploadRequest != null)
                {
                    ProductComponent.ProductComponentImageUrl = _uploadService.UploadAsync(uploadRequest);
                }  
                if (uploadRequest1 != null)
                {
                    ProductComponent.ProductComponentImageUrl1 = _uploadService.UploadAsync(uploadRequest1);
                } 
                if (uploadRequest2 != null)
                {
                    ProductComponent.ProductComponentImageUrl2 = _uploadService.UploadAsync(uploadRequest2);
                }  
                if (uploadRequest3 != null)
                {
                    ProductComponent.ProductComponentImageUrl3 = _uploadService.UploadAsync(uploadRequest3);
                }  
                if (uploadRequest4 != null)
                {
                    ProductComponent.ProductComponentImageUrl4 = _uploadService.UploadAsync(uploadRequest4);
                }  
                if (uploadRequest5 != null)
                {
                    ProductComponent.ProductComponentImageUrl5 = _uploadService.UploadAsync(uploadRequest5);
                } 
           
                await _unitOfWork.Repository<ProductCom>().AddAsync(ProductComponent);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductComponentsCacheKey);
                return await Result<int>.SuccessAsync(ProductComponent.Id, _localizer["Product Component Saved"]);
            }
            else
            {
                var ProductComponent = await _unitOfWork.Repository<ProductCom>().GetByIdAsync(command.Id);
                if (ProductComponent != null)
                {
                    ProductComponent.NameAr = command.NameAr ?? ProductComponent.NameAr;
                    ProductComponent.Order = command.Order;
                    ProductComponent.NameEn = command.NameEn ?? ProductComponent.NameEn;
                    ProductComponent.ProductId = command.ProductId ;

                    ProductComponent.DescriptionAboutEn = command.DescriptionAboutEn;


                    if (uploadRequest != null)
                    {
                        ProductComponent.ProductComponentImageUrl = _uploadService.UploadAsync(uploadRequest);
                    }
                    if (uploadRequest1 != null)
                    {
                        ProductComponent.ProductComponentImageUrl1 = _uploadService.UploadAsync(uploadRequest1);
                    }
                    if (uploadRequest2 != null)
                    {
                        ProductComponent.ProductComponentImageUrl2 = _uploadService.UploadAsync(uploadRequest2);
                    }
                    if (uploadRequest3 != null)
                    {
                        ProductComponent.ProductComponentImageUrl3 = _uploadService.UploadAsync(uploadRequest3);
                    }
                    if (uploadRequest4 != null)
                    {
                        ProductComponent.ProductComponentImageUrl4 = _uploadService.UploadAsync(uploadRequest4);
                    }
                    if (uploadRequest5 != null)
                    {
                        ProductComponent.ProductComponentImageUrl5 = _uploadService.UploadAsync(uploadRequest5);
                    }
                    await _unitOfWork.Repository<ProductCom>().UpdateAsync(ProductComponent);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductComponentsCacheKey);
                    return await Result<int>.SuccessAsync(ProductComponent.Id, _localizer["Product Component Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Product Component Not Found!"]);
                }
            }
        }
    }
}