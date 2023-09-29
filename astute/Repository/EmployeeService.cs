using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class EmployeeService : IEmployeeService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly ICommonService _commonService;
        private readonly IConfiguration _configuration;
        private readonly IJWTAuthentication _jWTAuthentication;
        #endregion

        #region Ctor
        public EmployeeService(AstuteDbContext dbContext,
            ICommonService commonService,
            IConfiguration configuration,
            IJWTAuthentication jWTAuthentication)
        {
            _dbContext = dbContext;
            _commonService = commonService;
            _configuration = configuration;
            _jWTAuthentication = jWTAuthentication;
        }
        #endregion

        #region Methods
        #region Employee Master
        public async Task<(string, int)> AddUpdateEmployee(Employee_Master employee_Master)
        {
            var encryptPassword = CoreService.Encrypt(employee_Master.Password);

            var employeeId = new SqlParameter("@employeeId", employee_Master.Employee_Id);
            var initial = !string.IsNullOrEmpty(employee_Master.Initial) ? new SqlParameter("@initial", employee_Master.Initial) : new SqlParameter("@initial", DBNull.Value);
            var firstName = new SqlParameter("@firstName", employee_Master.First_Name);
            var middleName = !string.IsNullOrEmpty(employee_Master.Middle_Name) ? new SqlParameter("@middleName", employee_Master.Middle_Name) : new SqlParameter("@middleName", DBNull.Value);
            var lastName = !string.IsNullOrEmpty(employee_Master.Last_Name) ? new SqlParameter("@lastName", employee_Master.Last_Name) : new SqlParameter("@lastName", DBNull.Value);
            var chineseName = !string.IsNullOrEmpty(employee_Master.Chinese_Name) ? new SqlParameter("@chineseName", employee_Master.Chinese_Name) : new SqlParameter("@chineseName", DBNull.Value);
            var address1 = !string.IsNullOrEmpty(employee_Master.Address_1) ? new SqlParameter("@address1", employee_Master.Address_1) : new SqlParameter("@address1", DBNull.Value);
            var address2 = !string.IsNullOrEmpty(employee_Master.Address_2) ? new SqlParameter("@address2", employee_Master.Address_2) : new SqlParameter("@address2", DBNull.Value);
            var address3 = !string.IsNullOrEmpty(employee_Master.Address_3) ? new SqlParameter("@address3", employee_Master.Address_3) : new SqlParameter("@address3", DBNull.Value);
            var cityId = new SqlParameter("@cityId", employee_Master.City_Id);
            var joinDate = !string.IsNullOrEmpty(employee_Master.Join_date) ? new SqlParameter("@joindate", employee_Master.Join_date) : new SqlParameter("@joindate", DBNull.Value);
            var employeeType = !string.IsNullOrEmpty(employee_Master.Employee_Type) ? new SqlParameter("@employeeType", employee_Master.Employee_Type) : new SqlParameter("@employeeType", DBNull.Value);
            var birthDate = !string.IsNullOrEmpty(employee_Master.Birth_Date) ? new SqlParameter("@birthDate", employee_Master.Birth_Date) : new SqlParameter("@birthDate", DBNull.Value);
            var gender = !string.IsNullOrEmpty(employee_Master.Gender) ? new SqlParameter("@gender", employee_Master.Gender) : new SqlParameter("@gender", DBNull.Value);
            var mobileNo = !string.IsNullOrEmpty(employee_Master.Mobile_No) ? new SqlParameter("@mobileNo", employee_Master.Mobile_No) : new SqlParameter("@mobileNo", DBNull.Value);
            var personalEmail = !string.IsNullOrEmpty(employee_Master.Personal_Email) ? new SqlParameter("@personalEmail", employee_Master.Personal_Email) : new SqlParameter("@personalEmail", DBNull.Value);
            var companyEmail = !string.IsNullOrEmpty(employee_Master.Company_Email) ? new SqlParameter("@companyEmail", employee_Master.Company_Email) : new SqlParameter("@companyEmail", DBNull.Value);
            var leaveDate = !string.IsNullOrEmpty(employee_Master.Leave_Date) ? new SqlParameter("@leaveDate", employee_Master.Leave_Date) : new SqlParameter("@leaveDate", DBNull.Value);
            var pSNID = !string.IsNullOrEmpty(employee_Master.PSN_ID) ? new SqlParameter("@pSNID", employee_Master.PSN_ID) : new SqlParameter("@pSNID", DBNull.Value);
            var bloodGroup = !string.IsNullOrEmpty(employee_Master.Blood_Group) ? new SqlParameter("@bloodGroup", employee_Master.Blood_Group) : new SqlParameter("@bloodGroup", DBNull.Value);
            var contractStartDate = !string.IsNullOrEmpty(employee_Master.Contract_Start_Date) ? new SqlParameter("@contractStartDate", employee_Master.Contract_Start_Date) : new SqlParameter("@contractStartDate", DBNull.Value);
            var contractEndDate = !string.IsNullOrEmpty(employee_Master.Contract_End_Date) ? new SqlParameter("@contractEndDate", employee_Master.Contract_End_Date) : new SqlParameter("@contractEndDate", DBNull.Value);
            var approveHolidays = employee_Master.Approve_Holidays > 0 ? new SqlParameter("@approveHolidays", employee_Master.Approve_Holidays) : new SqlParameter("@approveHolidays", DBNull.Value);
            var orderNo = employee_Master.Order_No > 0 ? new SqlParameter("@orderNo", employee_Master.Order_No) : new SqlParameter("@orderNo", DBNull.Value);
            var sortNo = employee_Master.Sort_No > 0 ? new SqlParameter("@sortNo", employee_Master.Sort_No) : new SqlParameter("@sortNo", DBNull.Value);
            var photo_Upload = !string.IsNullOrEmpty(employee_Master.Photo_Upload) ? new SqlParameter("@photoUpload", employee_Master.Photo_Upload) : new SqlParameter("@photoUpload", DBNull.Value);
            var userName = new SqlParameter("@userName", employee_Master.User_Name);
            var password = new SqlParameter("@password", encryptPassword);
            var employeeCode = !string.IsNullOrEmpty(employee_Master.Employee_Code) ? new SqlParameter("@employee_Code", employee_Master.Employee_Code) : new SqlParameter("@employee_Code", DBNull.Value);
            var status = new SqlParameter("@status", employee_Master.Status);

            var isExistUserName = new SqlParameter("@IsExistUserName", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var insertedId = new SqlParameter("@InsertedId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Employee_Master_Insert_Update @employeeId, @initial, @firstName, @middleName, @lastName,
            @chineseName, @address1, @address2, @address3, @cityId, @joindate, @employeeType, @birthDate, @gender, @mobileNo, @personalEmail, @companyEmail,
            @leaveDate, @pSNID, @bloodGroup, @contractStartDate, @contractEndDate, @approveHolidays, @orderNo, @sortNo, @photoUpload, @userName, @password, @employee_Code, @status,
            @IsExistUserName OUT, @IsExistOrderNo OUT, @IsExistSortNo OUT, @InsertedId OUT",
            employeeId, initial, firstName, middleName, lastName, chineseName, address1, address2, address3, cityId, joinDate, employeeType, birthDate, gender, mobileNo,
            personalEmail, companyEmail, leaveDate, pSNID, bloodGroup, contractStartDate, contractEndDate, approveHolidays, orderNo, sortNo, photo_Upload, userName, password,
            employeeCode, status, isExistUserName, isExistOrderNo, isExistSortNo, insertedId));

            bool _isExistUserName = (bool)isExistUserName.Value;
            if (_isExistUserName)
                return ("_error_username_exist", 0);

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return ("_error_order_no", 0);

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return ("_error_sort_no", 0);

            if (result > 0)
            {
                int _insertedId = (int)insertedId.Value;
                return ("success", _insertedId);
            }
            return ("error", 0);
        }
        public async Task<Employee_Master> Get_Employee_Details(int emoployee_Id)
        {
            var empId = emoployee_Id > 0 ? new SqlParameter("@employeeId", emoployee_Id) : new SqlParameter("@employeeId", DBNull.Value);
            var uName = new SqlParameter("@userName", DBNull.Value);
            var email = new SqlParameter("@email", DBNull.Value);

            var employee_Master = await Task.Run(() => _dbContext.Employee_Master
                                .FromSqlRaw(@"exec Employee_Master_Select @employeeId, @userName, @email", empId, uName, email)
                                .AsEnumerable()
                                .FirstOrDefault());
            if (employee_Master != null)
            {
                employee_Master.Password = CoreService.Decrypt(employee_Master.Password);
                employee_Master.Photo_Upload = !string.IsNullOrEmpty(employee_Master.Photo_Upload) ? _configuration["BaseUrl"] + CoreCommonFilePath.EmployeeIconImagesPath + employee_Master.Photo_Upload : employee_Master.Photo_Upload;
                if (employee_Master.Employee_Id > 0)
                {
                    var _emp_Id = employee_Master.Employee_Id > 0 ? new SqlParameter("@employeeId", employee_Master.Employee_Id) : new SqlParameter("@employeeId", DBNull.Value);

                    employee_Master.Employee_Document_List = await Task.Run(() => _dbContext.Employee_Document
                                                            .FromSqlRaw(@"exec Employee_Document_Select @employeeId", _emp_Id).ToListAsync());

                    if (employee_Master.Employee_Document_List != null && employee_Master.Employee_Document_List.Count > 0)
                    {
                        foreach (var item in employee_Master.Employee_Document_List)
                        {
                            item.Document_Url = !string.IsNullOrEmpty(item.Document_Url) ? _configuration["BaseUrl"] + CoreCommonFilePath.EmployeeDocumentsPath + item.Document_Url : item.Document_Url;
                        }
                    }

                    employee_Master.Employee_Salary_List = await Task.Run(() => _dbContext.Employee_Salary
                                                            .FromSqlRaw(@"exec Employee_Salary_Select @employeeId", _emp_Id).ToListAsync());
                }
            }

            return employee_Master;
        }
        public async Task<int> UpdateEmployee(Employee_Master employee_Master)
        {
            var encryptPassword = CoreService.Encrypt(employee_Master.Password);

            var employeeId = new SqlParameter("@employeeId", employee_Master.Employee_Id);
            var initial = !string.IsNullOrEmpty(employee_Master.Initial) ? new SqlParameter("@initial", employee_Master.Initial) : new SqlParameter("@initial", DBNull.Value);
            var firstName = new SqlParameter("@firstName", employee_Master.First_Name);
            var middleName = !string.IsNullOrEmpty(employee_Master.Middle_Name) ? new SqlParameter("@middleName", employee_Master.Middle_Name) : new SqlParameter("@middleName", DBNull.Value);
            var lastName = !string.IsNullOrEmpty(employee_Master.Last_Name) ? new SqlParameter("@lastName", employee_Master.Last_Name) : new SqlParameter("@lastName", DBNull.Value);
            var chineseName = !string.IsNullOrEmpty(employee_Master.Chinese_Name) ? new SqlParameter("@chineseName", employee_Master.Chinese_Name) : new SqlParameter("@chineseName", DBNull.Value);
            var address1 = !string.IsNullOrEmpty(employee_Master.Address_1) ? new SqlParameter("@address1", employee_Master.Address_1) : new SqlParameter("@address1", DBNull.Value);
            var address2 = !string.IsNullOrEmpty(employee_Master.Address_2) ? new SqlParameter("@address2", employee_Master.Address_2) : new SqlParameter("@address2", DBNull.Value);
            var address3 = !string.IsNullOrEmpty(employee_Master.Address_3) ? new SqlParameter("@address3", employee_Master.Address_3) : new SqlParameter("@address3", DBNull.Value);
            var cityId = new SqlParameter("@cityId", employee_Master.City_Id);
            var joinDate = new SqlParameter("@joindate", employee_Master.Join_date);
            var employeeType = !string.IsNullOrEmpty(employee_Master.Employee_Type) ? new SqlParameter("@employeeType", employee_Master.Employee_Type) : new SqlParameter("@employeeType", DBNull.Value);
            var birthDate = new SqlParameter("@birthDate", employee_Master.Birth_Date);
            var gender = !string.IsNullOrEmpty(employee_Master.Gender) ? new SqlParameter("@gender", employee_Master.Gender) : new SqlParameter("@gender", DBNull.Value);
            var mobileNo = !string.IsNullOrEmpty(employee_Master.Mobile_No) ? new SqlParameter("@mobileNo", employee_Master.Mobile_No) : new SqlParameter("@mobileNo", DBNull.Value);
            var personalEmail = !string.IsNullOrEmpty(employee_Master.Personal_Email) ? new SqlParameter("@personalEmail", employee_Master.Personal_Email) : new SqlParameter("@personalEmail", DBNull.Value);
            var companyEmail = !string.IsNullOrEmpty(employee_Master.Company_Email) ? new SqlParameter("@companyEmail", employee_Master.Company_Email) : new SqlParameter("@companyEmail", DBNull.Value);
            var leaveDate = new SqlParameter("@leaveDate", employee_Master.Leave_Date);
            var pSNID = !string.IsNullOrEmpty(employee_Master.PSN_ID) ? new SqlParameter("@pSNID", employee_Master.PSN_ID) : new SqlParameter("@pSNID", DBNull.Value);
            var bloodGroup = !string.IsNullOrEmpty(employee_Master.Blood_Group) ? new SqlParameter("@bloodGroup", employee_Master.Blood_Group) : new SqlParameter("@bloodGroup", DBNull.Value);
            var contractStartDate = new SqlParameter("@contractStartDate", employee_Master.Contract_Start_Date);
            var contractEndDate = new SqlParameter("@contractEndDate", employee_Master.Contract_End_Date);
            var approveHolidays = employee_Master.Approve_Holidays > 0 ? new SqlParameter("@approveHolidays", employee_Master.Approve_Holidays) : new SqlParameter("@approveHolidays", DBNull.Value);
            var orderNo = new SqlParameter("@orderNo", employee_Master.Order_No);
            var sortNo = new SqlParameter("@sortNo", employee_Master.Sort_No);
            var photo_Upload = !string.IsNullOrEmpty(employee_Master.Photo_Upload) ? new SqlParameter("@photoUpload", employee_Master.Photo_Upload) : new SqlParameter("@photoUpload", DBNull.Value);
            var userName = new SqlParameter("@userName", employee_Master.User_Name);
            var password = new SqlParameter("@password", encryptPassword);
            var employeeCode = !string.IsNullOrEmpty(employee_Master.Employee_Code) ? new SqlParameter("@employee_Code", employee_Master.Employee_Code) : new SqlParameter("@employee_Code", DBNull.Value);
            var status = new SqlParameter("@status", employee_Master.Status);
            var recordType = new SqlParameter("@recordType", "Update");
            //var isForce_Insert = new SqlParameter("@IsForceInsert", employee_Master.IsForceInsert);
            var isExistUserName = new SqlParameter("@IsExistUserName", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Employee_Master_Insert_Update @employeeId, @initial, @firstName, @middleName, @lastName,
            @chineseName, @address1, @address2, @address3, @cityId, @joindate, @employeeType, @birthDate, @gender, @mobileNo, @personalEmail, @companyEmail,
            @leaveDate, @pSNID, @bloodGroup, @contractStartDate, @contractEndDate, @approveHolidays, @orderNo, @sortNo, @photoUpload, @userName, @password, @employee_Code, @status, @recordType,
            @IsExistUserName OUT, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
            employeeId, initial, firstName, middleName, lastName, chineseName, address1, address2, address3, cityId, joinDate, employeeType, birthDate, gender, mobileNo,
            personalEmail, companyEmail, leaveDate, pSNID, bloodGroup, contractStartDate, contractEndDate, approveHolidays, orderNo, sortNo, photo_Upload, userName, password,
            employeeCode, status, recordType, isExistUserName, isExistOrderNo, isExistSortNo));

            bool _isExistUserName = (bool)isExistUserName.Value;
            if (_isExistUserName)
                return 2;

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 3;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 4;

            return result;
        }
        public async Task<(string, int)> DeleteEmployee(int employeeId)
        {
            var isReferencedParameter = new SqlParameter("@IsReference", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Employee_Master_Delete @employeeId, @IsReference OUT",
                                        new SqlParameter("@employeeId", employeeId),
                                        isReferencedParameter);

            var isReferenced = (bool)isReferencedParameter.Value;
            if (isReferenced)
                return ("_reference_found", (int)HttpStatusCode.Conflict);

            if (result > 0)
                return ("success", result);
            else
                return ("success", result);
        }
        public async Task<IList<Employee_Master>> GetEmployees(int employeeId, string userName, string personalEmail)
        {
            var empId = employeeId > 0 ? new SqlParameter("@employeeId", employeeId) : new SqlParameter("@employeeId", DBNull.Value);
            var uName = !string.IsNullOrEmpty(userName) ? new SqlParameter("@userName", userName) : new SqlParameter("@userName", DBNull.Value);
            var email = !string.IsNullOrEmpty(personalEmail) ? new SqlParameter("@email", personalEmail) : new SqlParameter("@email", DBNull.Value);

            var employees = await Task.Run(() => _dbContext.Employee_Master
                            .FromSqlRaw(@"exec Employee_Master_Select @employeeId, @userName, @email", empId, uName, email).ToListAsync());
            if(employees != null && employees.Count > 0)
            {
                foreach (var employee in employees)
                {
                    employee.Photo_Upload = !string.IsNullOrEmpty(employee.Photo_Upload) ? _configuration["BaseUrl"] + CoreCommonFilePath.EmployeeIconImagesPath + employee.Photo_Upload : null;
                }
            }

            return employees;
        }
        public async Task<Employee_Master> EmployeeLogin(UserModel userModel)
        {
            var password = CoreService.Encrypt(userModel.Password);
            var parameter = new List<SqlParameter>();

            parameter.Add(new SqlParameter("@userName", userModel.UserName));
            parameter.Add(new SqlParameter("@password", password));

            var result = await Task.Run(() => _dbContext.Employee_Master
                        .FromSqlRaw(@"exec EmployeeLogin @userName, @password", parameter.ToArray())
                        .AsEnumerable()
                        .FirstOrDefault());

            return result;
        }

        public async Task<AuthenticateResponse> AuthenticateEmployee(UserModel userModel)
        {
            var password = CoreService.Encrypt(userModel.Password);
            var parameter = new List<SqlParameter>();

            parameter.Add(new SqlParameter("@userName", userModel.UserName));
            parameter.Add(new SqlParameter("@password", password));

            var employee = await Task.Run(() => _dbContext.Employee_Master
                        .FromSqlRaw(@"exec EmployeeLogin @userName, @password", parameter.ToArray())
                        .AsEnumerable()
                        .FirstOrDefault());

            if (employee == null)
                return null;

            var jwt_Token = _jWTAuthentication.Generate_Jwt_Token(employee);

            return new AuthenticateResponse(employee, jwt_Token);
        }
        #endregion

        #region Employee Document
        public async Task<int> InsertEmployeeDocument(DataTable dataTable)
        {
            var parameter = new SqlParameter("@employee_Document", SqlDbType.Structured)
            {
                TypeName = "dbo.Employee_Document_Data_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Employee_Document_Insert_Update @employee_Document", parameter);
            return result;
        }
        #endregion

        #region Employee Salary
        public async Task<int> InsertEmployeeSalary(DataTable dataTable)
        {
            var parameter = new SqlParameter("@employee_Salary", SqlDbType.Structured)
            {
                TypeName = "dbo.Employee_Salary_Data_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Employee_Salary_Insert_Update @employee_Salary", parameter);
            return result;
        }
        #endregion

        #region Employee Mail
        public async Task<IList<Employee_Mail>> GetEmployeeMail(int employeeId)
        {
            var employee_Id = employeeId > 0 ? new SqlParameter("@Employee_Id", employeeId) : new SqlParameter("@Employee_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Employee_Mail
                            .FromSqlRaw(@"exec Employee_Mail_Select @Employee_Id", employee_Id).ToListAsync());
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.Email_Password = CoreService.Decrypt(item.Email_Password);
                }
            }

            return result;
        }
        public async Task<int> InsertEmployeeMail(Employee_Mail employee_Mail)
        {
            var emalPass = CoreService.Encrypt(employee_Mail.Email_Password);
            var employeeId = new SqlParameter("@Employee_Id", employee_Mail.Employee_Id);
            var emailId = !string.IsNullOrEmpty(employee_Mail.Email_id) ? new SqlParameter("@Email_id", employee_Mail.Email_id) : new SqlParameter("@Email_id", DBNull.Value);
            var emailPassword = !string.IsNullOrEmpty(emalPass) ? new SqlParameter("@Email_Password", emalPass) : new SqlParameter("@Email_Password", DBNull.Value);
            var SMTPServer = !string.IsNullOrEmpty(employee_Mail.SMTP_Server) ? new SqlParameter("@SMTP_Server", employee_Mail.SMTP_Server) : new SqlParameter("@SMTP_Server", DBNull.Value);
            var SMTPServerAddress = !string.IsNullOrEmpty(employee_Mail.SMTP_Server_Address) ? new SqlParameter("@SMTP_Server_Address", employee_Mail.SMTP_Server_Address) : new SqlParameter("@SMTP_Server_Address", DBNull.Value);
            var SMTPPort = employee_Mail.SMTP_Port > 0 ? new SqlParameter("@SMTP_Port", employee_Mail.SMTP_Port) : new SqlParameter("@SMTP_Port", DBNull.Value);
            var enableSSL = new SqlParameter("@Enable_SSL", employee_Mail.Enable_SSL);
            var recordType = new SqlParameter("@recordType", "Insert");

            var result = await Task.Run(() => _dbContext.Database
                            .ExecuteSqlRawAsync(@"EXEC Employee_Mail_Insert_Update @Employee_Id, @Email_id, @Email_Password, @SMTP_Server, @SMTP_Server_Address, @SMTP_Port, 
                            @Enable_SSL, @recordType", employeeId, emailId, emailPassword, SMTPServer, SMTPServerAddress, SMTPPort, enableSSL, recordType));
            return result;
        }
        public async Task<int> UpdateEmployeeMail(Employee_Mail employee_Mail)
        {
            var emalPass = CoreService.Encrypt(employee_Mail.Email_Password);
            var employeeId = new SqlParameter("@Employee_Id", employee_Mail.Employee_Id);
            var emailId = !string.IsNullOrEmpty(employee_Mail.Email_id) ? new SqlParameter("@Email_id", employee_Mail.Email_id) : new SqlParameter("@Email_id", DBNull.Value);
            var emailPassword = !string.IsNullOrEmpty(emalPass) ? new SqlParameter("@Email_Password", emalPass) : new SqlParameter("@Email_Password", DBNull.Value);
            var SMTPServer = !string.IsNullOrEmpty(employee_Mail.SMTP_Server) ? new SqlParameter("@SMTP_Server", employee_Mail.SMTP_Server) : new SqlParameter("@SMTP_Server", DBNull.Value);
            var SMTPServerAddress = !string.IsNullOrEmpty(employee_Mail.SMTP_Server_Address) ? new SqlParameter("@SMTP_Server_Address", employee_Mail.SMTP_Server_Address) : new SqlParameter("@SMTP_Server_Address", DBNull.Value);
            var SMTPPort = employee_Mail.SMTP_Port > 0 ? new SqlParameter("@SMTP_Port", employee_Mail.SMTP_Port) : new SqlParameter("@SMTP_Port", DBNull.Value);
            var enableSSL = new SqlParameter("@Enable_SSL", employee_Mail.Enable_SSL);
            var recordType = new SqlParameter("@recordType", "Update");

            var result = await Task.Run(() => _dbContext.Database
                            .ExecuteSqlRawAsync(@"EXEC Employee_Mail_Insert_Update @Employee_Id, @Email_id, @Email_Password, @SMTP_Server, @SMTP_Server_Address, @SMTP_Port, 
                            @Enable_SSL, @recordType", employeeId, emailId, emailPassword, SMTPServer, SMTPServerAddress, SMTPPort, enableSSL, recordType));
            return result;
        }
        public async Task<int> DeleteEmployeeMail(int employeeId)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Employee_Mail_Delete {employeeId}"));
        }
        #endregion
        #endregion
    }
}
