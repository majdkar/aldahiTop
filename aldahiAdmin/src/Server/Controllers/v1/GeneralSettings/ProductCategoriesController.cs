using FirstCall.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FirstCall.Application.Features.ProductCategories.Queries.GetAllPaged;

using FirstCall.Application.Features.ProductCategories.Queries.GetProductCategoryImage;

using FirstCall.Application.Features.ProductCategories.Commands.AddEdit;

using FirstCall.Application.Features.ProductCategories.Commands.Delete;

using FirstCall.Application.Features.ProductCategories.Queries.Export;
using FirstCall.Application.Features.ProductCategories.Queries.GetAll;

namespace FirstCall.Server.Controllers.v1.GeneralSettings
{
    public class ProductCategoriesController : BaseApiController<ProductCategoriesController>
    {
        ///// <summary>
        ///// Get All ProductCategories
        ///// </summary>
        ///// <param name="pageNumber"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="searchString"></param>
        ///// <param name="orderBy"></param>
        ///// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.ProductCategories.View)]
        //[HttpGet]
        //public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize, string searchString, string orderBy = null)
        //{
        //    var productCategories = await _mediator.Send(new GetAllPagedProductCategoriesQuery(pageNumber, pageSize, searchString, orderBy));
        //    return Ok(productCategories);
        //}

        /// <summary>
        /// Get All ProductCategories By type (B2B - B2C)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet("GetAllPagedByType")]
        public async Task<IActionResult> GetAllPaged(string type, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var productCategories = await _mediator.Send(new GetAllPagedProductCategoriesByTypeQuery(type, pageNumber, pageSize, searchString, orderBy));
            return Ok(productCategories);
        }

       
        /// <summary>
        /// Get All ProductCategories By ParentCategory
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet("GetAllByParentCategory/{categoryId}")]
        public async Task<IActionResult> GetAllByParentCategory(int categoryId)
        {
            var productCategories = await _mediator.Send(new GetAllProductCategoriesByByParentCategoryQuery { ParentCategoryId = categoryId });
            return Ok(productCategories);
        }

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _mediator.Send(new GetAllProductCategoriesQuery());
            return Ok(categories);
        }

        //[Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet("GetAllSons")]
        public async Task<IActionResult> GetAllSons(string type,int categoryId, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var productCategories = await _mediator.Send(new GetAllPagedProductCategorySonsQuery(type,categoryId, pageNumber, pageSize, searchString, orderBy));
            return Ok(productCategories);
        }

        /// <summary>
        /// Get All Categories By type (B2B - B2C)
        /// </summary>
        ///  <param name="type"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet("GetAllByType")]
        public async Task<IActionResult> GetAll(string type)
        {
            var categories = await _mediator.Send(new GetAllProductCategoriesByTypeQuery { Type = type});
            return Ok(categories);
        }

        /// <summary>
        /// Get All Parent Categories By type (B2B - B2C)
        /// </summary>
        ///  <param name="type"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet("GetAllParentCategoriesByType")]
        public async Task<IActionResult> GetAllParentCategories(string type)
        {
            var parentcategories = await _mediator.Send(new GetAllParentCategoriesByTypeQuery { Type = type });
            return Ok(parentcategories);
        }



        /// <summary>
        /// Get a ProductCategory Image by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetProductCategoryImageAsync(int id)
        {
            var result = await _mediator.Send(new GetProductCategoryImageQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Add/Edit a ProductCategory
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.ProductCategories.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditProductCategoryCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a ProductCategory
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        //[Authorize(Policy = Permissions.ProductCategories.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteProductCategoryCommand { Id = id }));
        }

        /// <summary>
        /// Search ProductCategories and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportProductCategoriesQuery(searchString)));
        }
    }
}
