using MediatR;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Core.Entities;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using FirstCall.Domain.Entities.Products;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;

namespace FirstCall.Application.Features.ProductCategories.Commands.Delete
{
    public class DeleteProductCategoryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    internal class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteProductCategoryCommandHandler> _localizer;

        public DeleteProductCategoryCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteProductCategoryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteProductCategoryCommand command, CancellationToken cancellationToken)
        {
            var subValues = await _unitOfWork.Repository<ProductCategory>().Entities.Where(x => x.ParentCategoryId == command.Id).ToListAsync();
            if (subValues.Count() > 0)
            {
                foreach (var item in subValues)
                {
                    var subsubValues = await _unitOfWork.Repository<ProductCategory>().Entities.Where(x => x.ParentCategoryId == item.Id).ToListAsync();
                    if (subsubValues.Count() > 0)
                    {
                        foreach (var itemsub in subsubValues)
                        {
                            var subsubsubValues = await _unitOfWork.Repository<ProductCategory>().Entities.Where(x => x.ParentCategoryId == itemsub.Id).ToListAsync();
                            if (subsubsubValues.Count() > 0)
                            {
                                foreach (var itemsubsub in subsubsubValues)
                                {
                                    var productCategorysubsubsub = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(itemsubsub.Id);
                                    if (productCategorysubsubsub != null)
                                    {

                                      
                                        productCategorysubsubsub.IsDeleted = true;
                                        await _unitOfWork.Repository<ProductCategory>().UpdateAsync(productCategorysubsubsub);
                                        await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductCategoriesCacheKey);
                                        //return await Result<int>.SuccessAsync(productCategory.Id, _localizer["ProductCategory Deleted"]);
                                    }
                                }
                            }
                            else
                            {
                                var productsubsubCategory = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(itemsub.Id);
                                if (productsubsubCategory != null)
                                {
      
                                    productsubsubCategory.IsDeleted = true;
                                    await _unitOfWork.Repository<ProductCategory>().UpdateAsync(productsubsubCategory);
                                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductCategoriesCacheKey);
                                   
                                }
                            }
                        }

                    }
                    else
                    {
                        var productsubCategory = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(item.Id);
                        if (productsubCategory != null)
                        {


                            productsubCategory.IsDeleted = true;
                            await _unitOfWork.Repository<ProductCategory>().UpdateAsync(productsubCategory);
                            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductCategoriesCacheKey);
                            //return await Result<int>.SuccessAsync(productCategory.Id, _localizer["ProductCategory Deleted"]);
                        }
                      
                    }
                }
                var productCategory = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(command.Id);
                if (productCategory != null)
                {
                   
                    productCategory.IsDeleted = true;
                    await _unitOfWork.Repository<ProductCategory>().UpdateAsync(productCategory);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductCategoriesCacheKey);
                    return await Result<int>.SuccessAsync(productCategory.Id, _localizer["ProductCategory Deleted"]);
                }
                return await Result<int>.SuccessAsync(command.Id, _localizer["ProductCategory Deleted"]);
            }
            else
                { 
                        var productCategory = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(command.Id);
                        if (productCategory != null)
                        {
                            //foreach (var item in productCategory.d)
                            //{
                                var productsubCategory= await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(command.Id);
                            productCategory.IsDeleted = true;
                            await _unitOfWork.Repository<ProductCategory>().UpdateAsync(productCategory);
                            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductCategoriesCacheKey);
                            return await Result<int>.SuccessAsync(productCategory.Id, _localizer["ProductCategory Deleted"]);
                        }
                        else
                        {
                            return await Result<int>.FailAsync(_localizer["ProductCategory Not Found!"]);
                        }
                }
            }
    }
}
