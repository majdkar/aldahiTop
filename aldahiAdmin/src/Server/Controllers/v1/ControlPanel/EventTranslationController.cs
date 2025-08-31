using FirstCall.Application.Services;
using FirstCall.Shared.ViewModels.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Polly;
using FirstCall.Shared.Constants.Permission;

namespace FirstCall.Api.Controllers
{
    public class EventTranslationController : ApiControllerBase
    {
        private readonly IEventTranslationService translationService;

        public EventTranslationController(IEventTranslationService translationService)
        {
            this.translationService = translationService;
        }
        [Authorize(Policy = Permissions.WebSiteManagement.View)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EventTranslationViewModel>> Get(int id)
        {
            try
            {
                var result = await translationService.GetTranslationById(id);

                if (result == null)
                    return NotFound();

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        [Authorize(Policy = Permissions.WebSiteManagement.Create)]
        [HttpPost]
        public async Task<ActionResult<EventTranslationViewModel>> Create(EventTranslationInsertModel translationInsertModel)
        {
            try
            {
                if (translationInsertModel == null)
                    return BadRequest();

                var createdTranslation = await translationService.AddTranslation(translationInsertModel);

                if (createdTranslation != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdTranslation.Id }, createdTranslation);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new record");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new record");
            }
        }

        [Authorize(Policy = Permissions.WebSiteManagement.Edit)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<EventTranslationViewModel>> Update(int id, EventTranslationUpdateModel translationUpdateModel)
        {
            try
            {
                if (translationUpdateModel.Id != id)
                {
                    return NotFound("IDs are not matching");
                }
                var transaltionToUpdate = await translationService.GetTranslationById(id);

                if (transaltionToUpdate == null)
                    return NotFound($"Record with Id = {translationUpdateModel.Id} not found");

                var updatedTransaltion = await translationService.UpdateTranslation(translationUpdateModel);

                if (updatedTransaltion != null)
                {
                    return Ok(updatedTransaltion);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error updating data");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        [Authorize(Policy = Permissions.WebSiteManagement.Delete)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<EventTranslationViewModel>> Delete(int id)
        {
            try
            {
                var transaltionToDelete = await translationService.GetTranslationById(id);

                if (transaltionToDelete == null)
                {
                    return NotFound($"Transaltion with Id = {id} not found");
                }

                var result = await translationService.SoftDeleteTranslation(id);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

    }
}
