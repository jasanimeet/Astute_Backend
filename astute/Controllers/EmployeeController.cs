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
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace astute.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class EmployeeController : ControllerBase
    {
        #region Fields
        private readonly IEmployeeService _employeeService;
        private readonly IConfiguration _configuration;
        private readonly ICommonService _commonService;
        private readonly IEmailSender _emailSender;
        private readonly IEmpRightsService _empRightsService;
        private readonly IJWTAuthentication _jWTAuthentication;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public EmployeeController(IEmployeeService employeeService,
            IConfiguration configuration,
            ICommonService commonService,
            IEmailSender emailSender,
            IEmpRightsService empRightsService,
            IJWTAuthentication jWTAuthentication,
            IHttpContextAccessor httpContextAccessor)
        {
            _employeeService = employeeService;
            _configuration = configuration;
            _commonService = commonService;
            _emailSender = emailSender;
            _empRightsService = empRightsService;
            _jWTAuthentication = jWTAuthentication;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Methods
        #region Employee Master
        [HttpGet]
        [Route("getemployees")]
        [Authorize]
        public async Task<IActionResult> GetEmployees(int employeeId, string userName)
        {
            try
            {
                var result = await _employeeService.GetEmployees(employeeId, userName, "");
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
                await _commonService.InsertErrorLog(ex.Message, "GetEmployees", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_active_employees")]
        [Authorize]
        public async Task<IActionResult> Get_Active_Employees(int employeeId, string userName)
        {
            try
            {
                var result = await _employeeService.Get_Active_Employees(employeeId, userName, "");
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Active_Employees", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_employee_details")]
        [Authorize]
        public async Task<IActionResult> Get_Employee_Details(int employee_Id)
        {
            try
            {
                var result = await _employeeService.Get_Employee_Details(employee_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Employee_Details", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_employee_details")]
        [Authorize]
        public async Task<IActionResult> Create_Employee_Details([FromForm] Employee_Master employee_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
                    var (message, employee_Id) = await _employeeService.AddUpdateEmployee(employee_Master);
                    if (message == "success" && employee_Id > 0)
                    {
                        //Employee Documents
                        if (employee_Master.Employee_Document_List != null && employee_Master.Employee_Document_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Employee_Document_Id", typeof(int));
                            dataTable.Columns.Add("Employee_Id", typeof(int));
                            dataTable.Columns.Add("Document_Type", typeof(int));
                            dataTable.Columns.Add("Document_Expiry_Date", typeof(string));
                            dataTable.Columns.Add("Document_Url", typeof(string));
                            dataTable.Columns.Add("Document_Url_2", typeof(string));
                            dataTable.Columns.Add("Document_Url_3", typeof(string));
                            dataTable.Columns.Add("Document_Url_4", typeof(string));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            #region Employee Document Log
                            DataTable dataTable1 = new DataTable();
                            dataTable1.Columns.Add("Logged_Employee_Id", typeof(int));
                            dataTable1.Columns.Add("IP_Address", typeof(string));
                            dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                            dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                            dataTable1.Columns.Add("Record_Type", typeof(string));
                            dataTable1.Columns.Add("Employee_Id", typeof(int));
                            dataTable1.Columns.Add("Document_Type", typeof(int));
                            dataTable1.Columns.Add("Document_Expiry_Date", typeof(string));
                            dataTable1.Columns.Add("Document_Url", typeof(string));
                            dataTable1.Columns.Add("Document_Url_2", typeof(string));
                            dataTable1.Columns.Add("Document_Url_3", typeof(string));
                            dataTable1.Columns.Add("Document_Url_4", typeof(string));
                            #endregion

                            foreach (var item in employee_Master.Employee_Document_List)
                            {
                                if (item.Document_Url_Name != null && item.Document_Url_Name.Length > 0)
                                {
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/EmployeeDocuments");
                                    if (!(Directory.Exists(filePath)))
                                    {
                                        Directory.CreateDirectory(filePath);
                                    }

                                    string fileName = Path.GetFileNameWithoutExtension(item.Document_Url_Name.FileName);
                                    string fileExt = Path.GetExtension(item.Document_Url_Name.FileName);
                                    string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                                    using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                                    {
                                        await item.Document_Url_Name.CopyToAsync(fileStream);
                                    }
                                    item.Document_Url = strFile;
                                }
                                if (item.Document_Url_Name_2 != null && item.Document_Url_Name_2.Length > 0)
                                {
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/EmployeeDocuments");
                                    if (!(Directory.Exists(filePath)))
                                    {
                                        Directory.CreateDirectory(filePath);
                                    }

                                    string fileName = Path.GetFileNameWithoutExtension(item.Document_Url_Name_2.FileName);
                                    string fileExt = Path.GetExtension(item.Document_Url_Name_2.FileName);
                                    string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                                    using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                                    {
                                        await item.Document_Url_Name_2.CopyToAsync(fileStream);
                                    }
                                    item.Document_Url_2 = strFile;
                                }
                                if (item.Document_Url_Name_3 != null && item.Document_Url_Name_3.Length > 0)
                                {
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/EmployeeDocuments");
                                    if (!(Directory.Exists(filePath)))
                                    {
                                        Directory.CreateDirectory(filePath);
                                    }

                                    string fileName = Path.GetFileNameWithoutExtension(item.Document_Url_Name_3.FileName);
                                    string fileExt = Path.GetExtension(item.Document_Url_Name_3.FileName);
                                    string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                                    using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                                    {
                                        await item.Document_Url_Name_3.CopyToAsync(fileStream);
                                    }
                                    item.Document_Url_3 = strFile;
                                }
                                if (item.Document_Url_Name_4 != null && item.Document_Url_Name_4.Length > 0)
                                {
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/EmployeeDocuments");
                                    if (!(Directory.Exists(filePath)))
                                    {
                                        Directory.CreateDirectory(filePath);
                                    }

                                    string fileName = Path.GetFileNameWithoutExtension(item.Document_Url_Name_4.FileName);
                                    string fileExt = Path.GetExtension(item.Document_Url_Name_4.FileName);
                                    string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                                    using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                                    {
                                        await item.Document_Url_Name_4.CopyToAsync(fileStream);
                                    }
                                    item.Document_Url_4 = strFile;
                                }
                                string document_Expiry_Date = !string.IsNullOrEmpty(item.Document_Expiry_Date) ? item.Document_Expiry_Date : null;

                                dataTable.Rows.Add(item.Employee_Document_Id, employee_Id, item.Document_Type, document_Expiry_Date, item.Document_Url, item.Document_Url_2, item.Document_Url_3, item.Document_Url_4, item.QueryFlag);
                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, employee_Id, item.Document_Type, document_Expiry_Date, item.Document_Url, item.Document_Url_2, item.Document_Url_3, item.Document_Url_4);
                                //}
                            }
                            await _employeeService.InsertEmployeeDocument(dataTable);
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    await _employeeService.Insert_Employee_Document_Trace(dataTable1);
                            //}
                        }
                        if (employee_Master.Employee_Salary_List != null && employee_Master.Employee_Salary_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Employee_Salary_Id", typeof(int));
                            dataTable.Columns.Add("Employee_Id", typeof(int));
                            dataTable.Columns.Add("Salary", typeof(int));
                            dataTable.Columns.Add("Start_Date", typeof(string));
                            dataTable.Columns.Add("Salary_Type", typeof(string));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            #region Employee Salary Log
                            DataTable dataTable1 = new DataTable();
                            dataTable1.Columns.Add("Logged_Employee_Id", typeof(int));
                            dataTable1.Columns.Add("IP_Address", typeof(string));
                            dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                            dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                            dataTable1.Columns.Add("Record_Type", typeof(string));
                            dataTable1.Columns.Add("Employee_Id", typeof(int));
                            dataTable1.Columns.Add("Salary", typeof(int));
                            dataTable1.Columns.Add("Start_Date", typeof(string));
                            dataTable1.Columns.Add("Salary_Type", typeof(string));
                            #endregion

                            foreach (var item in employee_Master.Employee_Salary_List)
                            {
                                dataTable.Rows.Add(item.Employee_Salary_Id, employee_Id, item.Salary, item.Start_Date, item.Salary_Type, item.QueryFlag);
                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, employee_Id, item.Salary, item.Start_Date, item.Salary_Type);
                                //}
                            }
                            await _employeeService.InsertEmployeeSalary(dataTable);
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    await _employeeService.Insert_Employee_Salary_Trace(dataTable1);
                            //}
                        }
                        if (employee_Master.Emergency_Contact_Detail_List != null && employee_Master.Emergency_Contact_Detail_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Emergency_Contact_Detail_Id", typeof(int));
                            dataTable.Columns.Add("Employee_Id", typeof(int));
                            dataTable.Columns.Add("Name", typeof(string));
                            dataTable.Columns.Add("Relation", typeof(string));
                            dataTable.Columns.Add("Mobile", typeof(string));
                            dataTable.Columns.Add("Address", typeof(string));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            #region Emergency Contact Detail Log
                            DataTable dataTable1 = new DataTable();
                            dataTable1.Columns.Add("Logged_Employee_Id", typeof(int));
                            dataTable1.Columns.Add("IP_Address", typeof(string));
                            dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                            dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                            dataTable1.Columns.Add("Record_Type", typeof(string));
                            dataTable1.Columns.Add("Name", typeof(string));
                            dataTable1.Columns.Add("Relation", typeof(string));
                            dataTable1.Columns.Add("Mobile", typeof(string));
                            dataTable1.Columns.Add("Address", typeof(string));
                            #endregion

                            foreach (var item in employee_Master.Emergency_Contact_Detail_List)
                            {
                                dataTable.Rows.Add(item.Emergency_Contact_Detail_Id, employee_Id, item.Name, item.Relation, item.Mobile, item.Address, item.QueryFlag);
                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, item.Name, item.Relation, item.Mobile, item.Address);
                                //}
                            }
                            await _employeeService.Insert_Emergency_Contact_Detail(dataTable);
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    await _employeeService.Insert_Emergency_Contact_Detail_Trace(dataTable1);
                            //}
                        }
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = employee_Master.Employee_Id == 0 ? CoreCommonMessage.EmployeeMasterCreated : CoreCommonMessage.EmployeeMasterUpdated
                        });
                    }
                    else if (message == "_error_username_exist")
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.EmployeeExists
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
            catch (SqlException ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Employee_Details", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deleteemployee")]
        [Authorize]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            try
            {
                var (message, result) = await _employeeService.DeleteEmployee(employeeId);
                if (message == "success" && result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = "Employee deleted successfully."
                    });
                }
                else if (message == "_reference_found" && result == (int)HttpStatusCode.Conflict)
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = "Reference found in Party Assist, you can not delete this record."
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

        [HttpGet]
        [Route("get_employee_code")]
        [Authorize]
        public async Task<IActionResult> Get_Employee_Code()
        {
            try
            {
                var result = await _employeeService.Get_Employee_Code();
                if (result > 0)
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Employee_Code", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_employee_for_report")]
        [Authorize]
        public async Task<IActionResult> Get_Employee_For_Report(bool is_Exist, int rm_Id, int user_Id)
        {
            try
            {
                var result = await _employeeService.Get_Employee_For_Report(is_Exist, rm_Id, user_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Employee_For_Report", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("change_status_employee_master")]
        [Authorize]
        public async Task<IActionResult> Change_Status_Employee_Master(int employee_Id, bool status)
        {
            try
            {
                var result = await _employeeService.Employee_Master_Change_Status(employee_Id, status);
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
                await _commonService.InsertErrorLog(ex.Message, "Change_Status_Employee_Master", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_buyer")]
        [Authorize]
        public async Task<IActionResult> Get_Buyer()
        {
            try
            {
                var result = await _employeeService.Get_Buyer();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Buyer", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Employee Login
        //[HttpPost]
        //[Route("employee_login")]
        //public async Task<IActionResult> EmployeeLogin(UserModel userModel)
        //{
        //    try
        //    {
        //        var employee = await _employeeService.EmployeeLogin(userModel);
        //        if (employee != null)
        //        {
        //            var claims = new[] {
        //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        //                new Claim(JwtRegisteredClaimNames.Sub, employee.User_Name),
        //                new Claim(JwtRegisteredClaimNames.Email, employee.User_Name),
        //                new Claim(ClaimTypes.Name, employee.User_Name)
        //           };

        //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:SecretKey"]));

        //            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        //            var token = new JwtSecurityToken(_configuration["JwtToken:Issuer"], _configuration["JwtToken:Audience"], claims, expires: DateTime.UtcNow.AddSeconds(10), signingCredentials: signIn);
        //            var response = Ok(new
        //            {
        //                statusCode = HttpStatusCode.OK,
        //                data = employee,
        //                token = new JwtSecurityTokenHandler().WriteToken(token)
        //            });
        //            return response;

        //        }
        //        return BadRequest(new { message = "Username or password is incorrect" });
        //    }
        //    catch (Exception ex)
        //    {
        //        await _commonService.InsertErrorLog(ex.Message, "EmployeeLogin", ex.StackTrace);
        //        return Ok(new
        //        {
        //            message = ex.Message
        //        });
        //    }
        //}

        [HttpPost]
        [Route("employee_login")]
        public async Task<IActionResult> EmployeeLogin(UserModel userModel)
        {
            try
            {
                var response = await _employeeService.AuthenticateEmployee(userModel);
                if (response == null)
                    return BadRequest(new { message = "Username or password is incorrect" });
                else
                {
                    var auth_user = await _jWTAuthentication.Get_Employee_JWT_Token(response.Id);
                    if (auth_user != null)
                    {
                        auth_user.Token = response.Token;
                        await _jWTAuthentication.Insert_Update_Employee_JWT_Token(auth_user);
                    }
                    else
                    {
                        var jwt_auth_tken = new Employee_JWT_Token()
                        {
                            Token_Id = 0,
                            Employee_Id = response.Id,
                            Token = response.Token
                        };
                        await _jWTAuthentication.Insert_Update_Employee_JWT_Token(jwt_auth_tken);
                    }
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        user_Id = response.Id,
                        user_Name = response.Username,
                        is_Admin = response.Is_Admin,
                        token = response.Token,
                        user_Type = response.Usert_Type
                    });
                }
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "EmployeeLogin", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("employeelogout")]
        public async Task<IActionResult> EmployeeLogout(int user_Id)
        {
            await _jWTAuthentication.Delete_Employee_JWT_Token(user_Id);
            return Ok();
        }

        [HttpPost]
        [Route("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            var employee = await _employeeService.GetEmployees(changePasswordModel.EmoployeeId, null, null);
            if (employee != null && employee.Count > 0)
            {
                var empMaster = employee.FirstOrDefault();
                string decryptPasswor = CoreService.Decrypt(empMaster.Password);
                if (decryptPasswor.Equals(changePasswordModel.OldPassword))
                {
                    empMaster.Password = CoreService.Encrypt(changePasswordModel.NewPassword);
                    var result = await _employeeService.UpdateEmployee(empMaster);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.PasswordChengedSuccessMessage
                        });
                    }
                }
                return Unauthorized(new
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    message = CoreCommonMessage.OldPasswordNotMatched
                });
            }
            return NoContent();
        }
        #endregion

        #region Forget Password
        [HttpPost]
        [Route("forgetpassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordModel forgetPasswordModel)
        {
            var employee = await _employeeService.GetEmployees(0, forgetPasswordModel.UserName, null);
            if (employee != null && employee.Count > 0)
            {
                var emp = employee.FirstOrDefault();
                string forgetPasswordBodyTemplate = _emailSender.ForgetPasswordBodyTemplate(forgetPasswordModel.UserName, emp.First_Name);
                _emailSender.SendEmail(toEmail: emp.Personal_Email, externalLink: "", subject: CoreCommonMessage.ForgetPasswordSubject, strBody: forgetPasswordBodyTemplate);
                return Ok(new
                {
                    statusCode = HttpStatusCode.OK,
                    message = CoreCommonMessage.ForgetPasswordSuccessMessage
                });
            }
            return NoContent();
        }

        [HttpPost]
        [Route("userverification")]
        public async Task<IActionResult> UserVerification(string encryptCode)
        {
            if (!string.IsNullOrEmpty(encryptCode))
            {
                var decryptCode = CoreService.Decrypt(encryptCode);
                var employee = await _employeeService.GetEmployees(0, decryptCode, null);
                if (employee != null && employee.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = "Success",
                        userName = decryptCode
                    });
                }
            }
            return NoContent();
        }

        [HttpPost]
        [Route("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var employee = await _employeeService.GetEmployees(0, resetPasswordModel.UserName, null);
            if (employee != null)
            {
                var empMaster = employee.FirstOrDefault();
                empMaster.Password = resetPasswordModel.Password;
                var result = await _employeeService.UpdateEmployee(empMaster);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = "Password successfully set."
                    });
                }
            }
            return NoContent();
        }
        #endregion

        #region Employee Rights
        [HttpPost]
        [Route("addupdateemployeerights")]
        [Authorize]
        public async Task<IActionResult> AddUpdateEmployeeRights(Emp_rights_Model emp_Rights_Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _empRightsService.InsertUpdateEmpRights(emp_Rights_Model);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.EmployeeRightsCreated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "AddUpdateEmployeeRights", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("copyemployeerights")]
        [Authorize]
        public async Task<IActionResult> CopyEmployeeRights(int fromEmployeeId, int toEmployeeId)
        {
            try
            {
                var result = await _empRightsService.Copy_Emp_Rights(fromEmployeeId, toEmployeeId);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.EmployeeRightsCopy,
                    });
                }
                return BadRequest();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Employee Mail
        [HttpGet]
        [Route("getemployeemail")]
        public async Task<IActionResult> GetEmployeeMail(int employeeId)
        {
            try
            {
                var result = await _employeeService.GetEmployeeMail(employeeId);
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
                await _commonService.InsertErrorLog(ex.Message, "GetEmployeeMail", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createemployeemail")]
        public async Task<IActionResult> CreateEmployeeMail(Employee_Mail employee_Mail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _employeeService.InsertEmployeeMail(employee_Mail);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.EmployeeMailCreated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateEmployeeMail", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updateemployeemail")]
        public async Task<IActionResult> UpdateEmployeeMail(Employee_Mail employee_Mail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _employeeService.UpdateEmployeeMail(employee_Mail);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.EmployeeMailUpdated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateEmployeeMail", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deleteemployeemail")]
        public async Task<IActionResult> DeleteEmployeeMail(int employeeId)
        {
            try
            {
                var result = await _employeeService.DeleteEmployeeMail(employeeId);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.EmployeeMailDeleted,
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched,
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "DeleteEmployeeMail", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("sendtestemail")]
        public async Task<IActionResult> SendTestEmail(EmailSendModel model)
        {
            try
            {
                await _emailSender.SendTestEmail(toEmail: model.ToEmail, subject: model.Subject, strBody: model.EmailText, employeeId: model.EmployeeId);
                return Ok(new
                {
                    statusCode = HttpStatusCode.OK,
                    message = CoreCommonMessage.EmailSendSuccessMessage
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "SendTestEmail", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region EmployeeDecryptPass
        [HttpGet]
        [Route("getemployeesdecryptpass")]
        [Authorize]
        public async Task<IActionResult> GetEmployeesDecryptPass(int employee_Id)
        {
            try
            {
                var employees = await _employeeService.GetEmployees(employee_Id, null, null);

                if (employees != null && employees.Count > 0)
                {
                    var empMasterList = employees.Where(x => !string.IsNullOrEmpty(x.Password)).ToList();
                    empMasterList.ForEach(empMaster => empMaster.Password = CoreService.Decrypt(empMaster.Password));

                    var resultData = empMasterList.Select(x => new
                    {
                        Employee_Id = x.Employee_Id,
                        User_Name = x.User_Name,
                        Password = x.Password
                    });

                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = resultData
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetEmployeesDecryptPass", ex.StackTrace);
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
