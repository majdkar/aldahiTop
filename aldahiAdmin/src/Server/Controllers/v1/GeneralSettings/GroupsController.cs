using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirstCall.Application.Features.Groups.Commands.AddEdit;
using FirstCall.Application.Features.Groups.Commands.Delete;
using FirstCall.Application.Features.Groups.Queries.GetAll;
using FirstCall.Application.Features.Groups.Queries.GetById;
using FirstCall.Shared.Constants.Permission;
using System.Threading.Tasks;
using FirstCall.Application.Features.Brands.Queries.GetAllPaged;
using FirstCall.Application.Features.Groups.Queries.GetAllPaged;

namespace FirstCall.Server.Controllers.v1.GeneralSettings
{
    public class GroupsController : BaseApiController<GroupsController>
    {
        /// <summary>
        /// Get All Groups
        /// </summary>
        /// 
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Groups.View)]
        [HttpGet("GetAllPaged")]
        public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var brands = await _mediator.Send(new GetAllPagedGroupsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(brands);
        }
        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Groups.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Groups = await _mediator.Send(new GetAllGroupsQuery());
            return Ok(Groups);
        }

        /// <summary>
        /// Get a Group By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Groups.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Group = await _mediator.Send(new GetGroupByIdQuery() { Id = id });
            return Ok(Group);
        }

        /// <summary>
        /// Create/Update a Color
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Groups.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditGroupCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Group
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Groups.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteGroupCommand { Id = id }));
        }

    }
}
