using astute.Repository;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Net;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.SS.Formula.Functions;

namespace astute.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {

        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthentication _jWTAuthentication;
        private readonly IConfiguration _configuration;
        private readonly ICommonService _commonService;
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;

        #endregion

        #region Ctor
        public TransactionController(
            IConfiguration configuration,
            ICommonService commonService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthentication jWTAuthentication,
            ITransactionService transactionService,
            ICategoryService categoryService)
        {
            _configuration = configuration;
            _commonService = commonService;
            _httpContextAccessor = httpContextAccessor;
            _jWTAuthentication = jWTAuthentication;
            _transactionService = transactionService;
            _categoryService = categoryService;
        }
        #endregion

        #region Methods

        [HttpPost]
        [Route("create_update_transaction_details")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Transaction_Details(Transaction_Master_Model transaction_Master_Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);

                    if (user_Id > 0)
                    {

                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("Stock_Id", typeof(string));
                        dataTable.Columns.Add("Disc", typeof(double));
                        dataTable.Columns.Add("Amount", typeof(double));

                        var errorMessages = string.Empty;
                        bool hasError = false;

                        if (transaction_Master_Model.transaction_Detail_Model != null && transaction_Master_Model.transaction_Detail_Model.Count > 0)
                        {
                            foreach (var item in transaction_Master_Model.transaction_Detail_Model)
                            {
                                var exists = await _transactionService.Check_Transaction_Detail_Stock_Id(item.Stock_Id);

                                if (exists.Count == 0 || transaction_Master_Model.Trans_Id >0)
                                {
                                    DataRow row = dataTable.NewRow();

                                    row["Stock_Id"] = !string.IsNullOrEmpty(item.Stock_Id) ? item.Stock_Id : (object)DBNull.Value;
                                    row["Disc"] = item.Disc;
                                    row["Amount"] = item.Amt;
                                    dataTable.Rows.Add(row);
                                }
                                else
                                {
                                    hasError = true;
                                    if (string.IsNullOrEmpty(errorMessages))
                                    {
                                        errorMessages = item.Stock_Id;
                                    }
                                    else
                                    {
                                        errorMessages += " , " + item.Stock_Id;
                                    }
                                }
                            }

                        }


                        var result = await _transactionService.Create_Update_Transaction_Details(dataTable, transaction_Master_Model.Trans_Id, transaction_Master_Model.Due_Days, transaction_Master_Model.Party_Code, transaction_Master_Model.Process, transaction_Master_Model.Remarks);

                        string message = string.Empty;
                        if (result > 0)
                        {
                            string baseMessage = transaction_Master_Model.Trans_Id == 0
                                    ? CoreCommonMessage.TransactionCreated
                                    : CoreCommonMessage.TransactionUpdated;

                            message = hasError
                                ? $"{baseMessage} Stock Ids : {errorMessages} already hold"
                                : baseMessage;

                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = message
                            });
                        }
                        else {

                            message = $"Stock Ids : {errorMessages} already hold";

                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = message
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
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Transaction_Details", ex.StackTrace);
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPost]
        [Route("get_transaction_details")]
        [Authorize]
        public async Task<IActionResult> Get_Transaction_Details([FromForm] Get_Transaction_Model get_Transaction, IFormFile? File_Location)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Stock_Id", typeof(string));
                    dataTable.Columns.Add("Disc", typeof(double));
                    dataTable.Columns.Add("Amount", typeof(double));

                    if (File_Location != null)
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

                        List<Dictionary<string, object>> resultObj = await _categoryService.Get_Import_Master_Detail_Purchase(get_Transaction.Import_Id ?? 0);
                        var UploadResults = await ProcessExcelFile(Path.Combine(filePath, strFile), resultObj, get_Transaction.Sheet_Name);
                        dataTable = UploadResults.dataTable;
                    }
                    var result = await _transactionService.Get_Transaction_Details(dataTable, get_Transaction.Stock_Id, get_Transaction.Id, get_Transaction.Sign, get_Transaction.Value);
                    if (result != null && result.Count > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.DataSuccessfullyFound,
                            data = result
                        });
                    }

                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Transaction_Details", ex.StackTrace);
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }

        public async Task<(List<Dictionary<string, string>> SuccessFields, List<Dictionary<string, string>> ErrorFields, DataTable dataTable)> ProcessExcelFile(string filePath, List<Dictionary<string, object>> result, string sheetName)
        {
            var successFields = new List<Dictionary<string, string>>();
            var errorFields = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Stock_Id", typeof(string));
            dataTable.Columns.Add("Disc", typeof(double));
            dataTable.Columns.Add("Amount", typeof(double));

            try
            {

                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[sheetName];
                    int totalRows = worksheet.Dimension.End.Row;
                    int totalColumns = worksheet.Dimension.End.Column;

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

                            if (int.TryParse(excelColumnNo, out int columnIndex))
                            {
                                if (columnIndex > 0 && columnIndex <= totalColumns)
                                {
                                    string columnKey = displayColumnName.Replace(" ", "_").ToUpper();

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
                                        rowData[columnKey] = defaultValue;
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
                            //rowData["ErrorMessage"] = $"Errors in row number {rowIndex}";
                            if (errorMessages.Count == 1)
                            {
                                rowData["ErrorMessage"] = errorMessages[0];
                            }
                            else if (errorMessages.Count > 1)
                            {
                                rowData["ErrorMessage"] = string.Join(", ", errorMessages);
                            }
                            else
                            {
                                rowData["ErrorMessage"] = string.Empty;
                            }
                            errorFields.Add(rowData);
                        }
                        else
                        {
                            successFields.Add(rowData);

                            HashSet<string> uniqueValues = new HashSet<string>();


                            string ref_no = rowData["REFERENCE_NO"] != null ? rowData["REFERENCE_NO"].ToString() : (rowData["CERTIFICATE_NO"] != null ? rowData["CERTIFICATE_NO"] : null);

                            string offer_amt = rowData["OFFER_AMOUNT"];
                            string offer_Disc = rowData["OFFER_DISC"];
                            if (!uniqueValues.Contains(ref_no))
                            {
                                uniqueValues.Add(ref_no);
                                dataTable.Rows.Add(ref_no, offer_Disc, !string.IsNullOrEmpty(offer_amt) ? offer_amt.Replace(",", "") : DBNull.Value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }


            return (successFields, errorFields, dataTable);
        }


        #endregion
    }
}
