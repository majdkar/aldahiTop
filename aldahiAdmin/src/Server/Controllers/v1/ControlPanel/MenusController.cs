using FirstCall.Application.Services;
using FirstCall.Shared.ViewModels.Menus;
using FirstCall.Shared.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using FirstCall.Server.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Polly;
using FirstCall.Shared.Constants.Permission;

namespace FirstCall.Api.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class MenusController : ApiControllerBase
    {
        private readonly IMenuService menuService;

        public MenusController(IMenuService menuService)
        {
            this.menuService = menuService;
        }
        //[Authorize(Policy = Permissions.Menues.View)]
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<MenuViewModel>>> Get()
        {
            try
            {
                List<MenuViewModel> data = await menuService.GetMenus();

                if (data != null)
                {
                    return Ok(data.OrderBy(x => x.LevelOrder));
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
        
        [HttpGet]
        [Route("GetAllWithChildern")]
        public async Task<ActionResult<List<MenuViewModel>>> GetWithChildern()
        {
            try
            {
                List<MenuViewModel> data = await menuService.GetMenusWithSubMenu();

                if (data != null)
                {
                    return Ok(data.OrderBy(x => x.LevelOrder));
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

        [Authorize(Policy = Permissions.Menues.View)]
        [HttpGet]
        public async Task<IActionResult> Get(string categoryId, string searchString, string orderBy)
        {
            try
            {
                int convertedCategoryId;
                var isConvertable = Int32.TryParse(categoryId, out convertedCategoryId);

                if (!isConvertable)
                {
                    return BadRequest($"try correct category Id!");
                }

                var filteredData = await menuService.GetPagedMenus(searchString, orderBy);

                if (convertedCategoryId != 0)
                {
                    filteredData = filteredData.Where(x => x.CategoryId == convertedCategoryId).OrderBy(x => x.LevelOrder).ToList();
                }

                var response = new PagedResponse<MenuViewModel>(filteredData, 0, 10, filteredData.Count());
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        [Authorize(Policy = Permissions.Menues.View)]
        [HttpGet]
        [Route("GetMenuMaster")]
        public async Task<IActionResult> GetMenuMaster(string categoryId, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            try
            {
                int convertedCategoryId;
                var isConvertable = Int32.TryParse(categoryId, out convertedCategoryId);

                if (!isConvertable)
                {
                    return BadRequest($"try correct category Id!");
                }

                var filteredData = await menuService.GetPagedMenus(searchString, orderBy);

                if (convertedCategoryId != 0)
                {
                    filteredData = filteredData.Where(x => x.CategoryId == convertedCategoryId && x.ParentId == null).OrderBy(x => x.LevelOrder).ToList();
                }

                if (pageSize == 0) pageSize = 10;
                var pagedData = filteredData
                .Skip((pageNumber) * pageSize)
               .Take(pageSize)
               .ToList();

                var response = new PagedResponse<MenuViewModel>(pagedData, pageNumber, pageSize, filteredData.Count());
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }
        [Authorize(Policy = Permissions.Menues.View)]
        [HttpGet]
        [Route("GetAllMasterOrSubmenu")]
        //[HttpGet("GetAllMasterOrSubmenu")]
        public async Task<IActionResult> GetMaster(int? menuId, string searchString, string orderBy)
        {
            try
            {

                var filteredData = await menuService.GetPagedMenus(searchString, orderBy);
                    filteredData = filteredData.Where(x => x.ParentId == menuId).OrderBy(x => x.LevelOrder).ToList();

                var response = new PagedResponse<MenuViewModel>(filteredData, 0, 10, filteredData.Count());
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        [Authorize(Policy = Permissions.Menues.View)]
        [HttpGet]
        [Route("GetMenuSub")]
        public async Task<IActionResult> GetMenuSub(string categoryId, int menuid, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            try
            {
                int convertedCategoryId;
                var isConvertable = Int32.TryParse(categoryId, out convertedCategoryId);

                if (!isConvertable)
                {
                    return BadRequest($"try correct category Id!");
                }

                var filteredData = await menuService.GetPagedMenus(searchString, orderBy);

                if (convertedCategoryId != 0)
                {
                    filteredData = filteredData.Where(x => x.CategoryId == convertedCategoryId && x.ParentId == menuid).OrderBy(x => x.LevelOrder).ToList();
                }

                if (pageSize == 0) pageSize = 10;
                var pagedData = filteredData
                .Skip((pageNumber) * pageSize)
               .Take(pageSize)
               .ToList();

                var response = new PagedResponse<MenuViewModel>(pagedData, pageNumber, pageSize, filteredData.Count());
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        [Authorize(Policy = Permissions.Menues.View)]
        [HttpGet]
        [Route("NoCategory")]
        public async Task<IActionResult> Get(string searchString, string orderBy)
        {
            try
            {
                var filteredData = await menuService.GetPagedMenus(searchString, orderBy);
                var response = new PagedResponse<MenuViewModel>(filteredData, 0, 10, filteredData.Count());
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        [Authorize(Policy = Permissions.Menues.View)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MenuViewModel>> Get(int id)
        {
            try
            {
                var result = await menuService.GetMenuById(id);

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

        [Authorize(Policy = Permissions.Menues.Create)]
        [HttpPost]
        public async Task<ActionResult<MenuViewModel>> Create(MenuInsertModel menuInsertModel)
        {
            try
            {
                if (menuInsertModel == null)
                    return BadRequest();

                // TODO : implement restriction to prevent adding an existing model 
                var createdMenu = await menuService.AddMenu(menuInsertModel);

                if (createdMenu != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdMenu.Id }, createdMenu);
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

        [Authorize(Policy = Permissions.Menues.Edit)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<MenuViewModel>> Update(int id, MenuUpdateModel menuUpdateModel)
        {
            try
            {
                if (menuUpdateModel.Id != id)
                {
                    return BadRequest("IDs  are not matching");
                }
                //var menuToUpdate = await menuService.GetMenuById(id);

                //if (menuToUpdate == null)
                //    return NotFound($"Record with Id = {id} is not found");

                var updatedMenu = await menuService.UpdateMenu(menuUpdateModel);

                if (updatedMenu != null)
                {
                    return Ok(updatedMenu);
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





        [Authorize(Policy = Permissions.Menues.Delete)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<MenuViewModel>> Delete(int id)
        {
            try
            {
                var menuToDelete = await menuService.GetMenuById(id);

                if (menuToDelete == null)
                {
                    return NotFound($"Menu with Id = {id} not found");
                }

                var result = await menuService.SoftDeleteMenu(id);
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
