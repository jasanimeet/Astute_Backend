using astute.CoreModel;
using astute.Models;
using astute.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace astute.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class MenuController : ControllerBase
    {
        #region Fields
        private readonly IMenuService _menuService;
        #endregion

        #region Ctor
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }
        #endregion

        #region Methods
        [HttpGet]
        [Route("getrecursivemenu")]
        [Authorize]
        public async Task<IActionResult> GetRecursiveMenu(int employeeId)
        {
            try
            {
                var result = await _menuService.Get_all_menus(employeeId);
                if(result != null && result.Count > 0)
                {
                    return Ok(new 
                    {
                        StatusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        [Route("getmenu")]
        [Authorize]
        public async Task<IActionResult> GetMenu(int menuId)
        {
            try
            {
                var result = await _menuService.GetMenu(menuId);
                if(result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [Route("createmenu")]
        [Authorize]
        public async Task<IActionResult> CreateMenu(Menu_Mas menu_Mas)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var result = await _menuService.InsertMenu(menu_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.MenuMasterCreated
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.MenuMasterExists,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch
            {
                throw;
            }
        }

        [HttpPut]
        [Route("updatemenu")]
        [Authorize]
        public async Task<IActionResult> UpdateMenu(Menu_Mas menu_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _menuService.UpdateMenu(menu_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.MenuMasterUpdated
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.MenuMasterExists,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("deletemenu")]
        [Authorize]
        public async Task<IActionResult> DeleteMenu(int menuId)
        {
            try
            {
                var result = await _menuService?.DeleteMenu(menuId);
                if(result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.MenuMasterDeleted
                    });
                }
                return BadRequest(new 
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
