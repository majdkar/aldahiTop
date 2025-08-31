using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Threading.Tasks;
using System;
using FirstCall.Api.Controllers;
using FirstCall.Shared.Constants.Permission;
using FirstCall.Shared.ViewModels.Blocks;
using FirstCall.Server.Services.Blocks;

namespace FirstCall.Server.Controllers.v1.ControlPanel
{
    public class BlockVideoController : ApiControllerBase
    {
        private readonly IBlockVideoService VideoService;

        public BlockVideoController(IBlockVideoService VideoService)
        {
            this.VideoService = VideoService;
        }


        [Authorize(Policy = Permissions.Blocks.View)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BlockVideoViewModel>> Get(int id)
        {
            try
            {
                var result = await VideoService.GetVideoById(id);

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

        [Authorize(Policy = Permissions.Blocks.Create)]
        [HttpPost]
        public async Task<ActionResult<BlockVideoViewModel>> Create(BlockVideoInsertModel VideoInsertModel)
        {
            try
            {
                if (VideoInsertModel == null)
                    return BadRequest();

                var createdVideo = await VideoService.AddVideo(VideoInsertModel);

                if (createdVideo != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdVideo.Id }, createdVideo);
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

        [Authorize(Policy = Permissions.Blocks.Edit)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<BlockVideoViewModel>> Update(int id, BlockVideoUpdateModel VideoUpdateModel)
        {
            try
            {
                if (VideoUpdateModel.Id != id)
                {
                    return NotFound("IDs are not matching");
                }
                var transaltionToUpdate = await VideoService.GetVideoById(id);

                if (transaltionToUpdate == null)
                    return NotFound($"Record with Id = {VideoUpdateModel.Id} not found");

                var updatedTransaltion = await VideoService.UpdateVideo(VideoUpdateModel);

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

        [Authorize(Policy = Permissions.Blocks.Delete)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<BlockVideoViewModel>> Delete(int id)
        {
            try
            {
                var transaltionToDelete = await VideoService.GetVideoById(id);

                if (transaltionToDelete == null)
                {
                    return NotFound($"Transaltion with Id = {id} not found");
                }

                var result = await VideoService.SoftDeleteVideo(id);
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
