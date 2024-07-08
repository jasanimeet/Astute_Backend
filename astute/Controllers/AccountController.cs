using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using astute.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace astute.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        #region Fields
        private readonly IConfiguration _configuration;
        private readonly ICommonService _commonService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthentication _jWTAuthentication;
        private readonly IAccount_Group_Service _account_Group_Service;
        private readonly IAccount_Master_Service _account_Master_Service;
        private readonly IFirst_Voucher_No _trans_Service;
        private readonly IAccount_Trans_Master_Service _account_Trans_Master_Service;
        private readonly ICategoryService _categoryService;
        #endregion

        #region Ctor
        public AccountController(
            IConfiguration configuration,
            ICommonService commonService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthentication jWTAuthentication,
            IAccount_Group_Service account_Group_Service,
            IAccount_Master_Service account_Master_Service,
            IFirst_Voucher_No trans_Service,
            IAccount_Trans_Master_Service account_Trans_Master_Service,
            ICategoryService categoryService)
        {
            _configuration = configuration;
            _commonService = commonService;
            _httpContextAccessor = httpContextAccessor;
            _jWTAuthentication = jWTAuthentication;
            _account_Group_Service = account_Group_Service;
            _account_Master_Service = account_Master_Service;
            _trans_Service = trans_Service;
            _account_Trans_Master_Service = account_Trans_Master_Service;
            _categoryService = categoryService;
        }
        #endregion

        #region Account Group Master
        [HttpGet]
        [Route("get_account_group")]
        [Authorize]
        public async Task<IActionResult> Get_Account_Group(int ac_Group_Code)
        {
            try
            {
                var result = await _account_Group_Service.Get_Account_Group(ac_Group_Code);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Account_Group", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_account_group")]
        [Authorize]
        public async Task<IActionResult> Create_Account_Group(Account_Group_Master account_Group_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                    var (msg, result) = await _account_Group_Service.Create_Update_Account_Group(account_Group_Master, user_Id ?? 0);
                    if (msg == "success" && result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.AccountGroupCreated
                        });
                    }
                    else if (msg == "exist" && result == 409)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.AccountGroupAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Account_Group", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("update_account_group")]
        [Authorize]
        public async Task<IActionResult> Update_Account_Group(Account_Group_Master account_Group_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                    var (msg, result) = await _account_Group_Service.Create_Update_Account_Group(account_Group_Master, user_Id ?? 0);
                    if (msg == "success" && result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.AccountGroupUpdated
                        });
                    }
                    else if (msg == "exist" && result == 409)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.AccountGroupAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Update_Account_Group", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_account_group")]
        [Authorize]
        public async Task<IActionResult> Delete_Account_Group(int ac_Group_Code)
        {
            try
            {
                var result = await _account_Group_Service.Delete_Account_Group(ac_Group_Code);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.AccountGroupDeleted
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Account_Group", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("account_group_excel_download")]
        [Authorize]
        public async Task<IActionResult> Account_Group_Excel_Download()
        {
            try
            {
                var dt_acc_group = await _account_Group_Service.Get_Account_Group_Excel();
                if (dt_acc_group != null && dt_acc_group.Rows.Count > 0)
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in dt_acc_group.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    var excelPath = string.Empty;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                    if (!(Directory.Exists(filePath)))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string filename = string.Empty;

                    filename = "Account_Group_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    EpExcelExport.Create_Account_Group_Excel(dt_acc_group, columnNamesTable, filePath, filePath + filename);
                    excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;

                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        result = excelPath,
                        file_name = filename
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Account_Group_Excel_Download", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_perent_account_group")]
        [Authorize]
        public async Task<IActionResult> Get_Perent_Account_Group()
        {
            try
            {
                var result = await _account_Group_Service.Get_Perent_Account_Group();
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
                await _commonService.InsertErrorLog(ex.Message, "get_perent_account_group", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_sub_account_group")]
        [Authorize]
        public async Task<IActionResult> Get_Sub_Account_Group(int Id)
        {
            try
            {
                var result = await _account_Group_Service.Get_Sub_Account_Group(Id);
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
                await _commonService.InsertErrorLog(ex.Message, "get_sub_account_group", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Account Master

        [HttpGet]
        [Route("get_account_master")]
        [Authorize]
        public async Task<IActionResult> Get_Account_Master(int account_Id)
        {
            try
            {
                var result = await _account_Master_Service.Get_Account_Master(account_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Account_Master", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_update_account_master")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Account_Master(Account_Master account_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    account_Master.User_Id = _jWTAuthentication.Validate_Jwt_Token(token);

                    var (msg, result) = await _account_Master_Service.Create_Update_Account_Master(account_Master);
                    if (msg == "success" && result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = account_Master.Account_Id == 0 ? CoreCommonMessage.AccountMasterCreated : CoreCommonMessage.AccountMasterUpdated
                        });
                    }
                    else if (msg == "exist" && result == 409)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.AccountGroupAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Account_Master", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_account_master")]
        [Authorize]
        public async Task<IActionResult> Delete_Account_Master(int account_Id)
        {
            try
            {
                var result = await _account_Master_Service.Delete_Account_Master(account_Id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.AccountMasterDeleted
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Account_Master", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPost]
        [Route("account_master_excel_download")]
        [Authorize]
        public async Task<IActionResult> Account_Master_Excel_Download()
        {
            try
            {
                var dt_acc_group = await _account_Master_Service.Get_Account_Master_Excel();
                if (dt_acc_group != null && dt_acc_group.Rows.Count > 0)
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in dt_acc_group.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    var excelPath = string.Empty;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                    if (!(Directory.Exists(filePath)))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string filename = string.Empty;

                    filename = "Account_Master_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    EpExcelExport.Create_Account_Group_Excel(dt_acc_group, columnNamesTable, filePath, filePath + filename);
                    excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;

                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        result = excelPath,
                        file_name = filename
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Account_Master_Excel_Download", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        #endregion

        #region First Voucher No

        [HttpGet]
        [Route("get_first_voucher_no_master")]
        [Authorize]
        public async Task<IActionResult> Get_First_Voucher_No_Master(int Id)
        {
            try
            {
                var result = await _trans_Service.Get_First_Voucher_No_Master(Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_First_Voucher_No_Master", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_update_first_voucher_no_master")]
        [Authorize]
        public async Task<IActionResult> Create_Update_First_Voucher_No_Master(IList<Trans_Model> trans_Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (trans_Model != null && trans_Model.Count > 0)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("Id", typeof(int));
                        dataTable.Columns.Add("Trans_Type", typeof(int));
                        dataTable.Columns.Add("Prefix", typeof(string));
                        dataTable.Columns.Add("Voucher_No", typeof(string));
                        dataTable.Columns.Add("Post_Prefix", typeof(bool));
                        dataTable.Columns.Add("Year_Reset", typeof(bool));

                        foreach (var item in trans_Model)
                        {
                            dataTable.Rows.Add(item.Id, item.Trans_Type, item.Prefix, item.Voucher_No, item.Post_Prefix, item.Year_Reset);
                        }

                        var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                        int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);

                        var (message, is_exists, is_Prefix_exists, result) = await _trans_Service.Create_Update_First_Voucher_No_Master(dataTable, user_Id);

                        if ((message == "exist" && (is_exists == true || is_Prefix_exists == true)))
                        {
                            if (is_exists == true && is_Prefix_exists == true && result > 0)
                            {
                                return Ok(new
                                {
                                    statusCode = HttpStatusCode.OK,
                                    message = trans_Model.Any(X => X.Id == 0) == true ? CoreCommonMessage.TransExistsTransPrefixOMasterCreated : CoreCommonMessage.TransExistsTransPrefixOMasterUpdated
                                });
                            }
                            else if (is_exists == true && is_Prefix_exists == true && result == 0)
                            {
                                return Ok(new
                                {
                                    statusCode = HttpStatusCode.OK,
                                    message = trans_Model.Any(X => X.Id == 0) == true ? CoreCommonMessage.TransExistsTransPrefixMasterCreated : CoreCommonMessage.TransExistsTransPrefixMasterUpdated
                                });
                            }
                            else if (is_exists == true && is_Prefix_exists == false && result > 0)
                            {
                                return Ok(new
                                {
                                    statusCode = HttpStatusCode.OK,
                                    message = trans_Model.Any(X => X.Id == 0) == true ? CoreCommonMessage.TransExistsMasterOCreated : CoreCommonMessage.TransExistsMasterOUpdated
                                });
                            }
                            else if (is_exists == true && is_Prefix_exists == false && result == 0)
                            {
                                return Ok(new
                                {
                                    statusCode = HttpStatusCode.OK,
                                    message = CoreCommonMessage.TransExistsMaster
                                });
                            }
                            else if (is_exists == false && is_Prefix_exists == true && result > 0)
                            {
                                return Ok(new
                                {
                                    statusCode = HttpStatusCode.OK,
                                    message = trans_Model.Any(X => X.Id == 0) == true ? CoreCommonMessage.TransExistsPrefixOMasterCreated : CoreCommonMessage.TransExistsPrefixOMasterUpdated
                                });
                            }
                            else if (is_exists == false && is_Prefix_exists == true && result == 0)
                            {
                                return Ok(new
                                {
                                    statusCode = HttpStatusCode.OK,
                                    message = CoreCommonMessage.TransExistsMaster
                                });
                            }
                        }
                        else if ((message == "success") && result > 0)
                        {
                            var result_data = await _trans_Service.Get_First_Voucher_No_Master(0);
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = trans_Model.Any(x => x.Id == 0) == true ? CoreCommonMessage.TransMasterCreated : CoreCommonMessage.TransMasterUpdated,
                                data = result_data
                            });
                        }
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_First_Voucher_No_Master", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_first_voucher_no_master")]
        [Authorize]
        public async Task<IActionResult> Delete_First_Voucher_No_Master(string Id)
        {
            try
            {
                var result = await _trans_Service.Delete_First_Voucher_No_Master(Id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.TransMasterDeleted
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_First_Voucher_No_Master", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Account Transaction Master

        [HttpGet]
        [Route("get_account_master_active_select")]
        [Authorize]
        public async Task<IActionResult> Get_Account_Master_Active_Select(string? trans_Type, string? rec_Type, int account_Id)
        {
            try
            {
                var result = await _account_Trans_Master_Service.Get_Account_Master_Active_Select(trans_Type, rec_Type, account_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Account_Master_Active_Select", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_currency_master_exchange_rate_select")]
        [Authorize]
        public async Task<IActionResult> Get_Currency_Master_Exchange_Rate_Select()
        {
            try
            {
                var result = await _account_Trans_Master_Service.Get_Currency_Master_Exchange_Rate_Select();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Currency_Master_Exchange_Rate_Select", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }


        [HttpGet]
        [Route("get_account_trans_master")]
        [Authorize]
        public async Task<IActionResult> Get_Account_Trans_Master(int? account_Trans_Id, int? account_Trans_Detail_Id,string trans_Type)
        {
            try
            {
                var result = await _account_Trans_Master_Service.Get_Account_Trans_Master(account_Trans_Id ?? 0, account_Trans_Detail_Id ?? 0, trans_Type);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Account_Trans_Master", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPost]
        [Route("create_update_account_trans_master")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Account_Trans_Master(Account_Trans_Master account_Trans_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (account_Trans_Master.account_Trans_Detail != null && account_Trans_Master.account_Trans_Detail.Count > 0)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("Voucher_No", typeof(string));
                        dataTable.Columns.Add("By_Account", typeof(int));
                        dataTable.Columns.Add("By_Type", typeof(string));
                        dataTable.Columns.Add("To_Account", typeof(int));
                        dataTable.Columns.Add("To_Type", typeof(string));
                        dataTable.Columns.Add("Amount", typeof(decimal));
                        dataTable.Columns.Add("Narration", typeof(string));

                        foreach (var item in account_Trans_Master.account_Trans_Detail)
                        {
                            dataTable.Rows.Add(!string.IsNullOrEmpty(item.voucherNo) ? item.voucherNo : DBNull.Value, account_Trans_Master.account, account_Trans_Master.type, item.account, item.type, item.amount, !string.IsNullOrEmpty(item.narration) ? item.narration : DBNull.Value);
                        }

                        var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                        int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);

                        var (message, result) = await _account_Trans_Master_Service.Create_Update_Account_Trans_Master(dataTable, account_Trans_Master.account_Trans_Id, account_Trans_Master.mod_Type, account_Trans_Master.invoice_No, account_Trans_Master.currency, account_Trans_Master.company, account_Trans_Master.year, account_Trans_Master.account, account_Trans_Master.rate, user_Id ?? 0);

                        if (message == "not_exists" && result == 409)
                        {
                            return Conflict(new
                            {
                                statusCode = HttpStatusCode.Conflict,
                                message = CoreCommonMessage.FirstAddFirstVoucherNo
                            });
                        }
                        else if (message == "success" && result > 0)
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = (account_Trans_Master.mod_Type == "B" ? (account_Trans_Master.account_Trans_Id > 0 ? CoreCommonMessage.AccountBankbookMasterUpdated : CoreCommonMessage.AccountBankbookMasterCreated) :
                                             account_Trans_Master.mod_Type == "C" ? (account_Trans_Master.account_Trans_Id > 0 ? CoreCommonMessage.AccountCashbookMasterUpdated : CoreCommonMessage.AccountCashbookMasterCreated) :
                                             account_Trans_Master.mod_Type == "JV" ? (account_Trans_Master.account_Trans_Id > 0 ? CoreCommonMessage.AccountJvMasterUpdated : CoreCommonMessage.AccountJvMasterCreated) :
                                             account_Trans_Master.mod_Type == "CO" ? (account_Trans_Master.account_Trans_Id > 0 ? CoreCommonMessage.AccountContraMasterUpdated : CoreCommonMessage.AccountContraMasterCreated) : "")
                            });
                        }
                    }

                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Account_Trans_Master", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }

        }

        [HttpDelete]
        [Route("delete_account_trans_master")]
        [Authorize]
        public async Task<IActionResult> Delete_Account_Trans_Master(int id,string trans_Type)
        {
            try
            {
                var result = await _account_Trans_Master_Service.Delete_Account_Trans_Master(id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = (trans_Type == "B" ? CoreCommonMessage.AccountBankbookMasterDeleted :
                                          trans_Type == "C" ? CoreCommonMessage.AccountCashbookMasterDeleted :
                                          trans_Type == "JV" ? CoreCommonMessage.AccountJvMasterDeleted :
                                          trans_Type == "CO" ? CoreCommonMessage.AccountContraMasterDeleted : 
                                          trans_Type == "P" ? CoreCommonMessage.AccountPurchaseMasterDeleted : "")

                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Account_Trans_Master", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        
        [HttpPost]
        [Route("create_update_account_trans_master_purchase")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Account_Trans_Master_Purchase(Account_Trans_Purchase_Master account_Trans_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);

                    if (user_Id > 0)
                    {
                        DateTime invoiceDate;
                        if (!DateTime.TryParseExact(account_Trans_Master.invoice_Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out invoiceDate))
                        {
                            invoiceDate = DateTime.Now.Date;
                        }

                        TimeSpan invoiceTime;
                        if (!TimeSpan.TryParseExact(account_Trans_Master.invoice_Time, "HH:mm", CultureInfo.InvariantCulture, out invoiceTime))
                        {
                            invoiceTime = DateTime.Now.TimeOfDay;
                        }

                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("Voucher_No", typeof(string));
                        dataTable.Columns.Add("By_Account", typeof(int));
                        dataTable.Columns.Add("By_Type", typeof(string));
                        dataTable.Columns.Add("To_Account", typeof(int));
                        dataTable.Columns.Add("To_Type", typeof(string));
                        dataTable.Columns.Add("Amount", typeof(decimal));
                        dataTable.Columns.Add("Narration", typeof(string));
                        dataTable.Columns.Add("Cat_Val_Id", typeof(int));
                        dataTable.Columns.Add("Parcel_Id", typeof(int));
                        dataTable.Columns.Add("Pcs", typeof(int));
                        dataTable.Columns.Add("Cts", typeof(decimal));
                        dataTable.Columns.Add("Remarks", typeof(string));
                        dataTable.Columns.Add("Rate", typeof(decimal));

                        if (account_Trans_Master.account_Trans_Detail != null && account_Trans_Master.account_Trans_Detail.Count > 0)
                        {
                            foreach (var item in account_Trans_Master.account_Trans_Detail)
                            {
                                DataRow row = dataTable.NewRow();

                                row["Voucher_No"] = !string.IsNullOrEmpty(item.voucherNo) ? item.voucherNo : (object)DBNull.Value;
                                row["By_Account"] = account_Trans_Master.account;
                                row["By_Type"] = account_Trans_Master.type;
                                row["To_Account"] = item.account;
                                row["To_Type"] = item.type;
                                row["Amount"] = item.amount;
                                row["Narration"] = !string.IsNullOrEmpty(item.narration) ? item.narration : (object)DBNull.Value;
                                row["Cat_Val_Id"] = item.Cat_Val_Id;
                                row["Parcel_Id"] = item.Parcel_Id > 0 ? (object)item.Parcel_Id : (object)DBNull.Value;
                                row["Pcs"] = item.Pcs > 0 ? (object)item.Pcs : (object)DBNull.Value;
                                row["Cts"] = item.Cts;
                                row["Remarks"] = item.Remarks;
                                row["Rate"] = item.Rate;

                                dataTable.Rows.Add(row);
                            }
                        }

                        DataTable dataTable_Terms = new DataTable();
                        dataTable_Terms.Columns.Add("Terms_Trans_Det_Id", typeof(int));
                        dataTable_Terms.Columns.Add("Terms_Id", typeof(int));
                        dataTable_Terms.Columns.Add("Amount", typeof(decimal));

                        if (account_Trans_Master.terms_Trans_Dets != null && account_Trans_Master.terms_Trans_Dets.Count > 0)
                        {
                            foreach (var item in account_Trans_Master.terms_Trans_Dets)
                            {
                                DataRow row = dataTable_Terms.NewRow();

                                row["Terms_Trans_Det_Id"] = item.Terms_Trans_Det_Id > 0 ? (object)item.Terms_Trans_Det_Id : (object)DBNull.Value;
                                row["Terms_Id"] = item.Terms_Id > 0 ? (object)item.Terms_Id : (object)DBNull.Value;
                                row["Amount"] = item.amount;

                                dataTable_Terms.Rows.Add(row);
                            }
                        }

                        DataTable dataTable_ExpenseTransDet = new DataTable();
                        dataTable_ExpenseTransDet.Columns.Add("Expense_Trans_Det_Id", typeof(int));
                        dataTable_ExpenseTransDet.Columns.Add("Account_Master_Id", typeof(int));
                        dataTable_ExpenseTransDet.Columns.Add("Sign", typeof(string));
                        dataTable_ExpenseTransDet.Columns.Add("Percentage", typeof(decimal));
                        dataTable_ExpenseTransDet.Columns.Add("Amount", typeof(decimal));
                        dataTable_ExpenseTransDet.Columns.Add("Amount_$", typeof(decimal));

                        if (account_Trans_Master.expense_Trans_Dets != null)
                        {
                            IList<Expense_Trans_Det> expense_Trans_Det = JsonConvert.DeserializeObject<IList<Expense_Trans_Det>>(account_Trans_Master.expense_Trans_Dets.ToString());

                            if (expense_Trans_Det != null && expense_Trans_Det.Count > 0)
                            {
                                foreach (var item in expense_Trans_Det)
                                {
                                    DataRow row = dataTable_ExpenseTransDet.NewRow();

                                    row["Expense_Trans_Det_Id"] = item.Expense_Trans_Det_Id > 0 ? (object)item.Expense_Trans_Det_Id : (object)DBNull.Value;
                                    row["Account_Master_Id"] = item.Account_Master_Id;
                                    row["Sign"] = item.Sign ?? (object)DBNull.Value;
                                    row["Percentage"] = item.Percentage > 0 ? (object)item.Percentage : (object)DBNull.Value;
                                    row["Amount"] = !string.IsNullOrEmpty(item.amount.ToString()) ? Convert.ToDecimal(item.amount.ToString()) : (object)DBNull.Value;
                                    if (item.amount_Dollar != null)
                                        row["Amount_$"] = Convert.ToDecimal(item.amount_Dollar);
                                    else
                                        row["Amount_$"] = 0;

                                    dataTable_ExpenseTransDet.Rows.Add(row);
                                }
                            }
                        }

                        DataTable dataTable_InwardDetail = new DataTable();
                        dataTable_InwardDetail.Columns.Add("Id", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Stock_Id", typeof(string));
                        dataTable_InwardDetail.Columns.Add("Cert_No", typeof(string));
                        dataTable_InwardDetail.Columns.Add("Shape", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Color", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Clarity", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Cts", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Rap_Price", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Rap_Amt", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Cost_Disc", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Cost_Amt", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Offer_Disc", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Offer_Amt", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Cut", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Polish", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Symm", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Flour_Intensity", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Length", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Width", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Depth", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Depth_Per", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Table_Per", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Crown_Angle", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Crown_Height", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Pavillion_Angle", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Pavillion_Height", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Lab", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Supplier_Ref_No", typeof(string));
                        dataTable_InwardDetail.Columns.Add("Girdle_Type", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Key_to_Symbol", typeof(string));
                        dataTable_InwardDetail.Columns.Add("Culet", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Lab_Comment", typeof(string));
                        dataTable_InwardDetail.Columns.Add("Str_Ln", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("LR_Half", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Girdle_Per", typeof(decimal));
                        dataTable_InwardDetail.Columns.Add("Girdle_Condition", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Table_White", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Crown_White", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Table_Black", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Crown_Black", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Shade", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Luster", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Pre_Sold", typeof(bool));
                        dataTable_InwardDetail.Columns.Add("Buyer", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Laser_Insc", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Cert_Date", typeof(DateTime));
                        dataTable_InwardDetail.Columns.Add("Cert_Type", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Company_Id", typeof(string));
                        //dataTable_InwardDetail.Columns.Add("Trans_Id", typeof(int));
                        //dataTable_InwardDetail.Columns.Add("Seq_No", typeof(int));
                        //dataTable_InwardDetail.Columns.Add("Year_Id", typeof(int));
                        //dataTable_InwardDetail.Columns.Add("Created_Date", typeof(DateTime));
                        //dataTable_InwardDetail.Columns.Add("Created_Time", typeof(TimeSpan));
                        //dataTable_InwardDetail.Columns.Add("Created_By", typeof(int));
                        //dataTable_InwardDetail.Columns.Add("Updated_Date", typeof(DateTime));
                        //dataTable_InwardDetail.Columns.Add("Updated_Time", typeof(TimeSpan));
                        //dataTable_InwardDetail.Columns.Add("Updated_By", typeof(int));
                        dataTable_InwardDetail.Columns.Add("RFID", typeof(string));
                        dataTable_InwardDetail.Columns.Add("Assign_Date", typeof(DateTime));
                        dataTable_InwardDetail.Columns.Add("Status", typeof(bool));
                        dataTable_InwardDetail.Columns.Add("Process", typeof(string));
                        dataTable_InwardDetail.Columns.Add("Close_Date", typeof(DateTime));

                        if (account_Trans_Master.inwardDetails != null && account_Trans_Master.inwardDetails.Count > 0)
                        {
                            //IList<InwardDetail> inwardDetails = JsonConvert.DeserializeObject<IList<InwardDetail>>(account_Trans_Master.inwardDetails.ToString());

                            foreach (var item in account_Trans_Master.inwardDetails)
                            {
                                DataRow row = dataTable_InwardDetail.NewRow();

                                row["Id"] = item.Id.HasValue ? (object)item.Id : DBNull.Value;
                                row["Stock_Id"] = !string.IsNullOrEmpty(item.Stock_Id) ? (object)item.Stock_Id : DBNull.Value;
                                row["Cert_No"] = !string.IsNullOrEmpty(item.Cert_No) ? (object)item.Cert_No : DBNull.Value;
                                row["Shape"] = item.Shape > 0 ? (object)item.Shape : DBNull.Value;
                                row["Color"] = item.Color > 0 ? (object)item.Color : DBNull.Value;
                                row["Clarity"] = item.Clarity > 0 ? (object)item.Clarity : DBNull.Value;
                                row["Cts"] = item.Cts.HasValue ? (object)item.Cts : DBNull.Value;
                                row["Rap_Price"] = item.Rap_Price.HasValue ? (object)item.Rap_Price : DBNull.Value;
                                row["Rap_Amt"] = item.Rap_Amt.HasValue ? (object)item.Rap_Amt : DBNull.Value;
                                row["Cost_Disc"] = item.Cost_Disc.HasValue ? (object)item.Cost_Disc : DBNull.Value;
                                row["Cost_Amt"] = item.Cost_Amt.HasValue ? (object)item.Cost_Amt : DBNull.Value;
                                row["Offer_Disc"] = item.Offer_Disc.HasValue ? (object)item.Offer_Disc : DBNull.Value;
                                row["Offer_Amt"] = item.Offer_Amt.HasValue ? (object)item.Offer_Amt : DBNull.Value;
                                row["Cut"] = item.Cut > 0 ? (object)item.Cut : DBNull.Value;
                                row["Polish"] = item.Polish > 0 ? (object)item.Polish : DBNull.Value;
                                row["Symm"] = item.Symm > 0 ? (object)item.Symm : DBNull.Value;
                                row["Flour_Intensity"] = item.Flour_Intensity.HasValue ? (object)item.Flour_Intensity : DBNull.Value;
                                row["Length"] = item.Length.HasValue ? (object)item.Length : DBNull.Value;
                                row["Width"] = item.Width.HasValue ? (object)item.Width : DBNull.Value;
                                row["Depth"] = item.Depth.HasValue ? (object)item.Depth : DBNull.Value;
                                row["Depth_Per"] = item.Depth_Per.HasValue ? (object)item.Depth_Per : DBNull.Value;
                                row["Table_Per"] = item.Table_Per.HasValue ? (object)item.Table_Per : DBNull.Value;
                                row["Crown_Angle"] = item.Crown_Angle.HasValue ? (object)item.Crown_Angle : DBNull.Value;
                                row["Crown_Height"] = item.Crown_Height.HasValue ? (object)item.Crown_Height : DBNull.Value;
                                row["Pavillion_Angle"] = item.Pavillion_Angle.HasValue ? (object)item.Pavillion_Angle : DBNull.Value;
                                row["Pavillion_Height"] = item.Pavillion_Height.HasValue ? (object)item.Pavillion_Height : DBNull.Value;
                                row["Lab"] = item.Lab > 0 ? (object)item.Lab : DBNull.Value;
                                row["Supplier_Ref_No"] = !string.IsNullOrEmpty(item.Supplier_Ref_No) ? item.Supplier_Ref_No : DBNull.Value;
                                row["Girdle_Type"] = item.Girdle_Type.HasValue ? (object)item.Girdle_Type : DBNull.Value;
                                row["Key_to_Symbol"] = !string.IsNullOrEmpty(item.Key_to_Symbol) ? (object)item.Rap_Price : DBNull.Value;
                                row["Culet"] = item.Culet.HasValue ? (object)item.Culet : DBNull.Value;
                                row["Lab_Comment"] = !string.IsNullOrEmpty(item.Lab_Comment) ? (object)item.Rap_Price : DBNull.Value;
                                row["Str_Ln"] = item.Str_Ln.HasValue ? (object)item.Str_Ln : DBNull.Value;
                                row["LR_Half"] = item.LR_Half.HasValue ? (object)item.LR_Half : DBNull.Value;
                                row["Girdle_Per"] = item.Girdle_Per.HasValue ? (object)item.Girdle_Per : DBNull.Value;
                                row["Girdle_Condition"] = item.Girdle_Condition.HasValue ? (object)item.Girdle_Condition : DBNull.Value;
                                row["Table_White"] = item.Table_White.HasValue ? (object)item.Table_White : DBNull.Value;
                                row["Crown_White"] = item.Crown_White.HasValue ? (object)item.Crown_White : DBNull.Value;
                                row["Table_Black"] = item.Table_Black.HasValue ? (object)item.Table_Black : DBNull.Value;
                                row["Crown_Black"] = item.Crown_Black.HasValue ? (object)item.Crown_Black : DBNull.Value;
                                row["Shade"] = item.Shade.HasValue ? (object)item.Shade : DBNull.Value;
                                row["Luster"] = item.Luster.HasValue ? (object)item.Luster : DBNull.Value;
                                row["Pre_Sold"] = item.Pre_Sold.HasValue ? (object)item.Pre_Sold : DBNull.Value;
                                row["Buyer"] = item.Buyer.HasValue ? (object)item.Buyer : DBNull.Value;
                                row["Laser_Insc"] = item.Laser_Insc.HasValue ? (object)item.Laser_Insc : DBNull.Value;
                                row["Cert_Date"] = item.Cert_Date.HasValue ? (object)item.Cert_Date : DBNull.Value;
                                row["Cert_Type"] = item.Cert_Type.HasValue ? (object)item.Cert_Type : DBNull.Value;
                                row["Company_Id"] = !string.IsNullOrEmpty(item.Company_Id) ? item.Company_Id : DBNull.Value;
                                //row["Trans_Id"] = item.Trans_Id.HasValue ? (object)item.Trans_Id : DBNull.Value;
                                //row["Seq_No"] = item.Seq_No.HasValue ? (object)item.Seq_No : DBNull.Value;
                                //row["Year_Id"] = item.Year_Id.HasValue ? (object)item.Year_Id : DBNull.Value;
                                //row["Created_Date"] = item.Created_Date.HasValue ? (object)item.Created_Date : DBNull.Value;
                                //row["Created_Time"] = item.Created_Time.HasValue ? (object)item.Created_Time : DBNull.Value;
                                //row["Created_By"] = item.Created_By.HasValue ? (object)item.Created_By : DBNull.Value;
                                //row["Updated_Date"] = item.Updated_Date.HasValue ? (object)item.Updated_Date : DBNull.Value;
                                //row["Updated_Time"] = item.Updated_Time.HasValue ? (object)item.Updated_Time : DBNull.Value;
                                //row["Updated_By"] = item.Updated_By.HasValue ? (object)item.Updated_By : DBNull.Value;
                                row["RFID"] = !string.IsNullOrEmpty(item.RFID) ? item.RFID : DBNull.Value;
                                row["Assign_Date"] = item.Assign_Date.HasValue ? (object)item.Assign_Date : DBNull.Value;
                                row["Status"] = item.Status.HasValue ? (object)item.Status : DBNull.Value;
                                row["Process"] = !string.IsNullOrEmpty(item.Process) ? item.Process : DBNull.Value;
                                row["Close_Date"] = item.Close_Date.HasValue ? (object)item.Close_Date : DBNull.Value;

                                dataTable_InwardDetail.Rows.Add(row);
                            }
                        }

                        var (message, result) = await _account_Trans_Master_Service.Create_Update_Account_Trans_Master_Purchase(
                            dataTable,
                            dataTable_Terms,
                            dataTable_ExpenseTransDet,
                            dataTable_InwardDetail,
                            account_Trans_Master.account_Trans_Id,
                            account_Trans_Master.mod_Type,
                            account_Trans_Master.invoice_No,
                            account_Trans_Master.currency,
                            account_Trans_Master.company,
                            account_Trans_Master.year,
                            account_Trans_Master.account,
                            account_Trans_Master.rate,
                            user_Id ?? 0,
                            account_Trans_Master.remarks,
                            invoiceDate,
                            invoiceTime,
                            account_Trans_Master.supplier_Id);

                        if (message == "not_exists" && result == 409)
                        {
                            return Conflict(new
                            {
                                statusCode = HttpStatusCode.Conflict,
                                message = CoreCommonMessage.FirstAddFirstVoucherNo
                            });
                        }
                        else if (message == "success" && result > 0)
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = (account_Trans_Master.mod_Type == "P" ? (account_Trans_Master.account_Trans_Id > 0 ? CoreCommonMessage.AccountPurchaseMasterUpdated : CoreCommonMessage.AccountPurchaseMasterCreated) : "")
                            });
                        }
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Account_Trans_Master_Purchase", ex.StackTrace);
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpGet]
        [Route("get_account_trans_master_purchase")]
        [Authorize]
        public async Task<IActionResult> Get_Account_Trans_Master_Purchase(int? account_Trans_Id, int? account_Trans_Detail_Id, string trans_Type, int? Year_Id)
        {
            try
            {
                var result = await _account_Trans_Master_Service.Get_Account_Trans_Master_Purchase(account_Trans_Id ?? 0, account_Trans_Detail_Id ?? 0, trans_Type, Year_Id ?? 0);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Account_Trans_Master_Purchase", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        
        [HttpGet]
        [Route("get_account_master_purchase")]
        [Authorize]
        public async Task<IActionResult> Get_Account_Master_Purchase(int account_Id)
        {
            try
            {
                var result = await _account_Master_Service.Get_Account_Master_Purchase(account_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Account_Master_Purchase", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_account_trans_master_purchase")]
        [Authorize]
        public async Task<IActionResult> Delete_Account_Trans_Master_Purchase(int id)
        {
            try
            {
                var result = await _account_Trans_Master_Service.Delete_Account_Trans_Master_Purchase(id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.AccountPurchaseMasterDeleted

                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Account_Trans_Master_Purchase", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_purchase_detail")]
        [Authorize]
        public async Task<IActionResult> Get_Purchase_Detail()
        {
            try
            {
                var result = await _account_Master_Service.Get_Purchase_Detail();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Purchase_Detail", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_account_trans_purchase")]
        [Authorize]
        public async Task<IActionResult> Get_Account_Trans_Purchase(int? account_Trans_Id, string trans_Type, int? Year_Id)
        {
            try
            {
                var result = await _account_Trans_Master_Service.Get_Account_Trans_Purchase(account_Trans_Id ?? 0, trans_Type, Year_Id ?? 0);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Account_Trans_Purchase", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("inward_details_read_excel")]
        [Authorize]
        public async Task<ActionResult> Inward_Details_Read_Excel([FromForm] IFormFile File_Location, int import_Id)
        {
            if (File_Location != null && File_Location.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/uploads");

                if (!(Directory.Exists(filePath)))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fileName = Path.GetFileNameWithoutExtension(File_Location.FileName);
                string fileExt = Path.GetExtension(File_Location.FileName);
                string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;

                using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                {
                    await File_Location.CopyToAsync(fileStream);
                }

                List<Dictionary<string, object>> result = await _categoryService.Get_Import_Master_Detail(import_Id);
                List<Dictionary<string, string>> UploadResults = await ProcessExcelFile(Path.Combine(filePath, strFile), result);

                if (UploadResults.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = UploadResults
                    });
                }
                else {
                    return NoContent();
                }
            }
            return Conflict(new
            {
                statusCode = HttpStatusCode.Conflict,
                message = CoreCommonMessage.FileNotFound
            });
        }

        public async Task<List<Dictionary<string, string>>> ProcessExcelFile(string filePath, List<Dictionary<string, object>> result)
        {
            List<Dictionary<string, string>> rowDataList = new List<Dictionary<string, string>>();

            try
            {
                using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                    int totalRows = worksheet.Dimension.End.Row;
                    var excelColumnHeaders = Enumerable.Range(1, worksheet.Dimension.End.Column)
                        .Select(col => worksheet.Cells[1, col].Value?.ToString()?.Trim())
                        .ToList();

                    var categoryIds = new Dictionary<string, int>
                    {
                        { "SHAPE", 13 },
                        { "COLOR", 14 },
                        { "CLARITY", 15 },
                        { "CUT", 16 },
                        { "POLISH", 17 },
                        { "SYMM", 19 },
                        { "FLS_INTENSITY", 21 },
                        { "TABLE_BLACK", 26 },
                        { "TABLE_WHITE", 27 },
                        { "SIDE_BLACK", 28 },
                        { "SIDE_WHITE", 29 },
                        { "LAB", 35 },
                        { "CERT_TYPE", 36 },
                        { "CULET", 37 },
                        { "GIRDLE_CONDITION", 49 },
                        { "LUSTER", 59 },
                        { "LASER_INSCRIPTION", 60 },
                        { "SHADE", 84 }
                    };

                    for (int rowIndex = 2; rowIndex <= totalRows; rowIndex++)
                    {
                        var rowData = new Dictionary<string, string>();

                        foreach (var mapping in result)
                        {
                            string displayColumnName = mapping["Column_Name"].ToString();

                            int columnIndex = excelColumnHeaders.FindIndex(header =>
                            {
                                string cleanedDisplayColumnName = displayColumnName.Replace("_", " ");
                                string cleanedDisplayColumnName1 = displayColumnName.Replace(" ", "_");

                                return string.Equals(header, displayColumnName, StringComparison.OrdinalIgnoreCase)
                                    || string.Equals(header, cleanedDisplayColumnName, StringComparison.OrdinalIgnoreCase)
                                    || string.Equals(header, cleanedDisplayColumnName1, StringComparison.OrdinalIgnoreCase);
                            });

                            if (columnIndex != -1)
                            {
                                string columnKey = displayColumnName.Replace(" ", "_").ToUpper();

                                if (categoryIds.TryGetValue(columnKey, out int categoryId))
                                {
                                    Initialize_ColumnData(rowData, columnKey);

                                    var result_category = await _categoryService.Get_Active_Category_Values(categoryId);

                                    var cellValue = worksheet.Cells[rowIndex, columnIndex + 1].Value?.ToString();
                                    var catValId = Find_Cat_Val_Id(result_category, cellValue);
                                    rowData[columnKey] = catValId.Item1.ToString();
                                    rowData[$"{columnKey}_NAME"] = catValId.Item2;
                                }
                                else if (columnKey == "CERTIFICATE_DATE")
                                {
                                    Initialize_ColumnData(rowData, columnKey);

                                    var dateCellValue = worksheet.Cells[rowIndex, columnIndex + 1].Value?.ToString();
                                    if (!string.IsNullOrEmpty(dateCellValue) && DateTime.TryParse(dateCellValue, out DateTime date))
                                    {
                                        rowData[columnKey] = date.ToString("dd-MM-yyyy");
                                    }
                                    else
                                    {
                                        rowData[columnKey] = null;
                                    }
                                }
                                else
                                {
                                    var defaultValue = worksheet.Cells[rowIndex, columnIndex + 1].Value?.ToString();
                                    rowData[columnKey] = defaultValue;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Column '{displayColumnName}' not found in Excel file.");
                            }
                        }

                        rowDataList.Add(rowData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return rowDataList;
        }

        private void Initialize_ColumnData(Dictionary<string, string> rowData, string columnKey)
        {
            string columnKey_Name = $"{columnKey}_NAME";

            if (!rowData.ContainsKey(columnKey))
            {
                rowData[columnKey] = null;
            }
            if (!rowData.ContainsKey(columnKey_Name))
            {
                rowData[columnKey_Name] = null;
            }
        }

        public (int, string) Find_Cat_Val_Id(IList<CategoryValueModel> data, string Cat_Name)
        {
            if (string.IsNullOrWhiteSpace(Cat_Name))
                return (0, null);

            var Info = data.FirstOrDefault(d => d.Cat_Name.Equals(Cat_Name, StringComparison.OrdinalIgnoreCase)
                                                  || (d.Synonyms != null && d.Synonyms.Split(',').Any(s => s.Equals(Cat_Name, StringComparison.OrdinalIgnoreCase))));

            if (Info != null)
            {
                return (Info.Cat_val_Id, Info.Cat_Name);
            }
            else
            {
                return (0, null);
            }
        }

        [HttpGet]
        [Route("get_import_excel_purchase")]
        [Authorize]
        public async Task<IActionResult> Get_Import_Excel_Purchase(int Type_Id)
        {
            try
            {
                var result = await _account_Master_Service.Get_Import_Excel_Purchase(Type_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Import_Excel_Purchase", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("get_account_trans_master_remarks")]
        [Authorize]
        public async Task<IActionResult> Get_Account_Trans_Master_Remarks(string Trans_Type)
        {
            try
            {
                var result = await _account_Master_Service.Get_Account_Trans_Master_Remarks(Trans_Type);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Account_Trans_Master_Remarks", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion
    }
}
