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
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class UserService : IUserService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;        
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public UserService(AstuteDbContext dbContext,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

        }
        #endregion

        #region Utilities
        private async Task Insert_User_Registration_Trace(User_Registration user_Registration, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);
            var encryptPassword = !string.IsNullOrEmpty(user_Registration.Password) ? CoreService.Encrypt(user_Registration.Password) : string.Empty;

            var company_Name = new SqlParameter("@Company_Name", user_Registration.Company_Name);
            var prefix = new SqlParameter("@Prefix", user_Registration.Prefix);
            var first_Name = new SqlParameter("@First_Name", user_Registration.First_Name);
            var last_Name = new SqlParameter("@Last_Name", user_Registration.Last_Name);
            var address = new SqlParameter("@Address", user_Registration.Address);
            var city_Id = new SqlParameter("@City_Id", user_Registration.City_Id);
            var pincode = !string.IsNullOrEmpty(user_Registration.PinCode) ? new SqlParameter("@PinCode", user_Registration.PinCode) : new SqlParameter("@PinCode", DBNull.Value);
            var mobile_1 = !string.IsNullOrEmpty(user_Registration.Mobile_1) ? new SqlParameter("@Mobile_1", user_Registration.Mobile_1) : new SqlParameter("@Mobile_1", DBNull.Value);
            var mobile_2 = !string.IsNullOrEmpty(user_Registration.Mobile_2) ? new SqlParameter("@Mobile_2", user_Registration.Mobile_2) : new SqlParameter("@Mobile_2", DBNull.Value);
            var phone_1 = !string.IsNullOrEmpty(user_Registration.Phone_1) ? new SqlParameter("@Phone_1", user_Registration.Phone_1) : new SqlParameter("@Phone_1", DBNull.Value);
            var phone_2 = !string.IsNullOrEmpty(user_Registration.Phone_2) ? new SqlParameter("@Phone_2", user_Registration.Phone_2) : new SqlParameter("@Phone_2", DBNull.Value);
            var fax = !string.IsNullOrEmpty(user_Registration.Fax) ? new SqlParameter("@Fax", user_Registration.Fax) : new SqlParameter("@Fax", DBNull.Value);
            var email_1 = new SqlParameter("@Email_1", user_Registration.Email_1);
            var email_2 = !string.IsNullOrEmpty(user_Registration.Email_2) ? new SqlParameter("@Email_2", user_Registration.Email_2) : new SqlParameter("@Email_2", DBNull.Value);
            var designation = user_Registration.Designation > 0 ? new SqlParameter("@Designation", user_Registration.Designation) : new SqlParameter("@Designation", DBNull.Value);
            var business_Reg_No = new SqlParameter("@Business_Reg_No", user_Registration.Business_Reg_No);
            var business_Reg_Upload = new SqlParameter("@Business_Reg_Upload", user_Registration.Business_Reg_Upload);
            var photo_Proof_Upload = new SqlParameter("@Photo_Proof_Upload", user_Registration.Photo_Proof_Upload);
            var address_Proof_Upload = new SqlParameter("@Address_Proof_Upload", user_Registration.Address_Proof_Upload);
            var user_Name = new SqlParameter("@User_Name", user_Registration.User_Name);
            var password = new SqlParameter("@Password", encryptPassword);
            var status = user_Registration.User_Id == 0 ? new SqlParameter("@Status", "Pending") : new SqlParameter("@Status", user_Registration.Status);

            await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC User_Registration_Trace_Insert @Company_Name, @Prefix, @First_Name, @Last_Name, @Address, @City_Id, @PinCode, @Mobile_1,
            @Mobile_2, @Phone_1, @Phone_2, @Fax, @Email_1, @Email_2, @Designation, @Business_Reg_No, @Business_Reg_Upload, @Photo_Proof_Upload, @Address_Proof_Upload,
            @User_Name, @Password, @Status, @IsExistUserName OUT", company_Name, prefix, first_Name, last_Name, address, city_Id, pincode, mobile_1, mobile_2,
            phone_1, phone_2, fax, email_1, email_2, designation, business_Reg_No, business_Reg_Upload, photo_Proof_Upload, address_Proof_Upload, user_Name, password, status));

        }
        #endregion

        #region Methods
        public virtual async Task<(string, int)> Add_Update_User(User_Registration user_Registration)
        {
            var encryptPassword = !string.IsNullOrEmpty(user_Registration.Password) ? CoreService.Encrypt(user_Registration.Password) : string.Empty;

            var user_Id = new SqlParameter("@User_Id", user_Registration.User_Id);
            var company_Name = new SqlParameter("@Company_Name", user_Registration.Company_Name);
            var prefix = new SqlParameter("@Prefix", user_Registration.Prefix);
            var first_Name = new SqlParameter("@First_Name", user_Registration.First_Name);
            var last_Name = new SqlParameter("@Last_Name", user_Registration.Last_Name);
            var address = new SqlParameter("@Address", user_Registration.Address);
            var city_Id = new SqlParameter("@City_Id", user_Registration.City_Id);
            var pincode = !string.IsNullOrEmpty(user_Registration.PinCode) ? new SqlParameter("@PinCode", user_Registration.PinCode) : new SqlParameter("@PinCode", DBNull.Value);
            var mobile_1 = !string.IsNullOrEmpty(user_Registration.Mobile_1) ? new SqlParameter("@Mobile_1", user_Registration.Mobile_1) : new SqlParameter("@Mobile_1", DBNull.Value);
            var mobile_2 = !string.IsNullOrEmpty(user_Registration.Mobile_2) ? new SqlParameter("@Mobile_2", user_Registration.Mobile_2) : new SqlParameter("@Mobile_2", DBNull.Value);
            var phone_1 = !string.IsNullOrEmpty(user_Registration.Phone_1) ? new SqlParameter("@Phone_1", user_Registration.Phone_1) : new SqlParameter("@Phone_1", DBNull.Value);
            var phone_2 = !string.IsNullOrEmpty(user_Registration.Phone_2) ? new SqlParameter("@Phone_2", user_Registration.Phone_2) : new SqlParameter("@Phone_2", DBNull.Value);
            var fax = !string.IsNullOrEmpty(user_Registration.Fax) ? new SqlParameter("@Fax", user_Registration.Fax) : new SqlParameter("@Fax", DBNull.Value);
            var email_1 = new SqlParameter("@Email_1", user_Registration.Email_1);
            var email_2 = !string.IsNullOrEmpty(user_Registration.Email_2) ? new SqlParameter("@Email_2", user_Registration.Email_2) : new SqlParameter("@Email_2", DBNull.Value);
            var website = !string.IsNullOrEmpty(user_Registration.Website) ? new SqlParameter("@Website", user_Registration.Website) : new SqlParameter("@Website", DBNull.Value);
            var designation = user_Registration.Designation > 0 ? new SqlParameter("@Designation", user_Registration.Designation) : new SqlParameter("@Designation", DBNull.Value);
            var business_Reg_No = new SqlParameter("@Business_Reg_No", user_Registration.Business_Reg_No);
            var business_Reg_Upload = new SqlParameter("@Business_Reg_Upload", user_Registration.Business_Reg_Upload);
            var photo_Proof_Upload = new SqlParameter("@Photo_Proof_Upload", user_Registration.Photo_Proof_Upload);
            var address_Proof_Upload = new SqlParameter("@Address_Proof_Upload", user_Registration.Address_Proof_Upload);
            var user_Name = new SqlParameter("@User_Name", user_Registration.User_Name);
            var password = new SqlParameter("@Password", encryptPassword);
            var status = user_Registration.User_Id == 0 ? new SqlParameter("@Status", "Pending") : new SqlParameter("@Status", user_Registration.Status);
            var assist_by_1 = user_Registration.Assist_By_1 > 0 ? new SqlParameter("@Assist_By_1", user_Registration.Assist_By_1) : new SqlParameter("@Assist_By_1", DBNull.Value);
            var assist_by_2 = user_Registration.Assist_By_2 > 0 ? new SqlParameter("@Assist_By_2", user_Registration.Assist_By_2) : new SqlParameter("@Assist_By_2", DBNull.Value);

            var isExistUserName = new SqlParameter("@IsExistUserName", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC User_Registration_Insert_Update @User_Id, @Company_Name, @Prefix, @First_Name, @Last_Name, @Address, @City_Id, @PinCode, @Mobile_1,
                        @Mobile_2, @Phone_1, @Phone_2, @Fax, @Email_1, @Email_2, @Website, @Designation, @Business_Reg_No, @Business_Reg_Upload, @Photo_Proof_Upload, @Address_Proof_Upload,
                        @User_Name, @Password, @Status, @Assist_By_1, @Assist_By_2, @IsExistUserName OUT", user_Id, company_Name, prefix, first_Name, last_Name, address, city_Id, pincode, mobile_1, mobile_2,
                        phone_1, phone_2, fax, email_1, email_2, website, designation, business_Reg_No, business_Reg_Upload, photo_Proof_Upload, address_Proof_Upload, user_Name, password, status, assist_by_1, assist_by_2, isExistUserName));

            bool _isExistUserName = (bool)isExistUserName.Value;
            if (_isExistUserName)
                return ("_error_username_exist", 409);

            return ("success", result);
        }
        public virtual async Task<List<Dictionary<string, object>>> Get_User(int user_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("User_Registration_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    var dataTable = ds.Tables[ds.Tables.Count - 1];

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var dict = new Dictionary<string, object>();
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            if (row[col] == DBNull.Value)
                            {
                                dict[col.ColumnName] = null;
                            }
                            if (col.ColumnName == "Business_Reg_Upload" && row[col] != DBNull.Value)
                            {
                                dict[col.ColumnName] = !string.IsNullOrEmpty(row[col].ToString()) ? _configuration["BaseUrl"] + CoreCommonFilePath.CompanyDocumentsPath + row[col].ToString() : null;
                            }
                            else if (col.ColumnName == "Photo_Proof_Upload" && row[col] != DBNull.Value)
                            {
                                dict[col.ColumnName] = !string.IsNullOrEmpty(row[col].ToString()) ? _configuration["BaseUrl"] + CoreCommonFilePath.CompanyDocumentsPath + row[col].ToString() : null;
                            }
                            else if (col.ColumnName == "Address_Proof_Upload" && row[col] != DBNull.Value)
                            {
                                dict[col.ColumnName] = !string.IsNullOrEmpty(row[col].ToString()) ? _configuration["BaseUrl"] + CoreCommonFilePath.CompanyDocumentsPath + row[col].ToString() : null;
                            }                            
                            else if(col.ColumnName == "Password" && row[col] != DBNull.Value)
                            {
                                dict[col.ColumnName] = CoreService.Decrypt(row[col].ToString());
                            }
                            else
                            {
                                dict[col.ColumnName] = row[col];
                            }
                        }
                        result.Add(dict);
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
