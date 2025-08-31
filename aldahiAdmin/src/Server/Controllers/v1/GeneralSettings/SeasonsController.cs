using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirstCall.Application.Features.Seasons.Commands.AddEdit;
using FirstCall.Application.Features.Seasons.Commands.Delete;
using FirstCall.Application.Features.Seasons.Queries.GetAll;
using FirstCall.Application.Features.Seasons.Queries.GetById;
using FirstCall.Shared.Constants.Permission;
using System.Threading.Tasks;
using FirstCall.Application.Features.Brands.Queries.GetAllPaged;
using FirstCall.Application.Features.Seasons.Queries.GetAllPaged;

namespace FirstCall.Server.Controllers.v1.GeneralSettings
{
    public class SeasonsController : BaseApiController<SeasonsController>
    {
        /// <summary>
        /// Get All Seasons
        /// </summary>
        /// 
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Seasons.View)]
        [HttpGet("GetAllPaged")]
        public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var brands = await _mediator.Send(new GetAllPagedSeasonsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(brands);
        }
        /// <summary>
        /// Get All Seasons
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Seasons.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Seasons = await _mediator.Send(new GetAllSeasonsQuery());
            return Ok(Seasons);
        }

        /// <summary>
        /// Get a Season By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Seasons.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Season = await _mediator.Send(new GetSeasonByIdQuery() { Id = id });
            return Ok(Season);
        }

        /// <summary>
        /// Create/Update a Color
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Seasons.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditSeasonCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Season
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Seasons.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteSeasonCommand { Id = id }));
        }

    }
}
