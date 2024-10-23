using astute.CoreModel;
using astute.Models;
using astute.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly ICommonService _commonService;
        #endregion

        #region Ctor
        public MenuController(IMenuService menuService, ICommonService commonService)
        {
            _menuService = menuService;
            _commonService = commonService;
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
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetRecursiveMenu", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
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
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetMenu", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
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
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateMenu", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
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
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateMenu", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
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
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "DeleteMenu", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_menu_max_order_no")]
        [Authorize]
        public async Task<IActionResult> Get_Menu_Max_Order_No()
        {
            try
            {
                var result = await _menuService.Get_Menu_Max_Order_No();

                return Ok(new
                {
                    statusCode = HttpStatusCode.OK,
                    order_no = result
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Menu_Max_Order_No", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion
    }
}
