using System;
using FirstCall.Application.Features.Princedoms.Queries.GetAll;
using FirstCall.Application.Features.Princedoms.Queries.GetById;
using FirstCall.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FirstCall.Application.Features.Princedoms.Commands.AddEdit;
using FirstCall.Application.Features.Princedoms.Commands.Delete;
using FirstCall.Application.Features.Princedoms.Queries.Export;

namespace FirstCall.Server.Controllers.v1.GeneralSettings
{
    public class PrincedomsController : BaseApiController<PrincedomsController>
    {
        /// <summary>
        /// Get All Princedoms
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Princedoms.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var princedoms = await _mediator.Send(new GetAllPrincedomsQuery());
            return Ok(princedoms);
        }

        /// <summary>
        /// Get a Princedom By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Princedoms.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var princedom = await _mediator.Send(new GetPrincedomByIdQuery() { Id = id });
            return Ok(princedom);
        }

        /// <summary>
        /// Create/Update a Princedom
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Princedoms.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditPrincedomCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Princedom
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Princedoms.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeletePrincedomCommand { Id = id }));
        }

        /// <summary>
        /// Search Princedoms and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Princedoms.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportPrincedomsQuery(searchString)));
        }
    }
}