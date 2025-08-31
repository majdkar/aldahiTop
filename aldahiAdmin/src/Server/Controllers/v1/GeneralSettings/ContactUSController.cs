using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FirstCall.Application.Features.ContactUs.Commands.AddEdit;

namespace FirstCall.Server.Controllers.v1.GeneralSettings
{
    public class ContactUSController : BaseApiController<ContactUSController>
    {

        /// <summary>
        /// Send Form Contact US
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(SendContactUsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
