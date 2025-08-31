using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirstCall.Application.Features.Countries.Commands.AddEdit;
using FirstCall.Application.Features.Countries.Commands.Delete;
using FirstCall.Application.Features.Countries.Queries.Export;
using FirstCall.Application.Features.Countries.Queries.GetAll;
using FirstCall.Application.Features.Countries.Queries.GetById;
using FirstCall.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace FirstCall.Server.Controllers.v1.GeneralSettings
{
    public class CountriesController : BaseApiController<CountriesController>
    {
        /// <summary>
        /// Get All Countriess
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Countries.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var countriess = await _mediator.Send(new GetAllCountriesQuery());
            return Ok(countriess);
        }

        /// <summary>
        /// Get a Country By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Countries.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var country = await _mediator.Send(new GetCountryByIdQuery() { Id = id });
            return Ok(country);
        }

        /// <summary>
        /// Create/Update a Country
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Countries.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCountryCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Country
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Countries.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteCountryCommand { Id = id }));
        }

        /// <summary>
        /// Search Countries and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Countries.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportCountriesQuery(searchString)));
        }
    }
}
