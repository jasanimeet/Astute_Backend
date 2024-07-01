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
        private readonly IEmailSender _emailSender;
        private readonly IJWTAuthentication _jWTAuthentication;
        private readonly IEmployeeService _employeeService;
        private readonly IAccount_Group_Service _account_Group_Service;
        private readonly IAccount_Master_Service _account_Master_Service;
        private readonly IFirst_Voucher_No _trans_Service;
        private readonly IAccount_Trans_Master_Service _account_Trans_Master_Service;

        #endregion

        #region Ctor
        public AccountController(
            IConfiguration configuration,
            ICommonService commonService,
            IHttpContextAccessor httpContextAccessor,
            IEmailSender emailSender,
            IJWTAuthentication jWTAuthentication,
            IEmployeeService employeeService,
            IAccount_Group_Service account_Group_Service,
            IAccount_Master_Service account_Master_Service,
            IFirst_Voucher_No trans_Service,
            IAccount_Trans_Master_Service account_Trans_Master_Service)
        {
            _configuration = configuration;
            _commonService = commonService;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
            _jWTAuthentication = jWTAuthentication;
            _employeeService = employeeService;
            _account_Group_Service = account_Group_Service;
            _account_Master_Service = account_Master_Service;
            _trans_Service = trans_Service;
            _account_Trans_Master_Service = account_Trans_Master_Service;
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
                        dataTable.Columns.Add("Cat_Val_Id", typeof(int));
                        dataTable.Columns.Add("Parcel_Id", typeof(int));
                        dataTable.Columns.Add("Pcs", typeof(int));
                        dataTable.Columns.Add("Cts", typeof(decimal));
                        dataTable.Columns.Add("Remarks", typeof(string));
                        dataTable.Columns.Add("Rate", typeof(decimal));

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

                        DataTable dataTable_Terms = new DataTable();
                        dataTable_Terms.Columns.Add("Terms_Id", typeof(int));
                        dataTable_Terms.Columns.Add("Amount", typeof(decimal));
                        dataTable_Terms.Columns.Add("Trans_Id", typeof(int));
                        dataTable_Terms.Columns.Add("Trans_Type", typeof(string));

                        foreach (var item in account_Trans_Master.terms_Trans_Dets)
                        {
                            DataRow row = dataTable_Terms.NewRow();

                            row["Terms_Id"] = item.Terms_Id > 0 ? (object)item.Terms_Id : (object)DBNull.Value;
                            row["Amount"] = item.Amount;
                            row["Trans_Id"] = item.Trans_Id > 0 ? (object)item.Trans_Id : (object)DBNull.Value;
                            row["Trans_Type"] = account_Trans_Master.mod_Type;

                            dataTable_Terms.Rows.Add(row);
                        }

                        var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                        int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);

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

                        var (message, result) = await _account_Trans_Master_Service.Create_Update_Account_Trans_Master_Purchase(
                            dataTable,
                            dataTable_Terms,
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
                            invoiceTime);

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


        #endregion
    }
}
