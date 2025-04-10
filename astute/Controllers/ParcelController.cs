using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using astute.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading.Tasks;

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
        [Route("create_update_parcel_master")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Parcel_Master(Parcel_Master parcel_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _parcel_Master_Service.Insert_Update_Parcel_Master(parcel_Master);
                    if (result == 1)
                    {
                        if (parcel_Master.Parcel_Id > 0)
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.ParcelMasterUpdated
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.ParcelMasterCreated
                            });
                        }
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
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Parcel_Master", ex.StackTrace);
                return Conflict(new
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
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_parcel_master_by_cat_val_id")]
        [Authorize]
        public async Task<IActionResult> Get_Parcel_Master_By_Cat_Val_Id(int cat_Val_Id)
        {
            try
            {
                var result = await _parcel_Master_Service.Get_Parcel_Master_By_Cat_Val_Id(cat_Val_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Parcel_Master_By_Cat_Val_Id", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        #endregion

        #region Parcel Ref Master
        [HttpGet]
        [Route("get_parcel_ref_master")]
        [Authorize]
        public async Task<IActionResult> Get_Parcel_Ref_Master(int parcel_Ref_Id)
        {
            try
            {
                var result = await _parcel_Master_Service.Get_Parcel_Ref_Master(parcel_Ref_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Parcel_Ref_Master", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_update_parcel_ref_master")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Parcel_Ref_Master(Parcel_Ref_Master parcel_Ref_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);

                    var result = await _parcel_Master_Service.Insert_Update_Parcel_Ref_Master(parcel_Ref_Master, user_Id ?? 0);
                    if (result == 1)
                    {
                        if (parcel_Ref_Master.Parcel_Ref_Id > 0)
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.ParcelRefMasterUpdated
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.ParcelRefMasterCreated
                            });
                        }
                    }
                    else if (result == 5)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.IsExistParcelRefMaster
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Parcel_Ref_Master", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_parcel_ref_master")]
        [Authorize]
        public async Task<IActionResult> Delete_Parcel_Ref_Master(int parcel_ref_Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);

                    var result = await _parcel_Master_Service.Delete_Parcel_Ref_Master(parcel_ref_Id, user_Id ?? 0);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ParcelRefMasterDeleted
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Parcel_Ref_Master", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_parcel_ref")]
        [Authorize]
        public async Task<IActionResult> Get_Parcel_Ref()
        {
            try
            {
                var result = await _parcel_Master_Service.Get_Parcel_Ref();
                if (result != null)
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Parcel_Ref", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        #endregion
        #endregion
    }
}
