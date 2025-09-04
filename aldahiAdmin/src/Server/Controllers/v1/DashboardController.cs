using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FirstCall.Application.Features.Dashboards.Queries.GetData;
using FirstCall.Shared.Constants.Permission;

namespace FirstCall.Server.Controllers.v1
{
    [ApiController]
    public class DashboardController : BaseApiController<DashboardController>
    {
        /// <summary>
        /// Get Dashboard Data
        /// </summary>
        /// <returns>Status 200 OK </returns>
        [Authorize(Policy = Permissions.Dashboards.View)]
        [HttpGet]
        public async Task<IActionResult> GetDataAsync()
        {
            var result = await _mediator.Send(new GetDashboardDataQuery());
            return Ok(result);
        }


        /// <summary>
        /// Get Dashboard Data
        /// </summary>
        /// <returns>Status 200 OK </returns>
        [AllowAnonymous]
        [HttpGet("GetDataProduct")]
        public async Task<IActionResult> GetDataProductAsync()
        {
            var result = await _mediator.Send(new GetDashboardDataProductQuery());
            return Ok(result);
        }
    }
}