using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirstCall.Application.Features.ProductComponents.Commands.AddEdit;
using FirstCall.Application.Features.Products.Commands.Delete;
using FirstCall.Application.Features.Products.Queries.Export;
using FirstCall.Application.Features.Products.Queries.GetAll;
using FirstCall.Application.Features.Products.Queries.GetAllPaged;
using FirstCall.Application.Features.Products.Queries.GetById;
using FirstCall.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace FirstCall.Server.Controllers.v1.ProductComponents
{
    public class ProductComponentsController : BaseApiController<ProductComponentsController>
    {
        /// <summary>
        /// Get All ProductComponents
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductComponents.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ProductComponents = await _mediator.Send(new GetAllProductComponentsQuery());
            return Ok(ProductComponents);
        }







        /// <summary>
        /// Get All Paged ProductComponents
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="ProductId"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductComponents.View)]
        [HttpGet("GetAllPagedByProductId")]
        public async Task<IActionResult> GetAllPagedByProductId(int ProductId,int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var ProductComponents = await _mediator.Send(new GetAllPagedProductComponentsByProductIdQuery(pageNumber, pageSize, searchString, orderBy,ProductId));
            return Ok(ProductComponents);
        }
          /// <summary>
        /// Get All Paged ProductComponents
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductComponents.View)]
        [HttpGet("GetAllPaged")]
        public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var ProductComponents = await _mediator.Send(new GetAllPagedProductComponentsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(ProductComponents);
        }

      


        /// <summary>
        /// Get ProductComponent By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductComponents.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _mediator.Send(new GetProductComponentByIdQuery { Id = id });
            return Ok(company);
        }


        /// <summary>
        /// Add/Edit a ProductComponent for company profile
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductComponents.Create)]
        [HttpPost("AddEditCompanyProductComponent")]
        public async Task<IActionResult> Post(AddEditCompanyProductComponentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a ProductComponent
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.ProductComponents.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteProductComponentCommand { Id = id }));
        }


    }
}
