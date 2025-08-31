using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirstCall.Application.Features.Products.Commands.AddEdit;
using FirstCall.Application.Features.Products.Commands.Delete;
using FirstCall.Application.Features.Products.Queries.Export;
using FirstCall.Application.Features.Products.Queries.GetAll;
using FirstCall.Application.Features.Products.Queries.GetAllPaged;
using FirstCall.Application.Features.Products.Queries.GetById;
using FirstCall.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace FirstCall.Server.Controllers.v1.Products
{
    public class ProductsController : BaseApiController<ProductsController>
    {
        /// <summary>
        /// Get All Products
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }







        /// <summary>
        /// Get All Paged Products
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.View)]
        [HttpGet("GetAllPaged")]
        public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var products = await _mediator.Send(new GetAllPagedProductsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(products);
        }


        /// <summary>
        /// Get All Paged Products By CategoryId
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="categoryId"></param>
        /// <param name="subcategoryId"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [HttpGet("GetAllPagedByCategoryId")]
        public async Task<IActionResult> GetAllPagedByCategoryId(int categoryId,int subcategoryId, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var products = await _mediator.Send(new GetAllPagedProductsByCategoryIdQuery(pageNumber, pageSize, searchString, orderBy,categoryId,subcategoryId));
            return Ok(products);
        }

        /// <summary>
        /// Get All Paged Search Products
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="productname"></param>
        /// <param name="fromprice"></param> 
        /// <param name="toprice"></param>        
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.View)]
        [HttpGet("GetAllPagedSearchProduct")]
        public async Task<IActionResult> GetAllPagedSearchProduct( string productname, decimal fromprice, decimal toprice,int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var products = await _mediator.Send(new GetAllPagedSearchProductsQuery(  pageNumber, pageSize, searchString, orderBy, productname, fromprice, toprice));
            return Ok(products);
        }


        /// <summary>
        /// Get Product By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _mediator.Send(new GetProductByIdQuery { Id = id });
            return Ok(company);
        }


        /// <summary>
        /// Add/Edit a Product for company profile
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.Create)]
        [HttpPost("AddEditCompanyProduct")]
        public async Task<IActionResult> Post(AddEditCompanyProductCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Products.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteProductCommand { Id = id }));
        }

        /// <summary>
        /// Search Products and Export to Excel
        /// </summary>
     
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportCompanyProductsQuery(searchString)));
        }

    }
}
