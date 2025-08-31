using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirstCall.Application.Features.Kinds.Commands.AddEdit;
using FirstCall.Application.Features.Kinds.Commands.Delete;
using FirstCall.Application.Features.Kinds.Queries.GetAll;
using FirstCall.Application.Features.Kinds.Queries.GetById;
using FirstCall.Shared.Constants.Permission;
using System.Threading.Tasks;
using FirstCall.Application.Features.Brands.Queries.GetAllPaged;
using FirstCall.Application.Features.Kinds.Queries.GetAllPaged;

namespace FirstCall.Server.Controllers.v1.GeneralSettings
{
    public class KindsController : BaseApiController<KindsController>
    {
        /// <summary>
        /// Get All Kinds
        /// </summary>
        /// 
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Kinds.View)]
        [HttpGet("GetAllPaged")]
        public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var brands = await _mediator.Send(new GetAllPagedKindsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(brands);
        }
        /// <summary>
        /// Get All Kinds
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Kinds.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Kinds = await _mediator.Send(new GetAllKindsQuery());
            return Ok(Kinds);
        }

        /// <summary>
        /// Get a Kind By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Kinds.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Kind = await _mediator.Send(new GetKindByIdQuery() { Id = id });
            return Ok(Kind);
        }

        /// <summary>
        /// Create/Update a Color
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Kinds.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditKindCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Kind
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Kinds.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteKindCommand { Id = id }));
        }

    }
}
