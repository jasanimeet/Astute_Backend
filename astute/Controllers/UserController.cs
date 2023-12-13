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
    public partial class UserController : ControllerBase
    {
        #region Fields
        private readonly IUserService _userService;
        private readonly ICommonService _commonService;
        private readonly IAc_Group_Service _ac_group_service;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthentication _jWTAuthentication;
        #endregion

        #region Ctor
        public UserController(IUserService userService,
            ICommonService commonService,
            IAc_Group_Service ac_group_service,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthentication jWTAuthentication)
        {
            _userService = userService;
            _commonService = commonService;
            _ac_group_service = ac_group_service;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _jWTAuthentication = jWTAuthentication;
        }
        #endregion

        #region Methods
        #region User Registration
        [HttpPost]
        [Route("user_registration")]
        public virtual async Task<IActionResult> User_Registration([FromForm] User_Registration user_Registration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (user_Registration.Business_Reg_Upload_File != null && user_Registration.Business_Reg_Upload_File.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/UserDocuments");
                        if (!(Directory.Exists(filePath)))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        string fileName = Path.GetFileNameWithoutExtension(user_Registration.Business_Reg_Upload_File.FileName);
                        string fileExt = Path.GetExtension(user_Registration.Business_Reg_Upload_File.FileName);

                        string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                        using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                        {
                            await user_Registration.Business_Reg_Upload_File.CopyToAsync(fileStream);
                        }
                        user_Registration.Business_Reg_Upload = strFile;
                    }

                    if (user_Registration.Photo_Proof_Upload_File != null && user_Registration.Photo_Proof_Upload_File.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/UserDocuments");
                        if (!(Directory.Exists(filePath)))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        string fileName = Path.GetFileNameWithoutExtension(user_Registration.Photo_Proof_Upload_File.FileName);
                        string fileExt = Path.GetExtension(user_Registration.Photo_Proof_Upload_File.FileName);

                        string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                        using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                        {
                            await user_Registration.Photo_Proof_Upload_File.CopyToAsync(fileStream);
                        }
                        user_Registration.Photo_Proof_Upload = strFile;
                    }
                    if (user_Registration.Address_Proof_Upload_File != null && user_Registration.Address_Proof_Upload_File.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/UserDocuments");
                        if (!(Directory.Exists(filePath)))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        string fileName = Path.GetFileNameWithoutExtension(user_Registration.Address_Proof_Upload_File.FileName);
                        string fileExt = Path.GetExtension(user_Registration.Address_Proof_Upload_File.FileName);

                        string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                        using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                        {
                            await user_Registration.Address_Proof_Upload_File.CopyToAsync(fileStream);
                        }
                        user_Registration.Address_Proof_Upload = strFile;
                    }
                    await _userService.Add_Update_User(user_Registration);
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.UserRegisteredSuccessfully
                    });
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "User_Registration", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("update_user")]
        public virtual async Task<IActionResult> Update_User(User_Registration user_Registration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (user_Registration.Business_Reg_Upload_File != null && user_Registration.Business_Reg_Upload_File.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/UserDocuments");
                        if (!(Directory.Exists(filePath)))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        string fileName = Path.GetFileNameWithoutExtension(user_Registration.Business_Reg_Upload_File.FileName);
                        string fileExt = Path.GetExtension(user_Registration.Business_Reg_Upload_File.FileName);

                        string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                        using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                        {
                            await user_Registration.Business_Reg_Upload_File.CopyToAsync(fileStream);
                        }
                        user_Registration.Business_Reg_Upload = strFile;
                    }

                    if (user_Registration.Photo_Proof_Upload_File != null && user_Registration.Photo_Proof_Upload_File.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/UserDocuments");
                        if (!(Directory.Exists(filePath)))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        string fileName = Path.GetFileNameWithoutExtension(user_Registration.Photo_Proof_Upload_File.FileName);
                        string fileExt = Path.GetExtension(user_Registration.Photo_Proof_Upload_File.FileName);

                        string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                        using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                        {
                            await user_Registration.Photo_Proof_Upload_File.CopyToAsync(fileStream);
                        }
                        user_Registration.Photo_Proof_Upload = strFile;
                    }
                    if (user_Registration.Address_Proof_Upload_File != null && user_Registration.Address_Proof_Upload_File.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/UserDocuments");
                        if (!(Directory.Exists(filePath)))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        string fileName = Path.GetFileNameWithoutExtension(user_Registration.Address_Proof_Upload_File.FileName);
                        string fileExt = Path.GetExtension(user_Registration.Address_Proof_Upload_File.FileName);

                        string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                        using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                        {
                            await user_Registration.Address_Proof_Upload_File.CopyToAsync(fileStream);
                        }
                        user_Registration.Address_Proof_Upload = strFile;
                    }
                    await _userService.Add_Update_User(user_Registration);
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.UserUpdatedSuccessfully
                    });
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Update_User", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_user")]
        public virtual async Task<IActionResult> Get_User(int user_Id)
        {
            try
            {
                var result = await _userService.Get_User(user_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_User", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Account Group
        [HttpGet]
        [Route("get_ac_group")]
        [Authorize]
        public virtual async Task<IActionResult> Get_Ac_Group(int ac_Group_Id)
        {
            try
            {
                var result = await _ac_group_service.Get_Ac_Group(ac_Group_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Ac_Group", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_active_ac_group")]
        [Authorize]
        public virtual async Task<IActionResult> Get_Active_Ac_Group(int ac_Group_Id)
        {
            try
            {
                var result = await _ac_group_service.Get_Active_Ac_Group(ac_Group_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Active_Ac_Group", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_ac_group_detail")]
        [Authorize]
        public virtual async Task<IActionResult> Create_Ac_Group_Detail([FromForm] Ac_Group_Master ac_Group_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    var user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                    var (message, ac_group_Id) = await _ac_group_service.Add_Update_Ac_Group(ac_Group_Master);
                    if (message == "success" && ac_group_Id > 0)
                    {
                        if (ac_Group_Master.Ac_Group_Detail_List != null && ac_Group_Master.Ac_Group_Detail_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Ac_Group_Det_Id", typeof(int));
                            dataTable.Columns.Add("Ac_Group_Id", typeof(int));
                            dataTable.Columns.Add("Ac_Group_Det_Name", typeof(string));
                            dataTable.Columns.Add("Trans_Type", typeof(string));
                            dataTable.Columns.Add("Basic_Group", typeof(string));
                            dataTable.Columns.Add("Opp_Group_Det_Id", typeof(int));
                            dataTable.Columns.Add("Parent_Group", typeof(int));
                            dataTable.Columns.Add("Status", typeof(bool));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            //#region Ac group detail log
                            //DataTable dataTable1 = new DataTable();
                            //dataTable1.Columns.Add("Employee_Id", typeof(int));
                            //dataTable1.Columns.Add("IP_Address", typeof(string));
                            //dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                            //dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                            //dataTable1.Columns.Add("Record_Type", typeof(string));
                            //dataTable1.Columns.Add("Ac_Group_Id", typeof(int));
                            //dataTable1.Columns.Add("Ac_Group_Det_Name", typeof(string));
                            //dataTable1.Columns.Add("Trans_Type", typeof(string));
                            //dataTable1.Columns.Add("Basic_Group", typeof(string));
                            //dataTable1.Columns.Add("Opp_Group_Det_Id", typeof(int));
                            //dataTable1.Columns.Add("Parent_Group", typeof(int));
                            //#endregion

                            foreach (var item in ac_Group_Master.Ac_Group_Detail_List)
                            {
                                dataTable.Rows.Add(item.Ac_Group_Det_Id, ac_group_Id, item.Ac_Group_Det_Name, item.Trans_Type, item.Basic_Group, item.Opp_Group_Det_Id, item.Parent_Group, item.Status, item.QueryFlag);
                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable1.Rows.Add(user_Id ?? 0, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, ac_group_Id, item.Ac_Group_Det_Name, item.Trans_Type, item.Basic_Group, item.Opp_Group_Det_Id, item.Parent_Group);
                                //}
                            }
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    await _ac_group_service.Insert_Ac_Group_Detail_Trace(dataTable1);
                            //}
                            await _ac_group_service.Add_Update_Ac_Group_Detail(dataTable);
                        }
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = ac_Group_Master.Ac_Group_Id == 0 ? CoreCommonMessage.AccountGroupCreated : CoreCommonMessage.AccountGroupUpdated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Ac_Group_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_ac_group_detail")]
        [Authorize]
        public virtual async Task<IActionResult> Get_Ac_Group_Detail(int ac_Group_Id)
        {
            try
            {
                var result = await _ac_group_service.Get_Ac_Group_Detail(ac_Group_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Ac_Group_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_ac_group")]
        [Authorize]
        public async Task<IActionResult> Delete_Ac_Group(int ac_Group_Id)
        {
            try
            {
                var result = await _ac_group_service.Delete_Ac_Group(ac_Group_Id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.AccountGroupDeleted
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
                await _commonService.InsertErrorLog(ex.Message, "Delete_Ac_Group", ex.StackTrace);
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
