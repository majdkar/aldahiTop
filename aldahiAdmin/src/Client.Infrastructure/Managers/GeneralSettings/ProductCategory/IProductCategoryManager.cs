using FirstCall.Application.Features.ProductCategories.Commands.AddEdit;
using FirstCall.Application.Features.ProductCategories.Queries.GetAll;
using FirstCall.Application.Features.ProductCategories.Queries.GetAllPaged;

using FirstCall.Application.Requests.ProductCategories;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.ProductCategory
{
    public interface IProductCategoryManager : IManager
    {
        Task<PaginatedResult<GetAllPagedProductCategoriesResponse>> GetPagedByTypeAsync(GetAllPagedProductCategoriesRequest request);

        Task<IResult<List<GetAllProductCategoriesResponse>>> GetAllByTypeAsync(string type);
        Task<IResult<List<GetAllProductCategoriesResponse>>> GetAllAsync();
        // Task<IResult<List<GetAllProductCategoriesResponse>>> GetAllParentCategoriesByTypeAsync(string type);


        Task<IResult<List<GetAllProductCategoriesResponse>>> GetAllByParentCategoryAsync(int categoryId);
        Task<PaginatedResult<GetAllPagedProductCategoriesResponse>> GetAllCategorySonsAsync(GetAllPagedProductCategoriesRequest request, int categoryId);

        Task<IResult<string>> GetProductCategoryImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditProductCategoryCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}
