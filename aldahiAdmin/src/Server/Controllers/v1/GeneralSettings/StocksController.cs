using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirstCall.Application.Features.Stocks.Commands.AddEdit;
using FirstCall.Application.Features.Stocks.Commands.Delete;
using FirstCall.Application.Features.Stocks.Queries.GetAll;
using FirstCall.Application.Features.Stocks.Queries.GetById;
using FirstCall.Shared.Constants.Permission;
using System.Threading.Tasks;
using FirstCall.Application.Features.Brands.Queries.GetAllPaged;
using FirstCall.Application.Features.Stocks.Queries.GetAllPaged;

namespace FirstCall.Server.Controllers.v1.GeneralSettings
{
    public class StocksController : BaseApiController<StocksController>
    {
        /// <summary>
        /// Get All Stocks
        /// </summary>
        /// 
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Stocks.View)]
        [HttpGet("GetAllPaged")]
        public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var brands = await _mediator.Send(new GetAllPagedStocksQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(brands);
        }
        /// <summary>
        /// Get All Stocks
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Stocks.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Stocks = await _mediator.Send(new GetAllStocksQuery());
            return Ok(Stocks);
        }

        /// <summary>
        /// Get a Stock By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Stocks.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Stock = await _mediator.Send(new GetStockByIdQuery() { Id = id });
            return Ok(Stock);
        }

        /// <summary>
        /// Create/Update a Color
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Stocks.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditStockCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Stock
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Stocks.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteStockCommand { Id = id }));
        }

    }
}
