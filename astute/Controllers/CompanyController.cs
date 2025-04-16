using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using astute.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace astute.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class CompanyController : ControllerBase
    {
        #region Fields
        private readonly ICompanyService _companyService;
        private readonly ICommonService _commonService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public CompanyController(ICompanyService companyService,
            ICommonService commonService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _companyService = companyService;
            _commonService = commonService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion

        #region Methods
        [HttpGet]
        [Route("getcompanies")]
        [Authorize]
        public async Task<IActionResult> GetCompanies(int companyId)
        {
            try
            {
                var result = await _companyService.GetCompany(companyId);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        data = result
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = "Records not found."
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetCompanies", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_company_details")]
        [Authorize]
        public async Task<IActionResult> Get_Company_Details(int company_Id)
        {
            try
            {
                var result = await _companyService.Get_Company_Details_By_Id(company_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Company_Details", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_active_company")]
        public async Task<IActionResult> Get_Active_Company()
        {
            try
            {
                var result = await _companyService.Get_Active_Company();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Active_Company", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_company_details")]
        [Authorize]
        public async Task<IActionResult> Create_Company_Details([FromForm] Company_Master company_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
                    var (message, company_Id) = await _companyService.AddUpdateCompany(company_Master);
                    if (message == "success" && company_Id > 0)
                    {
                        //Company Documents
                        if (company_Master.Company_Document_List != null && company_Master.Company_Document_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Company_Document_Id", typeof(int));
                            dataTable.Columns.Add("Company_Id", typeof(int));
                            dataTable.Columns.Add("Cat_Val_Id", typeof(int));
                            dataTable.Columns.Add("Document_No", typeof(string));
                            dataTable.Columns.Add("Start_Date", typeof(string));
                            dataTable.Columns.Add("Expiry_Date", typeof(string));
                            dataTable.Columns.Add("Upload_Path", typeof(string));
                            dataTable.Columns.Add("Upload_Path_1", typeof(string));
                            dataTable.Columns.Add("Upload_Path_2", typeof(string));
                            dataTable.Columns.Add("Upload_Path_3", typeof(string));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            //#region Company Document Log
                            //DataTable dataTable2 = new DataTable();
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    dataTable2.Columns.Add("Employee_Id", typeof(int));
                            //    dataTable2.Columns.Add("IP_Address", typeof(string));
                            //    dataTable2.Columns.Add("Trace_Date", typeof(DateTime));
                            //    dataTable2.Columns.Add("Trace_Time", typeof(TimeSpan));
                            //    dataTable2.Columns.Add("Record_Type", typeof(string));
                            //    dataTable2.Columns.Add("Cat_Val_Id", typeof(int));
                            //    dataTable2.Columns.Add("Start_Date", typeof(DateTime));
                            //    dataTable2.Columns.Add("Expiry_Date", typeof(DateTime));
                            //    dataTable2.Columns.Add("Upload_Path", typeof(string));
                            //}
                            //#endregion

                            foreach (var item in company_Master.Company_Document_List)
                            {
                                if (item.Upload_Path_Name != null && item.Upload_Path_Name.Length > 0)
                                {
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/CompanyDocuments");
                                    if (!(Directory.Exists(filePath)))
                                    {   
                                        Directory.CreateDirectory(filePath);
                                    }
                                    string fileName = Path.GetFileNameWithoutExtension(item.Upload_Path_Name.FileName);
                                    string fileExt = Path.GetExtension(item.Upload_Path_Name.FileName);

                                    string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                                    using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                                    {
                                        await item.Upload_Path_Name.CopyToAsync(fileStream);
                                    }
                                    item.Upload_Path = strFile;
                                }
                                if (item.Upload_Path_Name_1 != null && item.Upload_Path_Name_1.Length > 0)
                                {
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/CompanyDocuments");
                                    if (!(Directory.Exists(filePath)))
                                    {
                                        Directory.CreateDirectory(filePath);
                                    }
                                    string fileName = Path.GetFileNameWithoutExtension(item.Upload_Path_Name_1.FileName);
                                    string fileExt = Path.GetExtension(item.Upload_Path_Name_1.FileName);

                                    string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                                    using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                                    {
                                        await item.Upload_Path_Name_1.CopyToAsync(fileStream);
                                    }
                                    item.Upload_Path_1 = strFile;
                                }
                                if (item.Upload_Path_Name_2 != null && item.Upload_Path_Name_2.Length > 0)
                                {
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/CompanyDocuments");
                                    if (!(Directory.Exists(filePath)))
                                    {
                                        Directory.CreateDirectory(filePath);
                                    }
                                    string fileName = Path.GetFileNameWithoutExtension(item.Upload_Path_Name_2.FileName);
                                    string fileExt = Path.GetExtension(item.Upload_Path_Name_2.FileName);

                                    string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                                    using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                                    {
                                        await item.Upload_Path_Name_2.CopyToAsync(fileStream);
                                    }
                                    item.Upload_Path_2 = strFile;
                                }
                                if (item.Upload_Path_Name_3 != null && item.Upload_Path_Name_3.Length > 0)
                                {
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/CompanyDocuments");
                                    if (!(Directory.Exists(filePath)))
                                    {
                                        Directory.CreateDirectory(filePath);
                                    }
                                    string fileName = Path.GetFileNameWithoutExtension(item.Upload_Path_Name_3.FileName);
                                    string fileExt = Path.GetExtension(item.Upload_Path_Name_3.FileName);

                                    string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                                    using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                                    {
                                        await item.Upload_Path_Name_3.CopyToAsync(fileStream);
                                    }
                                    item.Upload_Path_3 = strFile;
                                }
                                string start_Date = !string.IsNullOrEmpty(item.Start_Date) ? item.Start_Date : null;
                                string expire_Date = !string.IsNullOrEmpty(item.Expiry_Date) ? item.Expiry_Date : null;
                                dataTable.Rows.Add(item.Company_Document_Id, company_Id, item.Cat_Val_Id, item.Document_No, start_Date, expire_Date, item.Upload_Path, item.Upload_Path_1, item.Upload_Path_2, item.Upload_Path_3, item.QueryFlag);
                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable2.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, item.Cat_Val_Id, item.Start_Date, item.Expiry_Date, item.Upload_Path);
                                //}
                            }
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    //Insert Company Document Log
                            //    await _companyService.Insert_Company_Document_Trace(dataTable2);
                            //}

                            //Insert Company Document
                            await _companyService.InsertCompanyDocument(dataTable);
                        }
                        //Company Media
                        if (company_Master.Company_Media_List != null && company_Master.Company_Media_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Company_Media_Id", typeof(int));
                            dataTable.Columns.Add("Company_Id", typeof(int));
                            dataTable.Columns.Add("Cat_Val_Id", typeof(int));
                            dataTable.Columns.Add("Media_Detail", typeof(string));
                            dataTable.Columns.Add("QueryFlag", typeof(string));
                                                        
                            DataTable dataTable2 = new DataTable();

                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    dataTable2.Columns.Add("Employee_Id", typeof(int));
                            //    dataTable2.Columns.Add("IP_Address", typeof(string));
                            //    dataTable2.Columns.Add("Trace_Date", typeof(DateTime));
                            //    dataTable2.Columns.Add("Trace_Time", typeof(TimeSpan));
                            //    dataTable2.Columns.Add("Record_Type", typeof(string));
                            //    dataTable2.Columns.Add("Company_Id", typeof(int));
                            //    dataTable2.Columns.Add("Cat_Val_Id", typeof(int));
                            //    dataTable2.Columns.Add("Media_Detail", typeof(string));
                            //}

                            foreach (var item in company_Master.Company_Media_List)
                            {
                                dataTable.Rows.Add(item.Company_Media_Id, company_Id, item.Cat_Val_Id, item.Media_Detail, item.QueryFlag);
                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable2.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, company_Id, item.Cat_Val_Id, item.Media_Detail);
                                //}
                            }
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    await _companyService.Insert_Company_Media_Trace(dataTable2);
                            //}
                            await _companyService.InsertCompanyMedia(dataTable);
                        }
                        //Company Bank
                        if (company_Master.Company_Bank_List != null && company_Master.Company_Bank_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Company_Bank_Id", typeof(int));
                            dataTable.Columns.Add("Company_Id", typeof(int));
                            dataTable.Columns.Add("Bank_Id", typeof(int));
                            dataTable.Columns.Add("Currency", typeof(string));
                            dataTable.Columns.Add("Account_Type", typeof(int));
                            dataTable.Columns.Add("Account_No", typeof(string));
                            dataTable.Columns.Add("Process_Id", typeof(string));
                            dataTable.Columns.Add("Status", typeof(bool));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            DataTable dataTable2 = new DataTable();
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    dataTable2.Columns.Add("Employee_Id", typeof(int));
                            //    dataTable2.Columns.Add("IP_Address", typeof(string));
                            //    dataTable2.Columns.Add("Trace_Date", typeof(DateTime));
                            //    dataTable2.Columns.Add("Trace_Time", typeof(TimeSpan));
                            //    dataTable2.Columns.Add("Record_Type", typeof(string));
                            //    dataTable2.Columns.Add("Bank_Id", typeof(int));
                            //    dataTable2.Columns.Add("Currency", typeof(string));
                            //    dataTable2.Columns.Add("Account_Type", typeof(int));
                            //    dataTable2.Columns.Add("Account_No", typeof(string));
                            //    dataTable2.Columns.Add("Process_Id", typeof(string));
                            //    dataTable2.Columns.Add("Status", typeof(bool));
                            //    dataTable2.Columns.Add("Company_Bank_Id", typeof(int));
                            //}

                            foreach (var item in company_Master.Company_Bank_List)
                            {
                                dataTable.Rows.Add(item.Company_Bank_Id, company_Id, item.Bank_Id, item.Currency, item.Account_Type, item.Account_No, item.Process_Id, item.Status, item.QueryFlag);
                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable2.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, item.Bank_Id, item.Currency, item.Account_Type, item.Account_No, item.Process_Id, item.Status, item.Company_Bank_Id);
                                //}
                            }
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    await _companyService.Insert_Company_Bank_Trace(dataTable2);
                            //}
                            await _companyService.InsertCompanyBank(dataTable);
                        }
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = "Company added successfully."
                        });
                    }
                    else if (message == "_error_order_no")
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (message == "_error_sort_no")
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Company_Details", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("companydelete")]
        [Authorize]
        public async Task<IActionResult> DeleteCompany(int companyId)
        {
            try
            {
                var result = await _companyService.DeleteCompany(companyId);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = "Company deleted successfully."
                    });
                }
                return Ok(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "DeleteCompany", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("changestatuscompany")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusCompany(int company_Id, bool status)
        {
            try
            {
                var result = await _companyService.CompanyChangeStatus(company_Id, status);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.StatusChangedSuccessMessage
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusCompany", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_company_max_order_no")]
        [Authorize]
        public async Task<IActionResult> Get_Company_Max_Order_No()
        {
            try
            {
                var result = await _companyService.Get_Company_Max_Order_No();

                return Ok(new
                {
                    statusCode = HttpStatusCode.OK,
                    order_no = result
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Company_Max_Order_No", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion
    }
}
