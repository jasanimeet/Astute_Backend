using astute.CoreModel;
using astute.Models;
using astute.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
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
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthentication _jWTAuthentication;
        #endregion

        #region Ctor
        public UserController(IUserService userService,
            ICommonService commonService,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthentication jWTAuthentication)
        {
            _userService = userService;
            _commonService = commonService;
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
                    var (message, result) = await _userService.Add_Update_User(user_Registration);
                    if (message == "success" && result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.UserRegisteredSuccessfully
                        });
                    }
                    else if(message == "_error_username_exist" && result == 409)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.UserNameAlreadyExist
                        });
                    }
                    
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
                    var (message, result) = await _userService.Add_Update_User(user_Registration);
                    if (message == "success" && result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.UserUpdatedSuccessfully
                        });
                    }
                    else if (message == "_error_username_exist" && result == 409)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.UserNameAlreadyExist
                        });
                    }
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

        #region City/State/Country
        [HttpGet]
        [Route("get_active_cities")]        
        public async Task<IActionResult> Get_Active_Cities(string city)
        {
            try
            {
                var result = await _commonService.Get_Active_Cities(city);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Active_Cities", ex.StackTrace);
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
