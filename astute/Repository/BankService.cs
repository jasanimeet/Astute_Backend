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
    public partial class BankService : IBankService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public BankService(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion

        #region Utilities
        private async Task Insert_Bank_Trace(Bank_Master bank_Mas, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var bankName = !string.IsNullOrEmpty(bank_Mas.Bank_Name) ? new SqlParameter("@Bank_Name", bank_Mas.Bank_Name) : new SqlParameter("@Bank_Name", DBNull.Value);
            var branchName = !string.IsNullOrEmpty(bank_Mas.Branch_Name) ? new SqlParameter("@Branch_Name", bank_Mas.Branch_Name) : new SqlParameter("@Branch_Name", DBNull.Value);
            var branchAddress = !string.IsNullOrEmpty(bank_Mas.Branch_Address) ? new SqlParameter("@Branch_Address", bank_Mas.Branch_Address) : new SqlParameter("@Branch_Address", DBNull.Value);
            var ifscCode = !string.IsNullOrEmpty(bank_Mas.Ifsc_Code) ? new SqlParameter("@Ifsc_Code", bank_Mas.Ifsc_Code) : new SqlParameter("@Ifsc_Code", DBNull.Value);
            var correspondentBank = !string.IsNullOrEmpty(bank_Mas.Correspondent_Bank) ? new SqlParameter("@Correspondent_Bank", bank_Mas.Correspondent_Bank) : new SqlParameter("@Correspondent_Bank", DBNull.Value);
            var correspondentBankAddres = !string.IsNullOrEmpty(bank_Mas.Correspondent_Bank_Addres) ? new SqlParameter("@Correspondent_Bank_Addres", bank_Mas.Correspondent_Bank_Addres) : new SqlParameter("@Correspondent_Bank_Addres", DBNull.Value);
            var correspondentIfscCode = !string.IsNullOrEmpty(bank_Mas.Correspondent_Ifsc_Code) ? new SqlParameter("@Correspondent_Ifsc_Code", bank_Mas.Correspondent_Ifsc_Code) : new SqlParameter("@Correspondent_Ifsc_Code", DBNull.Value);
            var orderNo = bank_Mas.Order_No > 0 ? new SqlParameter("@Order_No", bank_Mas.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var sortNo = bank_Mas.Sort_No > 0 ? new SqlParameter("@Sort_No", bank_Mas.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var status = new SqlParameter("@Status", bank_Mas.Status);
            var bankCode = !string.IsNullOrEmpty(bank_Mas.Bank_Code) ? new SqlParameter("@Bank_Code", bank_Mas.Bank_Code) : new SqlParameter("@Bank_Code", DBNull.Value);
            var branchCode = !string.IsNullOrEmpty(bank_Mas.Branch_Code) ? new SqlParameter("@Branch_Code", bank_Mas.Branch_Code) : new SqlParameter("@Branch_Code", DBNull.Value);
            var accountNo = !string.IsNullOrEmpty(bank_Mas.Account_No) ? new SqlParameter("@Account_No", bank_Mas.Account_No) : new SqlParameter("@Account_No", DBNull.Value);
            var correspondentAccountNo = !string.IsNullOrEmpty(bank_Mas.Correspondent_Bank_Account_No) ? new SqlParameter("@Correspondent_Bank_Account_No", bank_Mas.Correspondent_Bank_Account_No) : new SqlParameter("@Correspondent_Bank_Account_No", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Bank_Master_Trace_Insert @Employee_Id, @IP_Address,@Trace_Date, @Trace_Time, @Record_Type, @Bank_Name, @Branch_Name, @Branch_Address, @Ifsc_Code, @Correspondent_Bank, 
                                @Correspondent_Bank_Addres, @Correspondent_Ifsc_Code, @Order_No, @Sort_No, @Status, @Bank_Code, @Branch_Code, @Account_No, @Correspondent_Bank_Account_No",
                                empId, ipaddress, date, time, record_Type, bankName, branchName, branchAddress, ifscCode, correspondentBank, correspondentBankAddres, correspondentIfscCode, orderNo, sortNo, status, bankCode, branchCode,
                                accountNo, correspondentAccountNo));
        }
        #endregion

        #region Methods
        public async Task<int> InsertBank(Bank_Master bank_Mas)
        {
            var bankId = bank_Mas.Bank_Id > 0 ? new SqlParameter("@Bank_Id", bank_Mas.Bank_Id) : new SqlParameter("@Bank_Id", DBNull.Value);
            var bankName = !string.IsNullOrEmpty(bank_Mas.Bank_Name) ? new SqlParameter("@Bank_Name", bank_Mas.Bank_Name) : new SqlParameter("@Bank_Name", DBNull.Value);
            var branchName = !string.IsNullOrEmpty(bank_Mas.Branch_Name) ? new SqlParameter("@Branch_Name", bank_Mas.Branch_Name) : new SqlParameter("@Branch_Name", DBNull.Value);
            var branchAddress = !string.IsNullOrEmpty(bank_Mas.Branch_Address) ? new SqlParameter("@Branch_Address", bank_Mas.Branch_Address) : new SqlParameter("@Branch_Address", DBNull.Value);
            var ifscCode = !string.IsNullOrEmpty(bank_Mas.Ifsc_Code) ? new SqlParameter("@Ifsc_Code", bank_Mas.Ifsc_Code) : new SqlParameter("@Ifsc_Code", DBNull.Value);
            var correspondentBank = !string.IsNullOrEmpty(bank_Mas.Correspondent_Bank) ? new SqlParameter("@Correspondent_Bank", bank_Mas.Correspondent_Bank) : new SqlParameter("@Correspondent_Bank", DBNull.Value);
            var correspondentBankAddres = !string.IsNullOrEmpty(bank_Mas.Correspondent_Bank_Addres) ? new SqlParameter("@Correspondent_Bank_Addres", bank_Mas.Correspondent_Bank_Addres) : new SqlParameter("@Correspondent_Bank_Addres", DBNull.Value);
            var correspondentIfscCode = !string.IsNullOrEmpty(bank_Mas.Correspondent_Ifsc_Code) ? new SqlParameter("@Correspondent_Ifsc_Code", bank_Mas.Correspondent_Ifsc_Code) : new SqlParameter("@Correspondent_Ifsc_Code", DBNull.Value);
            var orderNo = bank_Mas.Order_No > 0 ? new SqlParameter("@Order_No", bank_Mas.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var sortNo = bank_Mas.Sort_No > 0 ? new SqlParameter("@Sort_No", bank_Mas.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var status = new SqlParameter("@Status", bank_Mas.Status);
            var bankCode = !string.IsNullOrEmpty(bank_Mas.Bank_Code) ? new SqlParameter("@Bank_Code", bank_Mas.Bank_Code) : new SqlParameter("@Bank_Code", DBNull.Value);
            var branchCode = !string.IsNullOrEmpty(bank_Mas.Branch_Code) ? new SqlParameter("@Branch_Code", bank_Mas.Branch_Code) : new SqlParameter("@Branch_Code", DBNull.Value);
            var accountNo = !string.IsNullOrEmpty(bank_Mas.Account_No) ? new SqlParameter("@Account_No", bank_Mas.Account_No) : new SqlParameter("@Account_No", DBNull.Value);
            var correspondentAccountNo = !string.IsNullOrEmpty(bank_Mas.Correspondent_Bank_Account_No) ? new SqlParameter("@Correspondent_Bank_Account_No", bank_Mas.Correspondent_Bank_Account_No) : new SqlParameter("@Correspondent_Bank_Account_No", DBNull.Value);
            var currency_Id = bank_Mas.Currency_Id > 0 ? new SqlParameter("@Currency_Id", bank_Mas.Currency_Id) : new SqlParameter("@Currency_Id", DBNull.Value);
            var account_Type = bank_Mas.Account_Type > 0 ? new SqlParameter("@Account_Type", bank_Mas.Account_Type) : new SqlParameter("@Account_Type", DBNull.Value);
            var recordType = new SqlParameter("@recordType", "Insert");
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Bank_Mas_Insert_Update @Bank_Id, @Bank_Name, @Branch_Name, @Branch_Address, @Ifsc_Code, @Correspondent_Bank, 
                                @Correspondent_Bank_Addres, @Correspondent_Ifsc_Code, @Order_No, @Sort_No, @Status, @Bank_Code, @Branch_Code, @Account_No, @Correspondent_Bank_Account_No,
                                @Currency_Id, @Account_Type, @recordType, @IsExistOrderNo OUT, @IsExistSortNo OUT", bankId, bankName, branchName, branchAddress, ifscCode, correspondentBank, correspondentBankAddres,
                                correspondentIfscCode, orderNo, sortNo, status, bankCode, branchCode, accountNo, correspondentAccountNo, currency_Id, account_Type, recordType, isExistOrderNo, isExistSortNo));

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            if (CoreService.Enable_Trace_Records(_configuration))
            {
                await Insert_Bank_Trace(bank_Mas, "Insert");
            }

            return result;
        }
        public async Task<int> UpdateBank(Bank_Master bank_Mas)
        {
            var bankId = bank_Mas.Bank_Id > 0 ? new SqlParameter("@Bank_Id", bank_Mas.Bank_Id) : new SqlParameter("@Bank_Id", DBNull.Value);
            var bankName = !string.IsNullOrEmpty(bank_Mas.Bank_Name) ? new SqlParameter("@Bank_Name", bank_Mas.Bank_Name) : new SqlParameter("@Bank_Name", DBNull.Value);
            var branchName = !string.IsNullOrEmpty(bank_Mas.Branch_Name) ? new SqlParameter("@Branch_Name", bank_Mas.Branch_Name) : new SqlParameter("@Branch_Name", DBNull.Value);
            var branchAddress = !string.IsNullOrEmpty(bank_Mas.Branch_Address) ? new SqlParameter("@Branch_Address", bank_Mas.Branch_Address) : new SqlParameter("@Branch_Address", DBNull.Value);
            var ifscCode = !string.IsNullOrEmpty(bank_Mas.Ifsc_Code) ? new SqlParameter("@Ifsc_Code", bank_Mas.Ifsc_Code) : new SqlParameter("@Ifsc_Code", DBNull.Value);
            var correspondentBank = !string.IsNullOrEmpty(bank_Mas.Correspondent_Bank) ? new SqlParameter("@Correspondent_Bank", bank_Mas.Correspondent_Bank) : new SqlParameter("@Correspondent_Bank", DBNull.Value);
            var correspondentBankAddres = !string.IsNullOrEmpty(bank_Mas.Correspondent_Bank_Addres) ? new SqlParameter("@Correspondent_Bank_Addres", bank_Mas.Correspondent_Bank_Addres) : new SqlParameter("@Correspondent_Bank_Addres", DBNull.Value);
            var correspondentIfscCode = !string.IsNullOrEmpty(bank_Mas.Correspondent_Ifsc_Code) ? new SqlParameter("@Correspondent_Ifsc_Code", bank_Mas.Correspondent_Ifsc_Code) : new SqlParameter("@Correspondent_Ifsc_Code", DBNull.Value);
            var orderNo = bank_Mas.Order_No > 0 ? new SqlParameter("@Order_No", bank_Mas.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var sortNo = bank_Mas.Sort_No > 0 ? new SqlParameter("@Sort_No", bank_Mas.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var status = new SqlParameter("@Status", bank_Mas.Status);
            var bankCode = !string.IsNullOrEmpty(bank_Mas.Bank_Code) ? new SqlParameter("@Bank_Code", bank_Mas.Bank_Code) : new SqlParameter("@Bank_Code", DBNull.Value);
            var branchCode = !string.IsNullOrEmpty(bank_Mas.Branch_Code) ? new SqlParameter("@Branch_Code", bank_Mas.Branch_Code) : new SqlParameter("@Branch_Code", DBNull.Value);
            var accountNo = !string.IsNullOrEmpty(bank_Mas.Account_No) ? new SqlParameter("@Account_No", bank_Mas.Account_No) : new SqlParameter("@Account_No", DBNull.Value);
            var correspondentAccountNo = !string.IsNullOrEmpty(bank_Mas.Correspondent_Bank_Account_No) ? new SqlParameter("@Correspondent_Bank_Account_No", bank_Mas.Correspondent_Bank_Account_No) : new SqlParameter("@Correspondent_Bank_Account_No", DBNull.Value);
            var currency_Id = bank_Mas.Currency_Id > 0 ? new SqlParameter("@Currency_Id", bank_Mas.Currency_Id) : new SqlParameter("@Currency_Id", DBNull.Value);
            var account_Type = bank_Mas.Account_Type > 0 ? new SqlParameter("@Account_Type", bank_Mas.Account_Type) : new SqlParameter("@Account_Type", DBNull.Value);
            var recordType = new SqlParameter("@recordType", "Update");
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Bank_Mas_Insert_Update @Bank_Id, @Bank_Name, @Branch_Name, @Branch_Address, @Ifsc_Code, @Correspondent_Bank, 
                                @Correspondent_Bank_Addres, @Correspondent_Ifsc_Code, @Order_No, @Sort_No, @Status, @Bank_Code, @Branch_Code, @Account_No, @Correspondent_Bank_Account_No,
                                @Currency_Id, @Account_Type, @recordType, @IsExistOrderNo OUT, @IsExistSortNo OUT", bankId, bankName, branchName, branchAddress, ifscCode, correspondentBank, correspondentBankAddres,
                                correspondentIfscCode, orderNo, sortNo, status, bankCode, branchCode, accountNo, correspondentAccountNo, currency_Id, recordType, account_Type, isExistOrderNo, isExistSortNo));

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            if (CoreService.Enable_Trace_Records(_configuration))
            {
                await Insert_Bank_Trace(bank_Mas, "Update");
            }

            return result;
        }
        public async Task<int> DeleteBank(int bankId)
        {
            if (CoreService.Enable_Trace_Records(_configuration))
            {
                var bank_Mas = await Task.Run(() => _dbContext.Bank_Master
                            .FromSqlRaw(@"exec Bank_Mas_Select @Bank_Id", bankId)
                            .AsEnumerable()
                            .FirstOrDefault());
                if(bank_Mas != null)
                {
                    await Insert_Bank_Trace(bank_Mas, "Delete");
                }
            }
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Bank_Mas_Delete {bankId}"));
        }
        public async Task<IList<Bank_Master>> GetBank(int bankId)
        {
            var BankId = bankId > 0 ? new SqlParameter("@Bank_Id", bankId) : new SqlParameter("@Bank_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Bank_Master
                            .FromSqlRaw(@"exec Bank_Mas_Select @Bank_Id", BankId).ToListAsync());

            return result;
        }
        public async Task<IList<Bank_Master>> Get_Active_Bank(int bankId, string bank_Name, int currency_Id)
        {
            var _bank_Id = bankId > 0 ? new SqlParameter("@Bank_Id", bankId) : new SqlParameter("@Bank_Id", DBNull.Value);
            var _bank_Name = !string.IsNullOrEmpty(bank_Name) ? new SqlParameter("@Bank_Name", bank_Name) : new SqlParameter("@Bank_Name", DBNull.Value);
            var _currency_Id = currency_Id > 0 ? new SqlParameter("@Currency_Id", currency_Id) : new SqlParameter("@Currency_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Bank_Master
                            .FromSqlRaw(@"exec Bank_Mas_Active_Select @Bank_Id, @Bank_Name, @Currency_Id", _bank_Id, _bank_Name, _currency_Id).ToListAsync());

            return result;
        }
        public async Task<int> BankChangeStatus(int bank_Id, bool status)
        {
            var bankId = new SqlParameter("@Bank_Id", bank_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Bank_Master_Update_Status @Bank_Id, @Status", bankId, Status));
            return result;
        }
        public async Task<IList<Bank_Dropdown_Model>> Get_Bank_Distinct()
        {
            var result = await Task.Run(() => _dbContext.Bank_Dropdown_Model
                            .FromSqlRaw(@"exec Bank_Master_Distinct_Select").ToListAsync());

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Bank_Branch(string bank_Name)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Bank_Master_Branch_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Bank_Name", bank_Name));

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
