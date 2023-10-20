using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using astute.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
    public partial class PartyController : ControllerBase
    {
        #region Fields
        private readonly IPartyService _partyService;
        private readonly IConfiguration _configuration;
        private readonly ICommonService _commonService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISupplierService _supplierService;
        #endregion

        #region Ctor
        public PartyController(IPartyService partyService,
            IConfiguration configuration,
            ICommonService commonService,
            IHttpContextAccessor httpContextAccessor,
            ISupplierService supplierService)
        {
            _partyService = partyService;
            _configuration = configuration;
            _commonService = commonService;
            _httpContextAccessor = httpContextAccessor;
            _supplierService = supplierService;
        }
        #endregion

        #region Methods
        #region Party Master
        [HttpGet]
        [Route("getparty")]
        [Authorize]
        public async Task<IActionResult> GetParty(int party_Id)
        {
            try
            {
                var result = await _partyService.GetParty(party_Id);
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
            catch
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("deleteparty")]
        [Authorize]
        public async Task<IActionResult> DeleteParty(int party_Id)
        {
            try
            {
                var (message, result) = await _partyService.DeleteParty(party_Id);
                if (message == "success" && result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.PartyMasterDeleted
                    });
                }
                else if (message == "_reference_found" && result == (int)HttpStatusCode.Conflict)
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = "Reference found in the Party Api/ Party File/ Party FTP, you can not delete this record."
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = "parameter mismatched."
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "DeleteEmployee", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Party Bank
        [HttpPut]
        [Route("changestatuspartybank")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusPartyBank(int account_Id, bool status)
        {
            try
            {
                var result = await _partyService.PartyBankChangeStatus(account_Id, status);
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
            catch
            {
                throw;
            }
        }
        #endregion

        #region Party Shipping
        [HttpPut]
        [Route("changestatuspartyshipping")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusPartyShipping(int shipId, bool status)
        {
            try
            {
                var result = await _partyService.PartyShippingChangeStatus(shipId, status);
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
            catch
            {
                throw;
            }
        }
        #endregion
        #endregion

        #region Party Details
        [HttpGet]
        [Route("get_party_details")]
        [Authorize]
        public async Task<IActionResult> Get_Party_Details(int party_Id)
        {
            try
            {
                var result = await _partyService.Get_Party_Details(party_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Party_Details", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_party_detils")]
        [Authorize]
        public async Task<IActionResult> Create_Party_Detils([FromForm] Party_Master party_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
                    var (message, party_Id) = await _partyService.Add_Update_Party(party_Master);
                    if (message == "success" && party_Id > 0)
                    {
                        // Party Contact Detail
                        if (party_Master.Party_Contact_List != null && party_Master.Party_Contact_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Contact_Id", typeof(int));
                            dataTable.Columns.Add("Party_Id", typeof(int));
                            dataTable.Columns.Add("Contact_Name", typeof(string));
                            dataTable.Columns.Add("Sex", typeof(string));
                            dataTable.Columns.Add("Designation_Id", typeof(int));
                            dataTable.Columns.Add("Mobile_No", typeof(string));
                            dataTable.Columns.Add("Email", typeof(string));
                            dataTable.Columns.Add("Birth_Date", typeof(string));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            DataTable dataTable1 = new DataTable();
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                dataTable1.Columns.Add("Employee_Id", typeof(int));
                                dataTable1.Columns.Add("IP_Address", typeof(string));
                                dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                                dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                                dataTable1.Columns.Add("Record_Type", typeof(string));
                                dataTable1.Columns.Add("Party_Id", typeof(int));
                                dataTable1.Columns.Add("Contact_Name", typeof(string));
                                dataTable1.Columns.Add("Sex", typeof(string));
                                dataTable1.Columns.Add("Designation_Id", typeof(int));
                                dataTable1.Columns.Add("Mobile_No", typeof(string));
                                dataTable1.Columns.Add("Email", typeof(string));
                                dataTable1.Columns.Add("Birth_Date", typeof(string));
                            }

                            foreach (var item in party_Master.Party_Contact_List)
                            {
                                dataTable.Rows.Add(item.Contact_Id, party_Id, item.Contact_Name, item.Sex, item.Designation_Id, item.Mobile_No, item.Email, item.Birth_Date, item.QueryFlag);
                                if (CoreService.Enable_Trace_Records(_configuration))
                                {
                                    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Contact_Name, item.Sex, item.Designation_Id, item.Mobile_No, item.Email, item.Birth_Date, false);
                                }
                            }
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                await _partyService.Insert_Party_Contact_Trace(dataTable1);
                            }
                            await _partyService.AddUpdatePartyContact(dataTable);
                        }
                        // Party Assist
                        if (party_Master.Party_Assist_List != null && party_Master.Party_Assist_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Assist_Id", typeof(int));
                            dataTable.Columns.Add("Party_Id", typeof(int));
                            dataTable.Columns.Add("Diamond_Type", typeof(int));
                            dataTable.Columns.Add("Assist_1", typeof(int));
                            dataTable.Columns.Add("Per_1", typeof(decimal));
                            dataTable.Columns.Add("Assist_2", typeof(int));
                            dataTable.Columns.Add("Per_2", typeof(decimal));
                            dataTable.Columns.Add("Assist_3", typeof(int));
                            dataTable.Columns.Add("QueryFlag", typeof(string));
                            dataTable.Columns.Add("Date", typeof(string));

                            DataTable dataTable1 = new DataTable();
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                dataTable1.Columns.Add("Employee_Id", typeof(int));
                                dataTable1.Columns.Add("IP_Address", typeof(string));
                                dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                                dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                                dataTable1.Columns.Add("Record_Type", typeof(string));
                                dataTable1.Columns.Add("Party_Id", typeof(int));
                                dataTable1.Columns.Add("Diamond_Type", typeof(int));
                                dataTable1.Columns.Add("Assist_1", typeof(int));
                                dataTable1.Columns.Add("Per_1", typeof(decimal));
                                dataTable1.Columns.Add("Assist_2", typeof(int));
                                dataTable1.Columns.Add("Per_2", typeof(decimal));
                                dataTable1.Columns.Add("Assist_3", typeof(int));
                            }
                            foreach (var item in party_Master.Party_Assist_List)
                            {
                                dataTable.Rows.Add(item.Assist_Id, party_Id, item.Diamond_Type, item.Assist_1, item.Per_1, item.Assist_2, item.Per_2, item.Assist_3, item.QueryFlag, item.Date);

                                if (CoreService.Enable_Trace_Records(_configuration))
                                {
                                    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Diamond_Type, item.Assist_1, item.Per_1, item.Assist_2, item.Per_2, item.Assist_3);
                                }
                            }
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                await _partyService.Insert_Party_Assist_Trace(dataTable1);
                            }
                            await _partyService.AddUpdatePartyAssist(dataTable);
                        }
                        // Party Bank Detail
                        if (party_Master.Party_Bank_List != null && party_Master.Party_Bank_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Account_Id", typeof(int));
                            dataTable.Columns.Add("Party_Id", typeof(int));
                            dataTable.Columns.Add("Bank_Id", typeof(int));
                            dataTable.Columns.Add("Account_No", typeof(string));
                            dataTable.Columns.Add("Status", typeof(bool));
                            dataTable.Columns.Add("Account_Type", typeof(int));
                            dataTable.Columns.Add("Default_Bank", typeof(bool));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            DataTable dataTable1 = new DataTable();
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                dataTable1.Columns.Add("Employee_Id", typeof(int));
                                dataTable1.Columns.Add("IP_Address", typeof(string));
                                dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                                dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                                dataTable1.Columns.Add("Record_Type", typeof(string));
                                dataTable1.Columns.Add("Party_Id", typeof(int));
                                dataTable1.Columns.Add("Bank_Id", typeof(int));
                                dataTable1.Columns.Add("Account_No", typeof(int));
                                dataTable1.Columns.Add("Status", typeof(bool));
                                dataTable1.Columns.Add("Account_Type", typeof(int));
                            }

                            foreach (var item in party_Master.Party_Bank_List)
                            {
                                dataTable.Rows.Add(item.Account_Id, party_Id, item.Bank_Id, item.Account_No, item.Status, item.Account_Type, item.Default_Bank, item.QueryFlag);
                                if (CoreService.Enable_Trace_Records(_configuration))
                                {
                                    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Bank_Id, item.Account_No, item.Status, item.Account_Type);
                                }
                            }
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                await _partyService.Insert_Party_Bank_Trace(dataTable1);
                            }
                            await _partyService.AddUpdatePartyBank(dataTable);
                        }
                        // Party KYC Document
                        if (party_Master.Party_Document_List != null && party_Master.Party_Document_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Document_Id", typeof(int));
                            dataTable.Columns.Add("Party_Id", typeof(int));
                            dataTable.Columns.Add("Document_Type", typeof(int));
                            dataTable.Columns.Add("Document_No", typeof(string));
                            dataTable.Columns.Add("Upload_Path", typeof(string));
                            dataTable.Columns.Add("Valid_From", typeof(string));
                            dataTable.Columns.Add("Valid_To", typeof(string));
                            dataTable.Columns.Add("Kyc_Grade", typeof(int));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            DataTable dataTable1 = new DataTable();
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                dataTable1.Columns.Add("Employee_Id", typeof(int));
                                dataTable1.Columns.Add("IP_Address", typeof(string));
                                dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                                dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                                dataTable1.Columns.Add("Record_Type", typeof(string));
                                dataTable1.Columns.Add("Party_Id", typeof(int));
                                dataTable1.Columns.Add("Document_Type", typeof(int));
                                dataTable1.Columns.Add("Document_No", typeof(string));
                                dataTable1.Columns.Add("Upload_Path", typeof(int));
                                dataTable1.Columns.Add("Valid_From", typeof(DateTime));
                                dataTable1.Columns.Add("Valid_To", typeof(DateTime));
                                dataTable1.Columns.Add("Kyc_Grade", typeof(int));
                            }

                            foreach (var item in party_Master.Party_Document_List)
                            {
                                if (item.Upload_Path_Name != null && item.Upload_Path_Name.Length > 0)
                                {
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/PartyDocuments");
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
                                dataTable.Rows.Add(item.Document_Id, party_Id, item.Document_Type, item.Document_No, item.Upload_Path, item.Valid_From, item.Valid_To, item.Kyc_Grade, item.QueryFlag);

                                if (CoreService.Enable_Trace_Records(_configuration))
                                {
                                    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Document_Type, item.Document_No, item.Upload_Path, item.Valid_From, item.Valid_To, item.Kyc_Grade);
                                }
                            }
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                await _partyService.Insert_Party_Document_Trace(dataTable1);
                            }
                            await _partyService.AddUpdatePartyDocument(dataTable);
                        }
                        //Party Media
                        if (party_Master.Party_Media_List != null && party_Master.Party_Media_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Party_Media_Id", typeof(int));
                            dataTable.Columns.Add("Party_Id", typeof(int));
                            dataTable.Columns.Add("Cat_val_Id", typeof(int));
                            dataTable.Columns.Add("ID", typeof(string));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            #region Party Media Log
                            DataTable dataTable1 = new DataTable();
                            dataTable1.Columns.Add("Employee_Id", typeof(int));
                            dataTable1.Columns.Add("IP_Address", typeof(string));
                            dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                            dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                            dataTable1.Columns.Add("Record_Type", typeof(string));
                            dataTable1.Columns.Add("Party_Id", typeof(int));
                            dataTable1.Columns.Add("Cat_val_Id", typeof(int));
                            dataTable1.Columns.Add("ID", typeof(string));
                            #endregion

                            foreach (var item in party_Master.Party_Media_List)
                            {
                                dataTable.Rows.Add(party_Id, item.Cat_val_Id, item.ID, item.QueryFlag);
                                if (CoreService.Enable_Trace_Records(_configuration))
                                {
                                    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Cat_val_Id, item.ID);
                                }
                            }
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                await _partyService.Insert_Party_Media_Trace(dataTable1);
                            }
                            await _partyService.AddUpdatePartyMedia(dataTable);
                        }
                        // Party Shipping Detail
                        if (party_Master.Party_Shipping_List != null && party_Master.Party_Shipping_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Ship_Id", typeof(int));
                            dataTable.Columns.Add("Party_Id", typeof(int));
                            dataTable.Columns.Add("Company_Name", typeof(string));
                            dataTable.Columns.Add("Address_1", typeof(string));
                            dataTable.Columns.Add("Address_2", typeof(string));
                            dataTable.Columns.Add("Address_3", typeof(string));
                            dataTable.Columns.Add("City_Id", typeof(int));
                            dataTable.Columns.Add("Mobile_No", typeof(string));
                            dataTable.Columns.Add("Phone_No", typeof(string));
                            dataTable.Columns.Add("Contact_Person", typeof(string));
                            dataTable.Columns.Add("Contact_Email", typeof(string));
                            dataTable.Columns.Add("Default_Address", typeof(bool));
                            dataTable.Columns.Add("Status", typeof(bool));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            DataTable dataTable1 = new DataTable();
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                dataTable1.Columns.Add("Employee_Id", typeof(int));
                                dataTable1.Columns.Add("IP_Address", typeof(string));
                                dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                                dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                                dataTable1.Columns.Add("Record_Type", typeof(string));
                                dataTable1.Columns.Add("Party_Id", typeof(int));
                                dataTable1.Columns.Add("Company_Name", typeof(string));
                                dataTable1.Columns.Add("Address_1", typeof(string));
                                dataTable1.Columns.Add("Address_2", typeof(string));
                                dataTable1.Columns.Add("Address_3", typeof(string));
                                dataTable1.Columns.Add("City_Id", typeof(int));
                                dataTable1.Columns.Add("Mobile_No", typeof(string));
                                dataTable1.Columns.Add("Phone_No", typeof(string));
                                dataTable1.Columns.Add("Contact_Person", typeof(string));
                                dataTable1.Columns.Add("Contact_Email", typeof(string));
                                dataTable1.Columns.Add("Default_Address", typeof(bool));
                                dataTable1.Columns.Add("Status", typeof(bool));
                            }

                            foreach (var item in party_Master.Party_Shipping_List)
                            {
                                dataTable.Rows.Add(item.Ship_Id, party_Id, item.Company_Name, item.Address_1, item.Address_2, item.Address_3, item.City_Id, item.Mobile_No, item.Phone_No, item.Contact_Person, item.Contact_Email, item.Default_Address, item.Status, item.QueryFlag);
                                if (CoreService.Enable_Trace_Records(_configuration))
                                {
                                    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Company_Name, item.Address_1, item.Address_2, item.Address_3, item.City_Id, item.Mobile_No, item.Phone_No, item.Contact_Person, item.Contact_Email, item.Default_Address, item.Status);
                                }
                            }
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                await _partyService.Insert_Party_Shipping_Trace(dataTable1);
                            }
                            await _partyService.AddUpdatePartyShipping(dataTable);
                        }
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.PartyMasterCreated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Party_Detils", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Supplier Details
        [HttpPost]
        [Route("create_update_supplier_detail")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Supplier_Detail([FromForm] Supplier_Details supplier_Details, IFormFile File_Location, IFormFile File_Location_1, IFormFile File_Location_2)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool success = false;
                    if (supplier_Details.Party_Api != null)
                    {
                        supplier_Details.Party_Api.Party_Id = supplier_Details.Party_Id;
                        var party_Api = await _partyService.Add_Update_Party_API(supplier_Details.Party_Api);
                        if (party_Api > 0)
                        {
                            success = true;
                        }
                    }
                    if (supplier_Details.Party_FTP != null)
                    {
                        supplier_Details.Party_FTP.Party_Id = supplier_Details.Party_Id;
                        var party_ftp = await _partyService.Add_Update_Party_FTP(supplier_Details.Party_FTP);
                        if (party_ftp > 0)
                        {
                            success = true;
                        }
                    }
                    if (supplier_Details.Party_File != null)
                    {
                        if (File_Location != null && File_Location.Length > 0)
                        {
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/SupplierDetailFile");
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
                            supplier_Details.Party_File.File_Location = strFile;
                        }
                        if (File_Location_1 != null && File_Location_1.Length > 0)
                        {
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/SupplierDetailFile");
                            if (!(Directory.Exists(filePath)))
                            {
                                Directory.CreateDirectory(filePath);
                            }
                            string fileName = Path.GetFileNameWithoutExtension(File_Location_1.FileName);
                            string fileExt = Path.GetExtension(File_Location_1.FileName);

                            string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                            using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                            {
                                await File_Location_1.CopyToAsync(fileStream);
                            }
                            supplier_Details.Party_File.File_Location_1 = strFile;
                        }
                        if (File_Location_2 != null && File_Location_2.Length > 0)
                        {
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/SupplierDetailFile");
                            if (!(Directory.Exists(filePath)))
                            {
                                Directory.CreateDirectory(filePath);
                            }
                            string fileName = Path.GetFileNameWithoutExtension(File_Location_2.FileName);
                            string fileExt = Path.GetExtension(File_Location_2.FileName);

                            string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                            using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                            {
                                await File_Location_2.CopyToAsync(fileStream);
                            }
                            supplier_Details.Party_File.File_Location_2 = strFile;
                        }
                        supplier_Details.Party_File.Party_Id = supplier_Details.Party_Id;
                        var party_file = await _partyService.Add_Update_Party_File(supplier_Details.Party_File);
                        if (party_file > 0)
                        {
                            success = true;
                        }
                    }
                    if (supplier_Details.Supplier_Column_Mapping_List != null && supplier_Details.Supplier_Column_Mapping_List.Count > 0)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("Supp_Col_Id", typeof(int));
                        dataTable.Columns.Add("Supp_Id", typeof(int));
                        dataTable.Columns.Add("Col_Id", typeof(int));
                        dataTable.Columns.Add("Supp_Col_Name", typeof(string));
                        dataTable.Columns.Add("Column_Type", typeof(string));
                        dataTable.Columns.Add("Column_Synonym", typeof(string));

                        foreach (var item in supplier_Details.Supplier_Column_Mapping_List)
                        {
                            dataTable.Rows.Add(item.Supp_Col_Id, supplier_Details.Party_Id, item.Col_Id, item.Supp_Col_Name, item.Column_Type, item.Column_Synonym);
                        }
                        var result = await _partyService.Add_Update_Supplier_Column_Mapping(dataTable);
                        if (result > 0)
                        {
                            success = true;
                        }
                    }
                    if (success)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.SupplierDetailSavedSuccessfully
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Supplier_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_supplier_detail")]
        [Authorize]
        public async Task<IActionResult> Get_Supplier_Detail(int party_Id)
        {
            try
            {
                var supplier_detail = new Supplier_Details();
                supplier_detail.Party_Id = party_Id;
                supplier_detail.Party_Api = await _partyService.Get_Party_API(0, party_Id);
                supplier_detail.Party_FTP = await _partyService.Get_Party_FTP(0, party_Id);
                supplier_detail.Party_File = await _partyService.Get_Party_File(0, party_Id);
                supplier_detail.Supplier_Column_Mapping_List = await _partyService.Get_Supplier_Column_Mapping(party_Id);
                if (supplier_detail.Party_File != null)
                {
                    supplier_detail.Party_File.File_Location = !string.IsNullOrEmpty(supplier_detail.Party_File.File_Location) ? _configuration["BaseUrl"] + CoreCommonFilePath.SupplierFilePath + supplier_detail.Party_File.File_Location : null;
                    supplier_detail.Party_File.File_Location_1 = !string.IsNullOrEmpty(supplier_detail.Party_File.File_Location_1) ? _configuration["BaseUrl"] + CoreCommonFilePath.SupplierFilePath + supplier_detail.Party_File.File_Location_1 : null;
                    supplier_detail.Party_File.File_Location_2 = !string.IsNullOrEmpty(supplier_detail.Party_File.File_Location_2) ? _configuration["BaseUrl"] + CoreCommonFilePath.SupplierFilePath + supplier_detail.Party_File.File_Location_2 : null;
                }
                return Ok(new
                {
                    statusCode = HttpStatusCode.OK,
                    message = CoreCommonMessage.DataSuccessfullyFound,
                    data = supplier_detail
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Supplier_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_supplier_detail_list")]
        [Authorize]
        public async Task<IActionResult> Get_Supplier_Detail_List(int party_Id)
        {
            try
            {
                var result = await _partyService.Get_Suplier_Detail_List(party_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Supplier_Detail_List", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_party_supplier")]
        [Authorize]
        public async Task<IActionResult> Get_Party_Supplier()
        {
            try
            {
                var result = await _partyService.Get_Party_Suplier();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Party_Supplier", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_party_type_courier")]
        [Authorize]
        public async Task<IActionResult> Get_Party_Type_Courier()
        {
            try
            {
                var result = await _partyService.Get_Party_Type_Courier();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Party_Type_Courier", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_party_type_customer")]
        [Authorize]
        public async Task<IActionResult> Get_Party_Type_Customer()
        {
            try
            {
                var result = await _partyService.Get_Party_Type_Customer();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Party_Type_Customer", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_supplier_column_mapping")]
        [Authorize]
        public async Task<IActionResult> Get_Supplier_Column_Mapping(int party_Id)
        {
            try
            {
                var result = await _partyService.Get_Supplier_Column_Mapping(party_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Supplier_Column_Mapping", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Value Config
        [HttpGet]
        [Route("get_value_config")]
        [Authorize]
        public async Task<IActionResult> Get_Value_Config(int valueMap_ID)
        {
            try
            {
                var result = await _supplierService.Get_Value_Config(valueMap_ID);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Value_Config", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_value_config")]
        [Authorize]
        public async Task<IActionResult> Create_Value_Config(Value_Config value_Config)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _supplierService.Add_Update_Value_Config(value_Config);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ValueConfigCreated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Value_Config", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("update_value_config")]
        [Authorize]
        public async Task<IActionResult> Update_Value_Config(Value_Config value_Config)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _supplierService.Add_Update_Value_Config(value_Config);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ValueConfigUpdated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Update_Value_Config", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_value_config")]
        [Authorize]
        public async Task<IActionResult> Delete_Value_Config(int valueMap_ID)
        {
            try
            {
                var result = await _supplierService.Delete_Value_Config(valueMap_ID);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.ValueConfigDeleted
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
                await _commonService.InsertErrorLog(ex.Message, "Delete_Value_Config", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Supplier Pricing
        [HttpGet]
        [Route("get_supplier_pricing")]
        [Authorize]
        public async Task<IActionResult> Get_Supplier_Pricing(int supplier_Pricing_Id, int supplier_Id)
        {
            try
            {
                var result = await _partyService.Get_Supplier_Pricing(supplier_Pricing_Id, supplier_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Supplier_Pricing", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_supplier_pricing")]
        [Authorize]
        public async Task<IActionResult> Create_Supplier_Pricing(Supplier_Pricing supplier_Pricing)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _partyService.Add_Update_Supplier_Pricing(supplier_Pricing);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = supplier_Pricing.Supplier_Pricing_Id == 0 ? CoreCommonMessage.SupplierPricingCreated : CoreCommonMessage.SupplierPricingUpdated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Supplier_Pricing", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion
    }
}
