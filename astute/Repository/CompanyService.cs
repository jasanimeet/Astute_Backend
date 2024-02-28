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
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class CompanyService : ICompanyService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public CompanyService(AstuteDbContext dbContext,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Utilities
        private async Task Insert_Company_Trace(Company_Master company_Master, string recordType)
        {
            var companyName = new SqlParameter("@Company_Name", company_Master.Company_Name);
            var address1 = new SqlParameter("@Address_1", company_Master.Address_1);
            var address2 = !string.IsNullOrEmpty(company_Master.Address_2) ? new SqlParameter("@Address_2", company_Master.Address_2) : new SqlParameter("@Address_2", DBNull.Value);
            var address3 = !string.IsNullOrEmpty(company_Master.Address_3) ? new SqlParameter("@Address_3", company_Master.Address_3) : new SqlParameter("@Address_3", DBNull.Value);
            var cityId = new SqlParameter("@City_Id", company_Master.City_Id);
            var phoneNo = new SqlParameter("@Phone_No", company_Master.Phone_No);
            var faxNo = new SqlParameter("@Fax_No", company_Master.Fax_No);
            var email = new SqlParameter("@Email", company_Master.Email);
            var website = !string.IsNullOrEmpty(company_Master.Website) ? new SqlParameter("@Website", company_Master.Website) : new SqlParameter("@Website", DBNull.Value);
            var orderNo = company_Master.Order_No > 0 ? new SqlParameter("@Order_No", company_Master.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var sortNo = company_Master.Sort_No > 0 ? new SqlParameter("@Sort_No", company_Master.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var status = new SqlParameter("@Status", company_Master.Status);

            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Company_Master_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @RecordType, @Company_Name, @Address_1, @Address_2, @Address_3, @City_Id, @Phone_No, @Fax_No,
            @Email, @Website, @Order_No, @Sort_No, @Status", empId, ipaddress, date, time, record_Type, companyName, address1, address2, address3, cityId, phoneNo, faxNo, email, website,
            orderNo, sortNo, status, recordType));
        }
        public async Task Insert_Company_Document_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Struct_Company_Document_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Company_Document_Trace_Data_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Company_Document_Trace_Insert @Struct_Company_Document_Trace", parameter);
        }
        public async Task Insert_Company_Media_Trace(DataTable dataTable)
        {   
            var parameter = new SqlParameter("@Struct_Company_Media_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Company_Media_Trace_Data_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Company_Media_Trace_Insert @Struct_Company_Media_Trace", parameter);
        }
        public async Task Insert_Company_Bank_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Struct_Company_Bank_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Company_Bank_Trace_Data_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Company_Bank_Trace_Insert @Struct_Company_Bank_Trace", parameter);
        }
        #endregion

        #region Methods
        #region Company Master
        public async Task<(string, int)> AddUpdateCompany(Company_Master company_Master)
        {
            var companyId = new SqlParameter("@Company_Id", company_Master.Company_Id);
            var companyName = new SqlParameter("@Company_Name", company_Master.Company_Name);
            var address1 = new SqlParameter("@Address_1", company_Master.Address_1);
            var address2 = !string.IsNullOrEmpty(company_Master.Address_2) ? new SqlParameter("@Address_2", company_Master.Address_2) : new SqlParameter("@Address_2", DBNull.Value);
            var address3 = !string.IsNullOrEmpty(company_Master.Address_3) ? new SqlParameter("@Address_3", company_Master.Address_3) : new SqlParameter("@Address_3", DBNull.Value);
            var cityId = new SqlParameter("@City_Id", company_Master.City_Id);
            var phoneNo = new SqlParameter("@Phone_No", company_Master.Phone_No);
            var faxNo = !string.IsNullOrEmpty(company_Master.Fax_No) ? new SqlParameter("@Fax_No", company_Master.Fax_No) : new SqlParameter("@Fax_No", DBNull.Value);
            var email = new SqlParameter("@Email", company_Master.Email);
            var website = !string.IsNullOrEmpty(company_Master.Website) ? new SqlParameter("@Website", company_Master.Website) : new SqlParameter("@Website", DBNull.Value);
            var orderNo = company_Master.Order_No > 0 ? new SqlParameter("@Order_No", company_Master.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var sortNo = company_Master.Sort_No > 0 ? new SqlParameter("@Sort_No", company_Master.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var status = new SqlParameter("@Status", company_Master.Status);

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
           .ExecuteSqlRawAsync(@"exec Company_Master_Insert_Update @Company_Id, @Company_Name, @Address_1, @Address_2, @Address_3, @City_Id, @Phone_No, @Fax_No,
            @Email, @Website, @Order_No, @Sort_No, @Status, @IsExistOrderNo OUT, @IsExistSortNo OUT, @InsertedId OUT", companyId, companyName, address1, address2, address3, cityId, phoneNo, faxNo, email, website,
           orderNo, sortNo, status, isExistOrderNo, isExistSortNo, insertedId));

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return ("_error_order_no", 0);

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return ("_error_sort_no", 0);

            if (result > 0)
            {
                string record_Type = string.Empty;
                int _insertedId = (int)insertedId.Value;
                //if (CoreService.Enable_Trace_Records(_configuration))
                //{
                //    if (company_Master.Company_Id == 0)
                //        record_Type = "Insert";
                //    else
                //        record_Type = "Update";
                //    await Insert_Company_Trace(company_Master, record_Type);
                //}
                return ("success", _insertedId);
            }
            return ("error", 0);
        }
        public async Task<Company_Master> Get_Company_Details_By_Id(int company_Id)
        {
            var _companyId = company_Id > 0 ? new SqlParameter("@CompanyId", company_Id) : new SqlParameter("@CompanyId", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Company_Master
                            .FromSqlRaw(@"exec Company_Master_Select @CompanyId", _companyId)
                            .AsEnumerable()
                            .FirstOrDefault());

            if (result != null)
            {
                if (company_Id > 0)
                {
                    var _company_Media_Id = new SqlParameter("@Company_Media_Id", DBNull.Value);
                    var _company_Id = company_Id > 0 ? new SqlParameter("@Company_Id", company_Id) : new SqlParameter("@Company_Id", DBNull.Value);
                    result.Company_Document_List = await Task.Run(() => _dbContext.Company_Document
                                                    .FromSqlRaw(@"exec Company_Document_Select @Company_Media_Id, @Company_Id", _company_Media_Id, _company_Id).ToListAsync());
                    if (result.Company_Document_List != null && result.Company_Document_List.Count > 0)
                    {
                        foreach (var item in result.Company_Document_List)
                        {
                            item.Upload_Path = !string.IsNullOrEmpty(item.Upload_Path) ? _configuration["BaseUrl"] + CoreCommonFilePath.CompanyDocumentsPath + item.Upload_Path : item.Upload_Path;
                            item.Upload_Path_1 = !string.IsNullOrEmpty(item.Upload_Path_1) ? _configuration["BaseUrl"] + CoreCommonFilePath.CompanyDocumentsPath + item.Upload_Path_1 : item.Upload_Path_1;
                            item.Upload_Path_2 = !string.IsNullOrEmpty(item.Upload_Path_2) ? _configuration["BaseUrl"] + CoreCommonFilePath.CompanyDocumentsPath + item.Upload_Path_2 : item.Upload_Path_2;
                            item.Upload_Path_3 = !string.IsNullOrEmpty(item.Upload_Path_3) ? _configuration["BaseUrl"] + CoreCommonFilePath.CompanyDocumentsPath + item.Upload_Path_3 : item.Upload_Path_3;
                        }
                    }

                    result.Company_Media_List = await Task.Run(() => _dbContext.Company_Media
                                                .FromSqlRaw(@"exec Company_Media_Select @Company_Media_Id, @Company_Id", _company_Media_Id, _company_Id).ToListAsync());

                    result.Company_Bank_List = await Task.Run(() => _dbContext.Company_Bank
                                                .FromSqlRaw(@"exec Company_Bank_Select @Company_Media_Id, @Company_Id", _company_Media_Id, _company_Id).ToListAsync());
                }
            }
            return result;
        }
        public async Task<int> DeleteCompany(int companyId)
        {
            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    var _companyId = companyId > 0 ? new SqlParameter("@CompanyId", companyId) : new SqlParameter("@CompanyId", DBNull.Value);

            //    var result = await Task.Run(() => _dbContext.Company_Master
            //                    .FromSqlRaw(@"exec Company_Master_Select @CompanyId", _companyId)
            //                    .AsEnumerable()
            //                    .FirstOrDefault());
            //    if (result != null)
            //    {
            //        await Insert_Company_Trace(result, "Delete");
            //    }
            //}
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Company_Master_Delete {companyId}"));
        }
        public async Task<IList<Company_Master>> GetCompany(int companyId)
        {
            var CompanyId = companyId > 0 ? new SqlParameter("@companyId", companyId) : new SqlParameter("@companyId", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Company_Master
                            .FromSqlRaw(@"exec Company_Master_Select @companyId", CompanyId).ToListAsync());

            return result;
        }
        public async Task<int> CompanyChangeStatus(int company_Id, bool status)
        {
            var companyId = new SqlParameter("@Company_Id", company_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Company_Master_Update_Status @Company_Id, @Status", companyId, Status));
            return result;
        }
        public async Task<IList<DropdownModel>> Get_Active_Company()
        {
            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Company_Master_Active_Select").ToListAsync());

            return result;
        }
        public async Task<int> Get_Company_Max_Order_No()
        {
            var result = await _dbContext.Company_Master.Select(x => x.Order_No).MaxAsync();
            if (result > 0)
            {
                var maxValue = checked((int)result + 1);
                return maxValue;
            }
            return 1;
        }
        #endregion

        #region Company Document
        public async Task<int> InsertCompanyDocument(DataTable dataTable)
        {
            var parameter = new SqlParameter("@company_Document", SqlDbType.Structured)
            {
                TypeName = "dbo.Company_Document_Data_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Company_Document_Insert_Update @company_Document", parameter);
            
            return result;
        }
        #endregion

        #region Company Media
        public async Task<int> InsertCompanyMedia(DataTable dataTable)
        {
            var parameter = new SqlParameter("@company_Media", SqlDbType.Structured)
            {
                TypeName = "dbo.Company_Media_Data_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Company_Media_Insert_Update @company_Media", parameter);

            return result;
        }
        #endregion

        #region Company Bank        
        public async Task<int> InsertCompanyBank(DataTable dataTable)
        {
            var parameter = new SqlParameter("@company_Bank", SqlDbType.Structured)
            {
                TypeName = "dbo.Company_Bank_Data_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Company_Bank_Insert_Update @company_Bank", parameter);
            return result;
        }
        #endregion
        #endregion
    }
}
