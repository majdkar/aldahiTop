using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirstCall.Application.Features.Warehousess.Commands.AddEdit;
using FirstCall.Application.Features.Warehousess.Commands.Delete;
using FirstCall.Application.Features.Warehousess.Queries.GetAll;
using FirstCall.Application.Features.Warehousess.Queries.GetById;
using FirstCall.Shared.Constants.Permission;
using System.Threading.Tasks;
using FirstCall.Application.Features.Brands.Queries.GetAllPaged;
using FirstCall.Application.Features.Warehousess.Queries.GetAllPaged;

namespace FirstCall.Server.Controllers.v1.GeneralSettings
{
    public class WarehousessController : BaseApiController<WarehousessController>
    {
        /// <summary>
        /// Get All Warehousess
        /// </summary>
        /// 
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Warehouses.View)]
        [HttpGet("GetAllPaged")]
        public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var brands = await _mediator.Send(new GetAllPagedWarehousessQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(brands);
        }
        /// <summary>
        /// Get All Warehousess
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Warehouses.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Warehousess = await _mediator.Send(new GetAllWarehousessQuery());
            return Ok(Warehousess);
        }

        /// <summary>
        /// Get a Warehouses By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Warehouses.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Warehouses = await _mediator.Send(new GetWarehousesByIdQuery() { Id = id });
            return Ok(Warehouses);
        }

        /// <summary>
        /// Create/Update a Color
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Warehouses.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditWarehousesCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Warehouses
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Warehouses.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteWarehousesCommand { Id = id }));
        }

    }
}
