using FirstCall.Application.Services;
using FirstCall.Shared.ViewModels.Blocks;
using FirstCall.Shared.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Polly;
using FirstCall.Shared.Constants.Permission;

namespace FirstCall.Api.Controllers
{
    public class BlocksController : ApiControllerBase
    {
        private readonly IBlockService blockService;

        public BlocksController(IBlockService blockService)
        {
            this.blockService = blockService;
        }



        //[Authorize(Policy = Permissions.WebSiteManagement.View)]
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var data = await blockService.GetBlocks();

                if (data != null)
                {
                    return Ok(data);
                }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error retrieving data");

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        //[Authorize(Policy = Permissions.WebSiteManagement.View)]
        [HttpGet]
        public async Task<ActionResult> Get(string categoryId, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            try
            {
                int convertedCategoryId;
                var isConvertable = Int32.TryParse(categoryId, out convertedCategoryId);

                if (!isConvertable)
                {
                    return BadRequest($"try correct category Id!");
                }

                var filteredData = await blockService.GetPagedBlocks(searchString, orderBy);

                if (convertedCategoryId != 0)
                {
                    filteredData = filteredData.Where(x => x.CategoryId == convertedCategoryId).ToList();
                }
                if (pageSize == 0) pageSize = 10;
                var pagedData = filteredData
                .Skip((pageNumber) * pageSize)
               .Take(pageSize)
               .ToList();

                var response = new PagedResponse<BlockViewModel>(pagedData, pageNumber, pageSize, filteredData.Count());
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        //[Authorize(Policy = Permissions.WebSiteManagement.View)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BlockViewModel>> Get(int id)
        {
            try
            {
                var result = await blockService.GetBlockById(id);

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
        public async Task<ActionResult<BlockViewModel>> Create(BlockInsertModel blockInsertModel)
        {
            try
            {
                if (blockInsertModel == null)
                    return BadRequest();

                // TODO : implement restriction to prevent adding an existing  model 
                var createdBlock = await blockService.AddBlock(blockInsertModel);

                if (createdBlock != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdBlock.Id }, createdBlock);
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
        public async Task<ActionResult<BlockViewModel>> Update(int id, BlockUpdateModel blockUpdateModel)
        {
            try
            {
                if (blockUpdateModel.Id != id)
                {
                    return BadRequest("IDs  are not matching");
                }
                var blockToUpdate = await blockService.GetBlockById(id);

                if (blockToUpdate == null)
                    return NotFound($"Record with Id = {id} is not found");

                var updatedBlock = await blockService.UpdateBlock(blockUpdateModel);

                if (updatedBlock != null)
                {
                    return Ok(updatedBlock);
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
        //[Route("{id:int}")]
        //[HttpPatch]
        public async Task<ActionResult<BlockViewModel>> Delete(int id)
        {
            try
            {
                var blockToDelete = await blockService.GetBlockById(id);

                if (blockToDelete == null)
                {
                    return NotFound($"Record with Id = {id} not found");
                }

                var result = await blockService.SoftDeleteBlock(id);
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
