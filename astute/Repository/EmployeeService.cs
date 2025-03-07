using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static astute.Models.Employee_Master;

namespace astute.Repository
{
    public partial class EmployeeService : IEmployeeService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IJWTAuthentication _jWTAuthentication;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public EmployeeService(AstuteDbContext dbContext,
            IConfiguration configuration,
            IJWTAuthentication jWTAuthentication,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _jWTAuthentication = jWTAuthentication;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Utilities
        private async Task Insert_Employee_Trace(Employee_Master employee_Master, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var encryptPassword = !string.IsNullOrEmpty(employee_Master.Password) ? CoreService.Encrypt(employee_Master.Password) : string.Empty;

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
            var userName = !string.IsNullOrEmpty(employee_Master.User_Name) ? new SqlParameter("@userName", employee_Master.User_Name) : new SqlParameter("@userName", DBNull.Value);
            var password = !string.IsNullOrEmpty(employee_Master.Password) ? new SqlParameter("@password", encryptPassword) : new SqlParameter("@password", DBNull.Value);
            var employeeCode = !string.IsNullOrEmpty(employee_Master.Employee_Code) ? new SqlParameter("@employee_Code", employee_Master.Employee_Code) : new SqlParameter("@employee_Code", DBNull.Value);
            var status = new SqlParameter("@status", employee_Master.Status);
            var marital_Status = !string.IsNullOrEmpty(employee_Master.Marital_Status) ? new SqlParameter("@marital_Status", employee_Master.Marital_Status) : new SqlParameter("@marital_Status", DBNull.Value);
            var mobile_Country_Code = !string.IsNullOrEmpty(employee_Master.Mobile_Country_Code) ? new SqlParameter("@mobile_Country_Code", employee_Master.Mobile_Country_Code) : new SqlParameter("@mobile_Country_Code", DBNull.Value);
            var mobile_1_Country_Code = !string.IsNullOrEmpty(employee_Master.Mobile_1_Country_Code) ? new SqlParameter("@mobile_1_Country_Code", employee_Master.Mobile_1_Country_Code) : new SqlParameter("@mobile_1_Country_Code", DBNull.Value);
            var probation_End_Date = !string.IsNullOrEmpty(employee_Master.Probation_End_Date) ? new SqlParameter("@probation_End_Date", employee_Master.Probation_End_Date) : new SqlParameter("@probation_End_Date", DBNull.Value);
            var personal_Mobile_No = !string.IsNullOrEmpty(employee_Master.Personal_Mobile_No) ? new SqlParameter("@personal_Mobile_No", employee_Master.Personal_Mobile_No) : new SqlParameter("@personal_Mobile_No", DBNull.Value);

            await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Employee_Master_Trace_Insert @Employee_Id, @IP_Address,@Trace_Date, @Trace_Time, @Record_Type, @employeeId, @initial, @firstName, @middleName, @lastName,
            @chineseName, @address1, @address2, @address3, @cityId, @joindate, @employeeType, @birthDate, @gender, @mobileNo, @personalEmail, @companyEmail,
            @leaveDate, @pSNID, @bloodGroup, @contractStartDate, @contractEndDate, @approveHolidays, @orderNo, @sortNo, @userName, @password, @employee_Code, @status,
            @marital_Status, @mobile_Country_Code, @mobile_1_Country_Code, @probation_End_Date, @personal_Mobile_No",
            empId, ipaddress, date, time, record_Type, employeeId, initial, firstName, middleName, lastName, chineseName, address1, address2, address3, cityId, joinDate, employeeType,
            birthDate, gender, mobileNo, personalEmail, companyEmail, leaveDate, pSNID, bloodGroup, contractStartDate, contractEndDate, approveHolidays, orderNo, sortNo, userName, password,
            employeeCode, status, marital_Status, mobile_Country_Code, mobile_1_Country_Code, probation_End_Date, personal_Mobile_No));
        }
        public async Task Insert_Employee_Document_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblEmployee_Document_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Employee_Document_Trace_Table_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Employee_Document_Trace_Insert @tblEmployee_Document_Trace", parameter);
        }
        public async Task Insert_Employee_Salary_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblEmployee_Salary_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Employee_Salary_Trace_Data_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Employee_Salary_Trace_Insert @tblEmployee_Salary_Trace", parameter);
        }
        public async Task Insert_Emergency_Contact_Detail_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblEmergency_Contact_Detail_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Emergency_Contact_Detail_Trace_Table_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Emergency_Contact_Detail_Trace_Insert @tblEmergency_Contact_Detail_Trace", parameter);
        }
        #endregion

        #region Methods
        #region Employee Master
        public async Task<(string, int)> AddUpdateEmployee(Employee_Master employee_Master)
        {
            var encryptPassword = !string.IsNullOrEmpty(employee_Master.Password) ? CoreService.Encrypt(employee_Master.Password) : string.Empty;

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
            var userName = !string.IsNullOrEmpty(employee_Master.User_Name) ? new SqlParameter("@userName", employee_Master.User_Name) : new SqlParameter("@userName", DBNull.Value);
            var password = !string.IsNullOrEmpty(employee_Master.Password) ? new SqlParameter("@password", encryptPassword) : new SqlParameter("@password", DBNull.Value);
            var employeeCode = !string.IsNullOrEmpty(employee_Master.Employee_Code) ? new SqlParameter("@employee_Code", employee_Master.Employee_Code) : new SqlParameter("@employee_Code", DBNull.Value);
            var status = new SqlParameter("@status", employee_Master.Status);
            var is_admin = new SqlParameter("@is_admin", employee_Master.Is_Admin);
            var marital_Status = !string.IsNullOrEmpty(employee_Master.Marital_Status) ? new SqlParameter("@marital_Status", employee_Master.Marital_Status) : new SqlParameter("@marital_Status", DBNull.Value);
            var mobile_Country_Code = !string.IsNullOrEmpty(employee_Master.Mobile_Country_Code) ? new SqlParameter("@mobile_Country_Code", employee_Master.Mobile_Country_Code) : new SqlParameter("@mobile_Country_Code", DBNull.Value);
            var mobile_1_Country_Code = !string.IsNullOrEmpty(employee_Master.Mobile_1_Country_Code) ? new SqlParameter("@mobile_1_Country_Code", employee_Master.Mobile_1_Country_Code) : new SqlParameter("@mobile_1_Country_Code", DBNull.Value);
            var probation_End_Date = !string.IsNullOrEmpty(employee_Master.Probation_End_Date) ? new SqlParameter("@probation_End_Date", employee_Master.Probation_End_Date) : new SqlParameter("@probation_End_Date", DBNull.Value);
            var personal_Mobile_No = !string.IsNullOrEmpty(employee_Master.Personal_Mobile_No) ? new SqlParameter("@personal_Mobile_No", employee_Master.Personal_Mobile_No) : new SqlParameter("@personal_Mobile_No", DBNull.Value);
            var designation_Id = employee_Master.Designation_Id > 0 ? new SqlParameter("@designation_Id", employee_Master.Designation_Id) : new SqlParameter("@designation_Id", DBNull.Value);
            var user_Type = !string.IsNullOrEmpty(employee_Master.User_Type) ? new SqlParameter("@User_Type", employee_Master.User_Type) : new SqlParameter("@User_Type", DBNull.Value);
            var is_Secretary = new SqlParameter("@Is_Secretary", employee_Master.Is_Secretary);

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
            @leaveDate, @pSNID, @bloodGroup, @contractStartDate, @contractEndDate, @approveHolidays, @orderNo, @sortNo, @userName, @password, @employee_Code, @status, @is_admin,
            @marital_Status, @mobile_Country_Code, @mobile_1_Country_Code, @probation_End_Date, @personal_Mobile_No, @designation_Id, @User_Type, @Is_Secretary, @IsExistUserName OUT, @IsExistOrderNo OUT, @IsExistSortNo OUT, @InsertedId OUT",
            employeeId, initial, firstName, middleName, lastName, chineseName, address1, address2, address3, cityId, joinDate, employeeType, birthDate, gender, mobileNo,
            personalEmail, companyEmail, leaveDate, pSNID, bloodGroup, contractStartDate, contractEndDate, approveHolidays, orderNo, sortNo, userName, password,
            employeeCode, status, is_admin, marital_Status, mobile_Country_Code, mobile_1_Country_Code, probation_End_Date, personal_Mobile_No, designation_Id, user_Type, is_Secretary, isExistUserName, isExistOrderNo, isExistSortNo, insertedId));

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
                //if (CoreService.Enable_Trace_Records(_configuration))
                //{
                //    if (employee_Master.Employee_Id > 0)
                //        await Insert_Employee_Trace(employee_Master, "Update");
                //    else
                //    {
                //        employee_Master.Employee_Id = _insertedId;
                //        await Insert_Employee_Trace(employee_Master, "Insert");
                //    }
                //}
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
                var password = !string.IsNullOrEmpty(employee_Master.Password) ? CoreService.Decrypt(employee_Master.Password) : string.Empty;
                employee_Master.Password = password;
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
                            item.Document_Url_2 = !string.IsNullOrEmpty(item.Document_Url_2) ? _configuration["BaseUrl"] + CoreCommonFilePath.EmployeeDocumentsPath + item.Document_Url_2 : item.Document_Url_2;
                            item.Document_Url_3 = !string.IsNullOrEmpty(item.Document_Url_3) ? _configuration["BaseUrl"] + CoreCommonFilePath.EmployeeDocumentsPath + item.Document_Url_3 : item.Document_Url_3;
                            item.Document_Url_4 = !string.IsNullOrEmpty(item.Document_Url_4) ? _configuration["BaseUrl"] + CoreCommonFilePath.EmployeeDocumentsPath + item.Document_Url_4 : item.Document_Url_4;
                        }
                    }

                    employee_Master.Employee_Salary_List = await Task.Run(() => _dbContext.Employee_Salary
                                                            .FromSqlRaw(@"exec Employee_Salary_Select @employeeId", _emp_Id).ToListAsync());

                    employee_Master.Emergency_Contact_Detail_List = await Task.Run(() => _dbContext.Emergency_Contact_Detail
                                                                    .FromSqlRaw(@"exec Emergency_Contact_Detail_Select @Emergency_Contact_Detail_Id, @employeeId", new SqlParameter("@Emergency_Contact_Detail_Id", DBNull.Value), _emp_Id).ToListAsync());

                    employee_Master.Employee_Secretary_List = await Task.Run(() => _dbContext.Employee_Secretary
                                                                    .FromSqlRaw(@"exec Employee_Secretary_Select @employeeId", _emp_Id).ToListAsync());
                }
            }

            return employee_Master;
        }
        public async Task<int> UpdateEmployee(Employee_Master employee_Master)
        {
            var encryptPassword = !string.IsNullOrEmpty(employee_Master.Password) ? CoreService.Encrypt(employee_Master.Password) : string.Empty;

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
            var userName = !string.IsNullOrEmpty(employee_Master.User_Name) ? new SqlParameter("@userName", employee_Master.User_Name) : new SqlParameter("@userName", DBNull.Value);
            var password = !string.IsNullOrEmpty(employee_Master.Password) ? new SqlParameter("@password", encryptPassword) : new SqlParameter("@password", DBNull.Value);
            var employeeCode = !string.IsNullOrEmpty(employee_Master.Employee_Code) ? new SqlParameter("@employee_Code", employee_Master.Employee_Code) : new SqlParameter("@employee_Code", DBNull.Value);
            var status = new SqlParameter("@status", employee_Master.Status);
            var marital_Status = !string.IsNullOrEmpty(employee_Master.Marital_Status) ? new SqlParameter("@marital_Status", employee_Master.Marital_Status) : new SqlParameter("@marital_Status", DBNull.Value);
            var mobile_Country_Code = !string.IsNullOrEmpty(employee_Master.Mobile_Country_Code) ? new SqlParameter("@mobile_Country_Code", employee_Master.Mobile_Country_Code) : new SqlParameter("@mobile_Country_Code", DBNull.Value);
            var mobile_1_Country_Code = !string.IsNullOrEmpty(employee_Master.Mobile_1_Country_Code) ? new SqlParameter("@mobile_1_Country_Code", employee_Master.Mobile_1_Country_Code) : new SqlParameter("@mobile_1_Country_Code", DBNull.Value);
            var probation_End_Date = !string.IsNullOrEmpty(employee_Master.Probation_End_Date) ? new SqlParameter("@probation_End_Date", employee_Master.Probation_End_Date) : new SqlParameter("@probation_End_Date", DBNull.Value);
            var personal_Mobile_No = !string.IsNullOrEmpty(employee_Master.Personal_Mobile_No) ? new SqlParameter("@personal_Mobile_No", employee_Master.Personal_Mobile_No) : new SqlParameter("@personal_Mobile_No", DBNull.Value);
            var designation_Id = employee_Master.Designation_Id > 0 ? new SqlParameter("@designation_Id", employee_Master.Designation_Id) : new SqlParameter("@designation_Id", DBNull.Value);

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
            @leaveDate, @pSNID, @bloodGroup, @contractStartDate, @contractEndDate, @approveHolidays, @orderNo, @sortNo, @userName, @password, @employee_Code, @status,
            @marital_Status, @mobile_Country_Code, @mobile_1_Country_Code, @probation_End_Date, @personal_Mobile_No, @designation_Id, @IsExistUserName OUT, @IsExistOrderNo OUT, @IsExistSortNo OUT, @InsertedId OUT",
            employeeId, initial, firstName, middleName, lastName, chineseName, address1, address2, address3, cityId, joinDate, employeeType, birthDate, gender, mobileNo,
            personalEmail, companyEmail, leaveDate, pSNID, bloodGroup, contractStartDate, contractEndDate, approveHolidays, orderNo, sortNo, userName, password,
            employeeCode, status, marital_Status, mobile_Country_Code, mobile_1_Country_Code, probation_End_Date, personal_Mobile_No, designation_Id, isExistUserName, isExistOrderNo, isExistSortNo, insertedId));

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

            return employees;
        }

        public async Task<Employee_Fortune_Master> GetEmployeeFortuneId(int employeeId)
        {
            var empId = employeeId > 0 ? new SqlParameter("@employeeId", employeeId) : new SqlParameter("@employeeId", DBNull.Value);

            var employees = await Task.Run(() => _dbContext.Employee_Fortune_Master
                            .FromSqlRaw(@"exec Employee_Master_Fortune_Id_Select @employeeId", empId)
                            .AsEnumerable()
                            .FirstOrDefault());

            return employees;
        }

        public async Task<Employee_Fortune_Order_Master> GetEmployeeFortuneIdByOrderNo(string order_No)
        {
            var OrderNo = !string.IsNullOrEmpty(order_No) ? new SqlParameter("@Order_No", order_No) : new SqlParameter("@Order_No", DBNull.Value);

            var employees = await Task.Run(() => _dbContext.Employee_Fortune_Order_Master
                            .FromSqlRaw(@"exec Employee_Master_Fortune_Id_Order_No_Select @Order_No", OrderNo)
                            .AsEnumerable()  
                            .FirstOrDefault());

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
        public async Task<int> Get_Employee_Code()
        {
            var employeeCodeParam = new SqlParameter("@EmployeeCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            await _dbContext.Database.ExecuteSqlRawAsync("exec Employee_Code_Select @EmployeeCode OUTPUT", employeeCodeParam);

            if (int.TryParse(employeeCodeParam.Value?.ToString(), out int employeeCode))
            {
                return employeeCode;
            }
            else
            {
                return 0;
            }
        }
        public async Task<IList<Employee_Master>> Get_Active_Employees(int employeeId, string userName, string personalEmail)
        {
            var empId = employeeId > 0 ? new SqlParameter("@employeeId", employeeId) : new SqlParameter("@employeeId", DBNull.Value);
            var uName = !string.IsNullOrEmpty(userName) ? new SqlParameter("@userName", userName) : new SqlParameter("@userName", DBNull.Value);
            var email = !string.IsNullOrEmpty(personalEmail) ? new SqlParameter("@email", personalEmail) : new SqlParameter("@email", DBNull.Value);

            var employees = await Task.Run(() => _dbContext.Employee_Master
                            .FromSqlRaw(@"exec Employee_Master_Active_Select @employeeId, @userName, @email", empId, uName, email).ToListAsync());

            return employees;
        }
        public async Task<IList<Employee_Master>> Get_Active_Secretary_Employees(int user_Id)
        {
            var _user_Id = user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);
            
            var employees = await Task.Run(() => _dbContext.Employee_Master.FromSqlRaw(@"exec Employee_Master_Secretary_Active_Select @User_Id", _user_Id).ToListAsync());

            return employees;
        }
        public async Task<IList<DropdownModel>> Get_Employee_For_Report(bool is_Exist, int rm_Id, int user_Id, string user_Type)
        {
            var _is_Exist = new SqlParameter("@Is_Exist", is_Exist);
            var _rm_Id = rm_Id > 0 ? new SqlParameter("@Rm_Id", rm_Id) : new SqlParameter("@Rm_Id", DBNull.Value);
            var _user_Id = user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var _user_Type = !string.IsNullOrEmpty(user_Type) ? new SqlParameter("@User_Type", user_Type) : new SqlParameter("@User_Type", DBNull.Value);
            var employees = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Employee_Master_Select_For_Report @Is_Exist, @Rm_Id, @User_Id, @User_Type", _is_Exist, _rm_Id, _user_Id, _user_Type).ToListAsync());

            return employees;
        }
        public async Task<int> Employee_Master_Change_Status(int employee_Id, bool status)
        {
            var _employee_Id = new SqlParameter("@Employee_Id", employee_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Employee_Master_Update_Status @Employee_Id, @Status", _employee_Id, Status));
            return result;
        }
        public async Task<List<DropdownModel>> Employee_Master_Name_Select(int user_Id)
        {
            var _user_Id = user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);

            var employees = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Employee_Master_Select_By_Id @User_Id", _user_Id).ToList());

            return employees;
        }

        public async Task<(string, int)> Change_Password(Change_Password_Model change_Password_Model, int? user_Id)
        {
            var encrypt_Password = CoreService.Encrypt(change_Password_Model.OldPassword);
            var new_encrypt_Password = CoreService.Encrypt(change_Password_Model.NewPassword);
            var _employee_Id = user_Id > 0 ? new SqlParameter("@Employee_Id", user_Id) : new SqlParameter("@Employee_Id", DBNull.Value);
            var _password = !string.IsNullOrEmpty(encrypt_Password) ? new SqlParameter("@Old_Password", encrypt_Password) : new SqlParameter("@Old_Password", DBNull.Value);
            var _new_password = !string.IsNullOrEmpty(new_encrypt_Password) ? new SqlParameter("@New_Password", new_encrypt_Password) : new SqlParameter("@New_Password", DBNull.Value);
            var _isPassNM = new SqlParameter("@IsPassNM", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Employee_Master_Change_Password @Employee_Id, @Old_Password, @New_Password, @IsPassNM OUT ", _employee_Id, _password, _new_password, _isPassNM));

            if ((int)_isPassNM.Value == 1)
            {
                return ("exist", 409);
            }
            return ("success", result);
        }
        
        public async Task<int> Update_FCMToken(UserModel userModel, int Id)
        {
            var _employee_Id = Id > 0 ? new SqlParameter("@Employee_Id", Id) : new SqlParameter("@Employee_Id", DBNull.Value);
            var _FCMToken_ANDROID = !string.IsNullOrEmpty(userModel.FCMToken_ANDROID) ? new SqlParameter("@FCMToken_ANDROID", userModel.FCMToken_ANDROID) : new SqlParameter("@FCMToken_ANDROID", DBNull.Value);
            var _FCMToken_IPHONE = !string.IsNullOrEmpty(userModel.FCMToken_IPHONE) ? new SqlParameter("@FCMToken_IPHONE", userModel.FCMToken_IPHONE) : new SqlParameter("@FCMToken_IPHONE", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Employee_Master_Update_FCMToken @Employee_Id, @FCMToken_ANDROID, @FCMToken_IPHONE ", _employee_Id, _FCMToken_ANDROID, _FCMToken_IPHONE));

            return result;
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
        public async Task<Employee_Mail> Get_Employee_Email_Details(int user_Id)
        {
            string password = "";
            var _user_Id = user_Id > 0 ? new SqlParameter("@Employee_Id", user_Id) : new SqlParameter("@Employee_Id", DBNull.Value);
            Employee_Mail emp = new Employee_Mail();
            var result = await Task.Run(() => _dbContext.Employee_Mail
                                .FromSqlRaw(@"EXEC Employee_Mail_Select @Employee_Id", _user_Id)
                                .ToListAsync());

            if (result != null && result.Count > 0)
            {
                emp = result.FirstOrDefault();
                if (emp != null)
                {
                    password = CoreService.Decrypt(emp.Email_Password);
                }
            }
            emp.Email_Password = password;
            return emp;
        }
        public async Task<Employee_Mail> Get_Employee_Email_Or_Default_Email(int user_Id)
        {
            string password = "";
            var _user_Id = user_Id > 0 ? new SqlParameter("@Employee_Id", user_Id) : new SqlParameter("@Employee_Id", DBNull.Value);
            Employee_Mail emp = new Employee_Mail();
            var result = await Task.Run(() => _dbContext.Employee_Mail
                                .FromSqlRaw(@"EXEC Employee_Mail_Default_Select @Employee_Id", _user_Id)
                                .ToListAsync());

            if (result != null && result.Count > 0)
            {
                emp = result.FirstOrDefault();
                if (emp != null)
                {
                    password = CoreService.Decrypt(emp.Email_Password);
                }
            }
            emp.Email_Password = password;
            return emp;
        }
        #endregion

        #region Emergency Contact Detail
        public async Task<int> Insert_Emergency_Contact_Detail(DataTable dataTable)
        {
            var parameter = new SqlParameter("@emergency_Contact_Detail", SqlDbType.Structured)
            {
                TypeName = "dbo.Emergency_Contact_Detail_Table_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Emergency_Contact_Detail_Insert_Update @emergency_Contact_Detail", parameter);
            return result;
        }
        #endregion

        #region Buyer List
        public async Task<IList<DropdownModel>> Get_Buyer()
        {
            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Get_Employee_User_Type_Buyer")
                            .ToListAsync());
            return result;
        }
        public async Task<IList<DropdownModel>> Get_Buyer_List()
        {
            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Get_Employee_User_Buyer")
                            .ToListAsync());
            return result;
        }

        #endregion

        #region Secretary List
        public async Task<IList<DropdownModel>> Get_Secretary()
        {
            var employees = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Employee_Master_Secretary_Select").ToList());

            return employees;
        }

        #endregion

        #region Employee Secretary
        public async Task<int> Insert_Update_Delete_Employee_Secretary(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Employee_Secretary", SqlDbType.Structured)
            {
                TypeName = "dbo.Employee_Secretary_Table_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Employee_Secretary_Insert_Update @Employee_Secretary", parameter);
            return result;
        }
        #endregion

        #endregion
    }
}
