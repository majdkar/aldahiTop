using FirstCall.Application.Features.Nations.Queries.GetAll;
using FirstCall.Application.Features.Nations.Queries.GetById;
using FirstCall.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FirstCall.Application.Features.Nations.Commands.AddEdit;
using FirstCall.Application.Features.Nations.Commands.Delete;
using FirstCall.Application.Features.Nations.Queries.Export;

namespace FirstCall.Server.Controllers.v1.GeneralSettings
{
    public class NationsController : BaseApiController<NationsController>
    {
        /// <summary>
        /// Get All Nations
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Nations.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var nations = await _mediator.Send(new GetAllNationsQuery());
            return Ok(nations);
        }

        /// <summary>
        /// Get a Nation By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Nations.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var nation = await _mediator.Send(new GetNationByIdQuery() { Id = id });
            return Ok(nation);
        }

        /// <summary>
        /// Create/Update a Nation
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Nations.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditNationCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Nation
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Nations.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteNationCommand { Id = id }));
        }

        /// <summary>
        /// Search Nations and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Nations.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportNationsQuery(searchString)));
        }
    }
}