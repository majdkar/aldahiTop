using FirstCall.Application.Services;
using FirstCall.Shared.ViewModels.Blocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Polly;
using FirstCall.Shared.Constants.Permission;

namespace FirstCall.Api.Controllers
{
    public class BlockPhotoController : ApiControllerBase
    {
        private readonly IBlockPhotoService photoService;

        public BlockPhotoController(IBlockPhotoService photoService)
        {
            this.photoService = photoService;
        }

        //[Authorize(Policy = Permissions.WebSiteManagement.View)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BlockPhotoViewModel>> Get(int id)
        {
            try
            {
                var result = await photoService.GetPhotoById(id);

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

        //[Authorize(Policy = Permissions.WebSiteManagement.Create)]
        [HttpPost]
        public async Task<ActionResult<BlockPhotoViewModel>> Create(BlockPhotoInsertModel photoInsertModel)
        {
            try
            {
                if (photoInsertModel == null)
                    return BadRequest();

                var createdPhoto = await photoService.AddPhoto(photoInsertModel);

                if (createdPhoto != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdPhoto.Id }, createdPhoto);
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

        //[Authorize(Policy = Permissions.WebSiteManagement.Edit)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<BlockPhotoViewModel>> Update(int id, BlockPhotoUpdateModel photoUpdateModel)
        {
            try
            {
                if (photoUpdateModel.Id != id)
                {
                    return NotFound("IDs are not matching");
                }
                var transaltionToUpdate = await photoService.GetPhotoById(id);

                if (transaltionToUpdate == null)
                    return NotFound($"Record with Id = {photoUpdateModel.Id} not found");

                var updatedTransaltion = await photoService.UpdatePhoto(photoUpdateModel);

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

        //[Authorize(Policy = Permissions.WebSiteManagement.Delete)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<BlockPhotoViewModel>> Delete(int id)
        {
            try
            {
                var transaltionToDelete = await photoService.GetPhotoById(id);

                if (transaltionToDelete == null)
                {
                    return NotFound($"Transaltion with Id = {id} not found");
                }

                var result = await photoService.SoftDeletePhoto(id);
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
