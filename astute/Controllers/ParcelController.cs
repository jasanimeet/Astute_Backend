using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using astute.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
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

        #region Stock Allocation

        [HttpGet]
        [Route("get_stock_allocation")]
        [Authorize]
        public async Task<IActionResult> Get_Stock_Allocation(int ac_Grp_Code, int company_Id, int year_Id)
        {
            try
            {
                var result = await _parcel_Master_Service.Get_Stock_Allocation(ac_Grp_Code, company_Id, year_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Stock_Allocation", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_update_stock_allocation")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Stock_Allocation(IList<Stock_Allocation> stock_Allocation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DataTable table = new DataTable();

                    table.Columns.Add("Id", typeof(int));
                    table.Columns.Add("Parcel_Id", typeof(int));
                    table.Columns.Add("Parcel_Ref_Id", typeof(int));
                    table.Columns.Add("Ac_Grp_Code", typeof(int));
                    table.Columns.Add("Trans_Type", typeof(string));
                    table.Columns.Add("PCS", typeof(decimal));
                    table.Columns.Add("CTS", typeof(decimal));
                    table.Columns.Add("Amount_In_US($)", typeof(decimal));
                    table.Columns.Add("Company_Id", typeof(int));
                    table.Columns.Add("Year_Id", typeof(int));

                    foreach (var item in stock_Allocation)
                    {
                        table.Rows.Add(
                            item.Id ?? (object)DBNull.Value,
                            item.Parcel_Id ?? (object)DBNull.Value,
                            item.Parcel_Ref_Id ?? (object)DBNull.Value,
                            item.Ac_Grp_Code ?? (object)DBNull.Value,
                            item.Trans_Type ?? (object)DBNull.Value,
                            SafeConvertToDouble(item.PCS) ?? (object)DBNull.Value,
                            SafeConvertToDouble(item.CTS) ?? (object)DBNull.Value,
                            SafeConvertToDouble(item.Amount_In_US) ?? (object)DBNull.Value,
                            item.Company_Id ?? (object)DBNull.Value,
                            item.Year_Id ?? (object)DBNull.Value
                        );
                    }

                    var result = await _parcel_Master_Service.Insert_Update_Stock_Allocation(table);

                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.Stock_Allocation_Saved
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Stock_Allocation", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_stock_allocation")]
        [Authorize]
        public async Task<IActionResult> Delete_Stock_Allocation(int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _parcel_Master_Service.Delete_Stock_Allocation(Id);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.Stock_Allocation_Deleted
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Stock_Allocation", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }
        public static double? SafeConvertToDouble(string value)
        {
            if (double.TryParse(value, out double result))
            {
                return result;
            }
            return null;
        }
        #endregion

        #endregion
    }
}
