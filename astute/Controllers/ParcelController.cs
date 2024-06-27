using astute.CoreModel;
using astute.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using System;
using astute.Repository;
using Microsoft.Extensions.Configuration;

namespace astute.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParcelController : ControllerBase
    {
        #region Fields
        private readonly IConfiguration _configuration;
        private readonly ICommonService _commonService;
        private readonly IParcel_Master_Service _parcel_Master_Service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthentication _jWTAuthentication;
        #endregion

        #region Ctor
        public ParcelController(
            IConfiguration configuration,
            ICommonService commonService,
            IParcel_Master_Service parcel_Master_Service,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthentication jWTAuthentication)
        {
            _configuration = configuration;
            _commonService = commonService;
            _parcel_Master_Service = parcel_Master_Service;
            _httpContextAccessor = httpContextAccessor;
            _jWTAuthentication = jWTAuthentication;
        }
        #endregion

        #region Methods
        #region Parcel Master

        [HttpGet]
        [Route("get_parcel_master")]
        [Authorize]
        public async Task<IActionResult> Get_Parcel_Master(int parcel_Id)
        {
            try
            {
                var result = await _parcel_Master_Service.Get_Parcel_Master(parcel_Id);
                if (result != null && result.Count > 0)
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Parcel_Master", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_parcel_master")]
        [Authorize]
        public async Task<IActionResult> Create_Parcel_Master(Parcel_Master parcel_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _parcel_Master_Service.Insert_Parcel_Master(parcel_Master);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ParcelMasterCreated
                        });
                    }
                    else if (result == 5)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.IsExistParcelMaster
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Parcel_Master", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("update_parcel_master")]
        [Authorize]
        public async Task<IActionResult> Update_Parcel_Master(Parcel_Master parcel_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _parcel_Master_Service.Update_Parcel_Master(parcel_Master);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ParcelMasterUpdated
                        });
                    }
                    else if (result == 5)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.IsExistParcelMaster
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Update_Parcel_Master", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_parcel_master")]
        [Authorize]
        public async Task<IActionResult> Delete_Parcel_Master(int parcel_Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _parcel_Master_Service.Delete_Parcel_Master(parcel_Id);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ParcelMasterDeleted
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Parcel_Master", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        #endregion
        #endregion
    }
}
