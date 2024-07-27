using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using astute.Repository;
using ClosedXML.Excel;
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
        public async Task<IActionResult> Get_Account_Trans_Master(int? account_Trans_Id, int? account_Trans_Detail_Id, string trans_Type)
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
        public async Task<IActionResult> Delete_Account_Trans_Master(int id, string trans_Type)
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
                                row["Amount"] = item.amount > 0 ? (object)item.amount : (object)DBNull.Value;
                                row["Narration"] = !string.IsNullOrEmpty(item.narration) ? item.narration : (object)DBNull.Value;
                                row["Cat_Val_Id"] = item.Cat_Val_Id > 0 ? (object)item.Cat_Val_Id : (object)DBNull.Value;
                                row["Parcel_Id"] = item.Parcel_Id > 0 ? (object)item.Parcel_Id : (object)DBNull.Value;
                                row["Pcs"] = item.Pcs > 0 ? (object)item.Pcs : (object)DBNull.Value;
                                row["Cts"] = item.Cts > 0 ? (object)item.Cts : (object)DBNull.Value;
                                row["Remarks"] = !string.IsNullOrEmpty(item.Remarks) ? item.Remarks : (object)DBNull.Value;
                                row["Rate"] = item.Rate > 0 ? (object)item.Rate : (object)DBNull.Value;

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
                                row["Amount"] = item.amount > 0 ? (object)item.amount : (object)DBNull.Value;

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
                        dataTable_InwardDetail.Columns.Add("Laser_Insc", typeof(string));
                        dataTable_InwardDetail.Columns.Add("Cert_Date", typeof(DateTime));
                        dataTable_InwardDetail.Columns.Add("Cert_Type", typeof(int));
                        dataTable_InwardDetail.Columns.Add("Company_Id", typeof(string));
                        dataTable_InwardDetail.Columns.Add("RFID", typeof(string));
                        dataTable_InwardDetail.Columns.Add("Assign_Date", typeof(DateTime));
                        dataTable_InwardDetail.Columns.Add("Status", typeof(bool));
                        dataTable_InwardDetail.Columns.Add("Process", typeof(string));
                        dataTable_InwardDetail.Columns.Add("Close_Date", typeof(DateTime));

                        if (account_Trans_Master.inwardDetails != null && account_Trans_Master.inwardDetails.Count > 0)
                        {
                            foreach (var item in account_Trans_Master.inwardDetails)
                            {
                                DataRow row = dataTable_InwardDetail.NewRow();

                                row["Id"] = item.Id.HasValue ? (object)item.Id.Value : DBNull.Value;
                                row["Stock_Id"] = !string.IsNullOrEmpty(item.Reference_No) ? (object)item.Reference_No : DBNull.Value;
                                row["Cert_No"] = !string.IsNullOrEmpty(item.Certificate_No) ? (object)item.Certificate_No : DBNull.Value;
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
                                row["Supplier_Ref_No"] = !string.IsNullOrEmpty(item.Supplier_Ref_No) ? (object)item.Supplier_Ref_No : DBNull.Value;
                                row["Key_to_Symbol"] = !string.IsNullOrEmpty(item.Key_to_Symbol) ? (object)item.Key_to_Symbol : DBNull.Value;
                                row["Culet"] = item.Culet.HasValue ? (object)item.Culet : DBNull.Value;
                                row["Lab_Comment"] = !string.IsNullOrEmpty(item.Lab_Comment) ? (object)item.Lab_Comment : DBNull.Value;
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
                                bool? preSoldValue = null;

                                if (!string.IsNullOrEmpty(item.Pre_Sold))
                                {
                                    if (item.Pre_Sold == "1")
                                    {
                                        preSoldValue = true;
                                    }
                                    else if (item.Pre_Sold == "0")
                                    {
                                        preSoldValue = false;
                                    }
                                }
                                row["Pre_Sold"] = preSoldValue.HasValue ? (object)preSoldValue.Value : DBNull.Value;
                                row["Buyer"] = item.Buyer.HasValue ? (object)item.Buyer : DBNull.Value;
                                var laserInscValue = item.Laser_Insc;
                                if (laserInscValue == "1")
                                {
                                    laserInscValue = "Y";
                                }
                                else if (laserInscValue == "0")
                                {
                                    laserInscValue = "N";
                                }
                                row["Laser_Insc"] = !string.IsNullOrEmpty(laserInscValue) ? (object)laserInscValue : DBNull.Value;
                                row["Cert_Date"] = item.Cert_Date.HasValue ? (object)item.Cert_Date : DBNull.Value;
                                row["Cert_Type"] = item.Cert_Type.HasValue ? (object)item.Cert_Type : DBNull.Value;
                                row["Company_Id"] = !string.IsNullOrEmpty(item.Company_Id) ? (object)item.Company_Id : DBNull.Value;
                                row["RFID"] = !string.IsNullOrEmpty(item.RFID) ? (object)item.RFID : DBNull.Value;
                                row["Assign_Date"] = item.Assign_Date.HasValue ? (object)item.Assign_Date : DBNull.Value;
                                row["Status"] = item.Status.HasValue ? (object)item.Status : DBNull.Value;
                                row["Process"] = !string.IsNullOrEmpty(item.Process) ? (object)item.Process : DBNull.Value;
                                row["Close_Date"] = item.Close_Date.HasValue ? (object)item.Close_Date : DBNull.Value;


                                if (!string.IsNullOrEmpty(item.Girdle_Type.ToString()))
                                {
                                    if (int.TryParse(item.Girdle_Type.ToString(), out int girdleType))
                                    {
                                        row["Girdle_Type"] = girdleType;
                                    }
                                    else
                                    {
                                        row["Girdle_Type"] = DBNull.Value;
                                    }
                                }
                                else
                                {
                                    row["Girdle_Type"] = DBNull.Value;
                                }

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

        [HttpGet]
        [Route("get_excel_sheet_name")]
        [Authorize]
        public async Task<ActionResult> Get_Excel_Sheet_Name([FromForm] IFormFile File_Location)
        {

            if (File_Location == null)
            {
                return Conflict(new
                {
                    statusCode = HttpStatusCode.Conflict,
                    message = CoreCommonMessage.FileNotFound
                });
            }

            string extension = Path.GetExtension(File_Location.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls")
            {
                return Conflict(new
                {
                    statusCode = HttpStatusCode.Conflict,
                    message = CoreCommonMessage.InvalidFileFormat
                });
            }

            try
            {
                using (var stream = File_Location.OpenReadStream())
                {
                    using (var workbook = new XLWorkbook(stream))
                    {
                        List<string> sheetNames = new List<string>();
                        foreach (IXLWorksheet worksheet in workbook.Worksheets)
                        {
                            sheetNames.Add(worksheet.Name);
                        }
                        return Ok(sheetNames);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("inward_details_read_excel")]
        [Authorize]
        public async Task<ActionResult> Inward_Details_Read_Excel([FromForm] IFormFile File_Location, int import_Id, string Sheet_Name)
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

                List<Dictionary<string, object>> result = await _categoryService.Get_Import_Master_Detail_Purchase(import_Id);
                var UploadResults = await ProcessExcelFile(Path.Combine(filePath, strFile), result, Sheet_Name);

                if (UploadResults.SuccessFields.Count > 0 || UploadResults.ErrorFields.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = new
                        {
                            SuccessFields = UploadResults.SuccessFields,
                            ErrorFields = UploadResults.ErrorFields
                        }
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
        public async Task<(List<Dictionary<string, string>> SuccessFields, List<Dictionary<string, string>> ErrorFields)> ProcessExcelFile(string filePath, List<Dictionary<string, object>> result, string sheetName)
        {
            var successFields = new List<Dictionary<string, string>>();
            var errorFields = new List<Dictionary<string, string>>();

            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[sheetName];
                    int totalRows = worksheet.Dimension.End.Row;
                    int totalColumns = worksheet.Dimension.End.Column;

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
                        { "GIRDLE_TYPE", 50 },
                        { "LUSTER", 59 },
                        //{ "LASER_INSCRIPTION", 60 },
                        { "SHADE", 84 }
                    };

                    for (int rowIndex = 2; rowIndex <= totalRows; rowIndex++)
                    {
                        bool isRowEmpty = true;
                        var rowData = new Dictionary<string, string>();
                        bool hasError = false;
                        var errorMessages = new List<string>();

                        for (int colIndex = 1; colIndex <= totalColumns; colIndex++)
                        {
                            if (!string.IsNullOrWhiteSpace(worksheet.Cells[rowIndex, colIndex].Value?.ToString()))
                            {
                                isRowEmpty = false;
                                break;
                            }
                        }

                        if (isRowEmpty) continue;

                        foreach (var mapping in result)
                        {
                            string excelColumnNo = mapping["Excel_Column_No"].ToString();
                            string displayColumnName = mapping["Column_Name"].ToString();
                            bool required = (bool)mapping["Required"];

                            if (int.TryParse(excelColumnNo, out int excelColumnNoInt))
                            {
                                int columnIndex = excelColumnNoInt;

                                if (columnIndex > 0 && columnIndex <= totalColumns)
                                {
                                    string columnKey = displayColumnName.Replace(" ", "_").ToUpper();

                                    if (categoryIds.TryGetValue(columnKey, out int categoryId))
                                    {
                                        Initialize_ColumnData(rowData, columnKey);

                                        var resultCategory = await _categoryService.Get_Active_Category_Values(categoryId);

                                        var cellValue = worksheet.Cells[rowIndex, columnIndex].Value?.ToString();
                                        var catValId = Find_Cat_Val_Id(resultCategory, cellValue, required);
                                        rowData[columnKey] = catValId.Item1;
                                        rowData[$"{columnKey}_NAME"] = catValId.Item2;
                                        
                                        if (catValId.Item2 != null && catValId.Item2.StartsWith("Invalid"))
                                        {
                                            hasError = true;
                                            errorMessages.Add($"Invalid value in column no: {columnIndex} and column header: {columnKey}");
                                        }
                                        else if (catValId.Item2 == null && required)
                                        {
                                            hasError = true;
                                            errorMessages.Add($"Value in column no: {columnIndex} and column header: {columnKey} is null.");
                                        }
                                    }
                                    else if (columnKey == "CERTIFICATE_DATE")
                                    {
                                        Initialize_ColumnData(rowData, columnKey);

                                        var dateCellValue = worksheet.Cells[rowIndex, columnIndex].Value?.ToString();
                                        if (!string.IsNullOrEmpty(dateCellValue) && DateTime.TryParse(dateCellValue, out DateTime date))
                                        {
                                            rowData[columnKey] = date.ToString("dd-MM-yyyy");
                                        }
                                        else
                                        {
                                            if (required)
                                            {
                                                rowData[columnKey] = null;
                                                hasError = true;
                                                errorMessages.Add($"Invalid date format in column no : {columnIndex} and column header : {columnKey}");
                                            }
                                            else
                                            {
                                                rowData[columnKey] = null;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var defaultValue = worksheet.Cells[rowIndex, columnIndex].Value?.ToString();                                        
                                        if (string.IsNullOrWhiteSpace(defaultValue))
                                        {
                                            if (required)
                                            {
                                                rowData[columnKey] = "Field is required";
                                                hasError = true;
                                                errorMessages.Add($"Field is required in column no : {columnIndex} and column header : {columnKey}");
                                            }
                                            else
                                            {
                                                rowData[columnKey] = null;
                                            }
                                        }
                                        else
                                        {
                                            if (defaultValue?.ToUpper() == "Y")
                                            {
                                                defaultValue = "1";
                                            }
                                            else if (defaultValue?.ToUpper() == "N")
                                            {
                                                defaultValue = "0";
                                            }
                                            rowData[columnKey] = defaultValue;
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Column index '{columnIndex}' is out of range.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Invalid column number '{excelColumnNo}' for '{displayColumnName}'.");
                            }
                        }

                        if (hasError)
                        {
                            rowData["ErrorMessage"] = $"Errors in row number {rowIndex}";
                            if (errorMessages.Count == 1)
                            {
                                rowData["ErrorColumn"] = errorMessages[0];
                            }
                            else if (errorMessages.Count > 1)
                            {
                                rowData["ErrorColumn"] = string.Join(", ", errorMessages);
                            }
                            else
                            {
                                rowData["ErrorColumn"] = string.Empty;
                            }
                            errorFields.Add(rowData);
                        }
                        else
                        {
                            successFields.Add(rowData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return (successFields, errorFields);
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

        public (string, string) Find_Cat_Val_Id(IList<CategoryValueModel> data, string catName, bool required)
        {
            if (required && string.IsNullOrWhiteSpace(catName))
            {
                return (null, $"Invalid {catName.ToLower()}");
            }

            if (!required && string.IsNullOrWhiteSpace(catName))
            {
                return (null, null);
            }

            var info = data.FirstOrDefault(d =>
                d.Cat_Name.Equals(catName, StringComparison.OrdinalIgnoreCase) ||
                (d.Synonyms != null && d.Synonyms.Split(',').Any(s => s.Equals(catName, StringComparison.OrdinalIgnoreCase)))
            );

            if (info != null)
            {
                return (info.Cat_val_Id.ToString(), info.Cat_Name);
            }

            return (null, required ? $"Invalid {catName.ToLower()}" : null);
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
