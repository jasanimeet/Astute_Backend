using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace astute.Repository
{
    public partial class PartyService : IPartyService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISupplierService _supplierService;
        private readonly IMemoryCache _cache;
        #endregion

        #region Ctor
        public PartyService(AstuteDbContext dbContext,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ISupplierService supplierService,
            IMemoryCache cache
            )
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _supplierService = supplierService;
            _cache = cache;
        }
        #endregion

        #region Utilitie
        private async Task Insert_Party_Master_Trace(Party_Master party_Master, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var party_Type = new SqlParameter("@Party_Type", party_Master.Party_Type);
            var party_Code = new SqlParameter("@Party_Code", party_Master.Party_Code);
            var party_Address1 = new SqlParameter("@Adress_1", party_Master.Adress_1);
            var party_Address2 = !string.IsNullOrEmpty(party_Master.Adress_2) ? new SqlParameter("@Adress_2", party_Master.Adress_2) : new SqlParameter("@Adress_2", DBNull.Value);
            var party_Address3 = !string.IsNullOrEmpty(party_Master.Adress_3) ? new SqlParameter("@Adress_3", party_Master.Adress_3) : new SqlParameter("@Adress_3", DBNull.Value);
            var city_Id = new SqlParameter("@City_Id", party_Master.City_Id);
            var pin_Code = !string.IsNullOrEmpty(party_Master.PinCode) ? new SqlParameter("@PinCode", party_Master.PinCode) : new SqlParameter("@PinCode", DBNull.Value);
            var mobile_No1 = !string.IsNullOrEmpty(party_Master.Mobile_1) ? new SqlParameter("@Mobile_1", party_Master.Mobile_1) : new SqlParameter("@Mobile_1", DBNull.Value);
            var mobile_No2 = !string.IsNullOrEmpty(party_Master.Mobile_2) ? new SqlParameter("@Mobile_2", party_Master.Mobile_2) : new SqlParameter("@Mobile_2", DBNull.Value);
            var phone_No1 = !string.IsNullOrEmpty(party_Master.Phone_1) ? new SqlParameter("@Phone_1", party_Master.Phone_1) : new SqlParameter("@Phone_1", DBNull.Value);
            var phone_No2 = !string.IsNullOrEmpty(party_Master.Phone_2) ? new SqlParameter("@Phone_2", party_Master.Phone_2) : new SqlParameter("@Phone_2", DBNull.Value);
            var fax = !string.IsNullOrEmpty(party_Master.Fax) ? new SqlParameter("@Fax", party_Master.Fax) : new SqlParameter("@Fax", DBNull.Value);
            var email_1 = !string.IsNullOrEmpty(party_Master.Email_1) ? new SqlParameter("@Email_1", party_Master.Email_1) : new SqlParameter("@Email_1", DBNull.Value);
            var email_2 = !string.IsNullOrEmpty(party_Master.Email_2) ? new SqlParameter("@Email_2", party_Master.Email_2) : new SqlParameter("@Email_2", DBNull.Value);
            var party_Name = new SqlParameter("@Party_Name", party_Master.Party_Name);
            var ship_PartyId = party_Master.Ship_PartyId > 0 ? new SqlParameter("@Ship_PartyId", party_Master.Ship_PartyId) : new SqlParameter("@Ship_PartyId", DBNull.Value);

            await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC Party_Master_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @Record_Type, @Party_Type, @Adress_1, @Adress_2, @Adress_3,
            @City_Id, @PinCode, @Mobile_1, @Mobile_2, @Phone_1, @Phone_2, @Fax, @Email_1, @Email_2, @Comp_Bank, @Party_Name, @Ship_PartyId", empId, ipaddress, date, time,
            record_Type, party_Type, party_Code, party_Address1, party_Address2, party_Address3, city_Id, pin_Code, mobile_No1, mobile_No2, phone_No1, phone_No2, fax,
            email_1, email_2, party_Name, ship_PartyId));
        }
        public async Task Insert_Party_Contact_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Struct_Party_Contact_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Party_Contact_Trace_Data_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Party_Contact_Trace_Insert @Struct_Party_Contact_Trace", parameter);
        }
        public async Task Insert_Party_Bank_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Struct_Party_Bank_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Party_Bank_Trace_Data_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Party_Bank_Trace_Insert @Struct_Party_Bank_Trace", parameter);
        }
        public async Task Insert_Party_Document_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Struct_Party_Document_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Party_Document_Trace_Data_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Party_Document_Trace_Insert @Struct_Party_Document_Trace", parameter);
        }
        public async Task Insert_Party_Assist_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Struct_Party_Assist_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Party_Assist_Trace_Data_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Party_Assist_Trace_Insert @Struct_Party_Assist_Trace", parameter);
        }
        public async Task Insert_Party_Shipping_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Struct_Party_Shipping_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Party_Shipping_Trace_Data_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Party_Shipping_Trace_Insert @Struct_Party_Shipping_Trace", parameter);
        }
        public async Task Insert_Party_Media_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblParty_Media_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Party_Media_Trace_Table_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Party_Media_Trace_Insert @tblParty_Media_Trace", parameter);
        }
        public async Task Insert_Party_Print_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblParty_Print_Process", SqlDbType.Structured)
            {
                TypeName = "dbo.[Party_Print_Process_Trace_Table_Type]",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Party_Contact_Trace_Insert @tblParty_Print_Process", parameter);
        }
        #endregion

        #region Methods
        #region Party Master        
        public async Task<(string, int)> DeleteParty(int party_Id)
        {
            var isReferencedParameter = new SqlParameter("@IsReference", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Party_Master_Delete @PartyId, @IsReference OUT",
                                        new SqlParameter("@PartyId", party_Id),
                                        isReferencedParameter);

            var isReferenced = (bool)isReferencedParameter.Value;
            if (isReferenced)
                return ("_reference_found", (int)HttpStatusCode.Conflict);

            if (result > 0)
                return ("success", result);
            else
                return ("success", result);
        }
        public async Task<IList<Party_Master_Replica>> GetPartyReplicateFromCache(int partyId, string partyType, int page_Size, int Page_No)
        {
            string cacheKey = $"PartyReplicate_{partyId}_{partyType}_{page_Size}_{Page_No}";

            if (_cache.TryGetValue(cacheKey, out IList<Party_Master_Replica> partyReplicate))
            {
                return partyReplicate;
            }

            partyReplicate = await GetParty_Raplicate_08052024(partyId, partyType, page_Size, Page_No);

            _cache.Set(cacheKey, partyReplicate, TimeSpan.FromMinutes(10)); // for 10 min

            return partyReplicate;
        }
        public async Task<IList<Party_Master_Replica>> GetPartyFromCache(int partyId, string partyType)
        {
            string cacheKey = $"PartyReplicate_{partyId}_{partyType}";

            if (_cache.TryGetValue(cacheKey, out IList<Party_Master_Replica> partyReplicate))
            {
                return partyReplicate;
            }

            partyReplicate = await GetParty_Raplicate(partyId, partyType);

            _cache.Set(cacheKey, partyReplicate, TimeSpan.FromMinutes(10)); // for 10 min

            return partyReplicate;
        }
        public async Task<IList<Party_Master_Replica>> GetParty_Raplicate(int party_Id, string party_Type)
        {

            var partyId = new SqlParameter("@PartyId", party_Id > 0 ? (object)party_Id : DBNull.Value);
            var partyType = new SqlParameter("@Party_Type", !string.IsNullOrEmpty(party_Type) ? (object)party_Type : DBNull.Value);

            var rawResult = await _dbContext.Party_Master_Replica.FromSqlRaw("exec Party_Master_Select_Raplicate @PartyId, @Party_Type", partyId, partyType)
                            .AsNoTracking()
                            .ToListAsync();

            var result = rawResult
            .Select(x => new Party_Master_Replica
            {
                Party_Id = x.Party_Id,
                Party_Type_Value = x.Party_Type_Value,
                Party_Code = x.Party_Code,
                Adress = x.Adress,
                City = x.City,
                State = x.State,
                Country = x.Country,
                Contact_Person = x.Contact_Person,
                PinCode = x.PinCode,
                Mobile_1 = x.Mobile_1,
                Mobile_2 = x.Mobile_2,
                Phone_1 = x.Phone_1,
                Phone_2 = x.Phone_2,
                Fax = x.Fax,
                Email_1 = x.Email_1,
                Email_2 = x.Email_2,
                Party_Name = x.Party_Name,
                Website = x.Website,
                Cust_Freight_Account_No = x.Cust_Freight_Account_No,
                Wechat_ID = x.Wechat_ID,
                Skype_ID = x.Skype_ID,
                Business_Reg_No = x.Business_Reg_No,
                TIN_No = x.TIN_No,
                Invoice_Grp = x.Invoice_Grp,
                Created_Date = x.Created_Date,
                Created_Time = x.Created_Time,
                Created_By = x.Created_By,
                Updated_Date = x.Updated_Date,
                Updated_Time = x.Updated_Time,
                Updated_By = x.Updated_By,
                Status = x.Status,
                LOOSE_Diamond_Type_Value = x.LOOSE_Diamond_Type_Value,
                LOOSE_Assist_Value_1 = x.LOOSE_Assist_Value_1,
                LOOSE_Per_1 = x.LOOSE_Per_1,
                LOOSE_Assist_Value_2 = x.LOOSE_Assist_Value_2,
                LOOSE_Per_2 = x.LOOSE_Per_2,
                LOOSE_Viewing_Rights_To_Values = x.LOOSE_Viewing_Rights_To_Values,
                LOOSE_Date = x.LOOSE_Date,
                CERTIFIED_Diamond_Type_Value = x.CERTIFIED_Diamond_Type_Value,
                CERTIFIED_Assist_Value_1 = x.CERTIFIED_Assist_Value_1,
                CERTIFIED_Per_1 = x.CERTIFIED_Per_1,
                CERTIFIED_Assist_Value_2 = x.CERTIFIED_Assist_Value_2,
                CERTIFIED_Per_2 = x.CERTIFIED_Per_2,
                CERTIFIED_Viewing_Rights_To_Values = x.CERTIFIED_Viewing_Rights_To_Values,
                CERTIFIED_Date = x.CERTIFIED_Date
            })
            .ToList();

            return result;

            //return await _dbContext.Party_Master_Replica.FromSqlRaw("exec Party_Master_Select_Raplicate @PartyId, @Party_Type", partyId, partyType).ToListAsync();
        }
        public async Task<IList<Party_Master_Replica>> GetParty_Raplicate_08052024(int party_Id, string party_Type, int page_Size, int Page_No)
        {
            var partyId = new SqlParameter("@PartyId", party_Id > 0 ? (object)party_Id : DBNull.Value);
            var partyType = new SqlParameter("@Party_Type", !string.IsNullOrEmpty(party_Type) ? (object)party_Type : DBNull.Value);
            var _page_Size = new SqlParameter("@PageSize", page_Size > 0 ? (object)page_Size : DBNull.Value);
            var _page_num = new SqlParameter("@PageNumber", Page_No > 0 ? (object)Page_No : DBNull.Value);
            var total_Rec_Count = new SqlParameter("@Total_Rec_Count", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await _dbContext.Party_Master_Replica.FromSqlRaw(@"EXEC Party_Master_Select_Raplicate_06052024 @PartyId, @Party_Type, @PageSize, @PageNumber, @Total_Rec_Count OUT", partyId, partyType, _page_Size, _page_num, total_Rec_Count).AsNoTracking()
                .ToListAsync();

            var _total_Rec_Count = (int)total_Rec_Count.Value;

            return result;

            //return await _dbContext.Party_Master_Replica.FromSqlRaw("exec Party_Master_Select_Raplicate @PartyId, @Party_Type", partyId, partyType).ToListAsync();
        }

        //public async Task<List<Dictionary<string, object>>> GetParty(int party_Id, string party_Type)
        //{
        //    var result = new List<Dictionary<string, object>>();
        //    DataTable dataTable = new DataTable();
        //    using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
        //    {
        //        using (var command = new SqlCommand("Party_Master_Select", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.Add(party_Id > 0 ? new SqlParameter("@PartyId", party_Id) : new SqlParameter("@PartyId", DBNull.Value));
        //            command.Parameters.Add(!string.IsNullOrEmpty(party_Type) ? new SqlParameter("@Party_Type", party_Type) : new SqlParameter("@Party_Type", DBNull.Value));

        //            await connection.OpenAsync();

        //            using var da = new SqlDataAdapter();
        //            da.SelectCommand = command;

        //            using var ds = new DataSet();
        //            da.Fill(ds);

        //            dataTable = ds.Tables[ds.Tables.Count - 1];

        //            foreach (DataRow row in dataTable.Rows)
        //            {
        //                var dict = new Dictionary<string, object>();
        //                foreach (DataColumn col in dataTable.Columns)
        //                {
        //                    if (row[col] == DBNull.Value)
        //                    {
        //                        dict[col.ColumnName] = null;
        //                    }
        //                    else
        //                    {
        //                        dict[col.ColumnName] = row[col];
        //                    }
        //                }
        //                result.Add(dict);
        //            }
        //        }
        //    }

        //    return result;
        //}
        public async Task<List<Dictionary<string, object>>> GetPartyCustomer(int party_Id, string party_Type)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Party_Master_Detail_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(party_Id > 0 ? new SqlParameter("@PartyId", party_Id) : new SqlParameter("@PartyId", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(party_Type) ? new SqlParameter("@Party_Type", party_Type) : new SqlParameter("@Party_Type", DBNull.Value));
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> GetParty(int party_Id, string party_Type)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Party_Master_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(party_Id > 0 ? new SqlParameter("@PartyId", party_Id) : new SqlParameter("@PartyId", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(party_Type) ? new SqlParameter("@Party_Type", party_Type) : new SqlParameter("@Party_Type", DBNull.Value));

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

        #region Party Contact        
        public async Task<int> AddUpdatePartyContact(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Party_Contact", SqlDbType.Structured)
            {
                TypeName = "dbo.Party_Contact_Data_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_Contact_Insert_Update @Party_Contact", parameter));

            return result;
        }
        public async Task<IList<DropdownModel>> Get_User_Name_From_Party_Contact(int party_Id)
        {
            var _party_Id = party_Id > 0 ? new SqlParameter("@Party_Id", party_Id) : new SqlParameter("@Party_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"EXEC Get_User_Name_From_Party_Contact @Party_Id", _party_Id).ToListAsync());

            return result;
        }
        #endregion

        #region Party Bank
        public async Task<int> AddUpdatePartyBank(DataTable dataTable)
        {
            var parameter = new SqlParameter("@party_Bank", SqlDbType.Structured)
            {
                TypeName = "dbo.Party_Bank_Data_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database.ExecuteSqlRawAsync(@"EXEC Party_Bank_Insert_Update @party_Bank", parameter));

            return result;
        }
        public async Task<int> PartyBankChangeStatus(int account_Id, bool status)
        {
            var accountId = new SqlParameter("@Account_Id", account_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Party_Bank_Update_Status @Account_Id, @Status", accountId, Status));
            return result;
        }
        #endregion

        #region Party Shipping
        public async Task<int> AddUpdatePartyShipping(DataTable dataTable)
        {
            var parameter = new SqlParameter("@party_Shipping", SqlDbType.Structured)
            {
                TypeName = "dbo.Party_Shipping_Data_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database.ExecuteSqlRawAsync(@"EXEC Party_Shipping_Insert_Update @party_Shipping", parameter));

            return result;
        }
        public async Task<int> PartyShippingChangeStatus(int ship_Id, bool status)
        {
            var shipId = new SqlParameter("@Ship_Id", ship_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Party_Shipping_Update_Status @Ship_Id, @Status", shipId, Status));
            return result;
        }
        #endregion

        #region Party Document
        public async Task<int> AddUpdatePartyDocument(DataTable dataTable)
        {
            var parameter = new SqlParameter("@party_Document", SqlDbType.Structured)
            {
                TypeName = "dbo.Party_Document_Data_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_Document_Insert_Update @party_Document", parameter));

            return result;
        }
        public async Task<IList<Party_Document>> GetPartyDocument(int document_Id, int party_Id)
        {
            var documentId = document_Id > 0 ? new SqlParameter("@Document_Id", document_Id) : new SqlParameter("@Document_Id", DBNull.Value);
            var partyId = party_Id > 0 ? new SqlParameter("@PartyId", party_Id) : new SqlParameter("@PartyId", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Party_Document
                            .FromSqlRaw(@"exec Party_Document_Select @Document_Id, @PartyId", documentId, partyId).ToListAsync());
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.Upload_Path = !string.IsNullOrEmpty(item.Upload_Path) ? _configuration["BaseUrl"] + CoreCommonFilePath.PartyDocumentsPath + item.Upload_Path : null;
                }
            }
            return result;
        }
        #endregion

        #region Party Assist
        public async Task<int> AddUpdatePartyAssist(DataTable dataTable)
        {
            var parameter = new SqlParameter("@party_Assist", SqlDbType.Structured)
            {
                TypeName = "dbo.Party_Assist_Data_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_Assist_Insert_Update @party_Assist", parameter));
            return result;
        }
        #endregion

        #region Party Media
        public async Task<int> AddUpdatePartyMedia(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblPartyMedia", SqlDbType.Structured)
            {
                TypeName = "dbo.Party_Media_Table_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_Media_Insert_Update @tblPartyMedia", parameter));
            return result;
        }
        #endregion

        #region Party Print Process
        public async Task<int> Add_Update_Party_Print_Process(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblParty_Print_Process", SqlDbType.Structured)
            {
                TypeName = "dbo.Party_Print_Process_Table_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_Print_Process_Insert_Update @tblParty_Print_Process", parameter));
            return result;
        }
        #endregion
        #endregion

        #region Party Details
        public async Task<(string, int)> Add_Update_Party(Party_Master party_Master)
        {
            var party_Id = new SqlParameter("@Party_Id", party_Master.Party_Id);
            var party_Type = new SqlParameter("@Party_Type", party_Master.Party_Type);
            var party_Code = !string.IsNullOrEmpty(party_Master.Party_Code) ? new SqlParameter("@Party_Code", party_Master.Party_Code) : new SqlParameter("@Party_Code", DBNull.Value);
            var party_Address1 = !string.IsNullOrEmpty(party_Master.Adress_1) ? new SqlParameter("@Adress_1", party_Master.Adress_1) : new SqlParameter("@Adress_1", DBNull.Value);
            var party_Address2 = !string.IsNullOrEmpty(party_Master.Adress_2) ? new SqlParameter("@Adress_2", party_Master.Adress_2) : new SqlParameter("@Adress_2", DBNull.Value);
            var party_Address3 = !string.IsNullOrEmpty(party_Master.Adress_3) ? new SqlParameter("@Adress_3", party_Master.Adress_3) : new SqlParameter("@Adress_3", DBNull.Value);
            var city_Id = party_Master.City_Id > 0 ? new SqlParameter("@City_Id", party_Master.City_Id) : new SqlParameter("@City_Id", DBNull.Value);
            var pin_Code = !string.IsNullOrEmpty(party_Master.PinCode) ? new SqlParameter("@PinCode", party_Master.PinCode) : new SqlParameter("@PinCode", DBNull.Value);
            var mobile_No1 = !string.IsNullOrEmpty(party_Master.Mobile_1) ? new SqlParameter("@Mobile_1", party_Master.Mobile_1) : new SqlParameter("@Mobile_1", DBNull.Value);
            var mobile_1_Country_Code = !string.IsNullOrEmpty(party_Master.Mobile_1_Country_Code) ? new SqlParameter("@Mobile_1_Country_Code", party_Master.Mobile_1_Country_Code) : new SqlParameter("@Mobile_1_Country_Code", DBNull.Value);
            var mobile_No2 = !string.IsNullOrEmpty(party_Master.Mobile_2) ? new SqlParameter("@Mobile_2", party_Master.Mobile_2) : new SqlParameter("@Mobile_2", DBNull.Value);
            var mobile_2_Country_Code = !string.IsNullOrEmpty(party_Master.Mobile_2_Country_Code) ? new SqlParameter("@Mobile_2_Country_Code", party_Master.Mobile_2_Country_Code) : new SqlParameter("@Mobile_2_Country_Code", DBNull.Value);
            var phone_No1 = !string.IsNullOrEmpty(party_Master.Phone_1) ? new SqlParameter("@Phone_1", party_Master.Phone_1) : new SqlParameter("@Phone_1", DBNull.Value);
            var phone_1_Country_Code = !string.IsNullOrEmpty(party_Master.Phone_1_Country_Code) ? new SqlParameter("@Phone_1_Country_Code", party_Master.Phone_1_Country_Code) : new SqlParameter("@Phone_1_Country_Code", DBNull.Value);
            var phone_No2 = !string.IsNullOrEmpty(party_Master.Phone_2) ? new SqlParameter("@Phone_2", party_Master.Phone_2) : new SqlParameter("@Phone_2", DBNull.Value);
            var phone_2_Country_Code = !string.IsNullOrEmpty(party_Master.Phone_2_Country_Code) ? new SqlParameter("@Phone_2_Country_Code", party_Master.Phone_2_Country_Code) : new SqlParameter("@Phone_2_Country_Code", DBNull.Value);
            var fax = !string.IsNullOrEmpty(party_Master.Fax) ? new SqlParameter("@Fax", party_Master.Fax) : new SqlParameter("@Fax", DBNull.Value);
            var fax_Country_Code = !string.IsNullOrEmpty(party_Master.Fax_Country_Code) ? new SqlParameter("@Fax_Country_Code", party_Master.Fax_Country_Code) : new SqlParameter("@Fax_Country_Code", DBNull.Value);
            var email_1 = !string.IsNullOrEmpty(party_Master.Email_1) ? new SqlParameter("@Email_1", party_Master.Email_1) : new SqlParameter("@Email_1", DBNull.Value);
            var email_2 = !string.IsNullOrEmpty(party_Master.Email_2) ? new SqlParameter("@Email_2", party_Master.Email_2) : new SqlParameter("@Email_2", DBNull.Value);
            var party_Name = new SqlParameter("@Party_Name", party_Master.Party_Name);
            var ship_PartyId = party_Master.Ship_PartyId > 0 ? new SqlParameter("@Ship_PartyId", party_Master.Ship_PartyId) : new SqlParameter("@Ship_PartyId", DBNull.Value);
            var final_Customer_Id = party_Master.Final_Customer_Id > 0 ? new SqlParameter("@Final_Customer_Id", party_Master.Final_Customer_Id) : new SqlParameter("@Final_Customer_Id", DBNull.Value);
            var website = !string.IsNullOrEmpty(party_Master.Website) ? new SqlParameter("@Website", party_Master.Website) : new SqlParameter("@Website", DBNull.Value);
            var cust_Freight_Account_No = !string.IsNullOrEmpty(party_Master.Cust_Freight_Account_No) ? new SqlParameter("@Cust_Freight_Account_No", party_Master.Cust_Freight_Account_No) : new SqlParameter("@Cust_Freight_Account_No", DBNull.Value);
            var alias_Name = !string.IsNullOrEmpty(party_Master.Alias_Name) ? new SqlParameter("@Alias_Name", party_Master.Alias_Name) : new SqlParameter("@Alias_Name", DBNull.Value);
            var wechat_ID = !string.IsNullOrEmpty(party_Master.Wechat_ID) ? new SqlParameter("@Wechat_ID", party_Master.Wechat_ID) : new SqlParameter("@Wechat_ID", DBNull.Value);
            var skype_ID = !string.IsNullOrEmpty(party_Master.Skype_ID) ? new SqlParameter("@Skype_ID", party_Master.Skype_ID) : new SqlParameter("@Skype_ID", DBNull.Value);
            var business_Reg_No = !string.IsNullOrEmpty(party_Master.Business_Reg_No) ? new SqlParameter("@Business_Reg_No", party_Master.Business_Reg_No) : new SqlParameter("@Business_Reg_No", DBNull.Value);
            var default_Remarks = party_Master.Default_Remarks > 0 ? new SqlParameter("@Default_Remarks", party_Master.Default_Remarks) : new SqlParameter("@Default_Remarks", DBNull.Value);
            var notification = !string.IsNullOrEmpty(party_Master.Notification) ? new SqlParameter("@Notification", party_Master.Notification) : new SqlParameter("@Notification", DBNull.Value);
            var reference_By = !string.IsNullOrEmpty(party_Master.Reference_By) ? new SqlParameter("@Reference_By", party_Master.Reference_By) : new SqlParameter("@Reference_By", DBNull.Value);
            var tIN_No = !string.IsNullOrEmpty(party_Master.TIN_No) ? new SqlParameter("@TIN_No", party_Master.TIN_No) : new SqlParameter("@TIN_No", DBNull.Value);
            var invoice_Grp = !string.IsNullOrEmpty(party_Master.Invoice_Grp) ? new SqlParameter("@Invoice_Grp", party_Master.Invoice_Grp) : new SqlParameter("@Invoice_Grp", DBNull.Value);
            var modified_By = party_Master.Modified_By > 0 ? new SqlParameter("@Modified_By", party_Master.Modified_By) : new SqlParameter("@Modified_By", DBNull.Value);
            //var status = new SqlParameter("@Status", true);
            var insertedId = new SqlParameter("@InsertedId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var party_Exists = new SqlParameter("@Party_Exists", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_Master_Insert_Update @Party_Id, @Party_Type, @Party_Code, @Adress_1, @Adress_2, @Adress_3, @City_Id, @PinCode, @Mobile_1,
                        @Mobile_1_Country_Code, @Mobile_2, @Mobile_2_Country_Code, @Phone_1, @Phone_1_Country_Code, @Phone_2, @Phone_2_Country_Code, @Fax, @Fax_Country_Code, @Email_1, @Email_2, @Party_Name, @Ship_PartyId, @Final_Customer_Id, @Website,
                        @Cust_Freight_Account_No, @Alias_Name, @Wechat_ID, @Skype_ID, @Business_Reg_No, @Default_Remarks, @Notification, @Reference_By, @TIN_No,@Invoice_Grp, @Modified_By, @InsertedId OUT, @Party_Exists OUT", party_Id,
                        party_Type, party_Code, party_Address1, party_Address2, party_Address3, city_Id, pin_Code, mobile_No1, mobile_1_Country_Code, mobile_No2, mobile_2_Country_Code,
                        phone_No1, phone_1_Country_Code, phone_No2, phone_2_Country_Code, fax, fax_Country_Code, email_1, email_2,
                        party_Name, ship_PartyId, final_Customer_Id, website, cust_Freight_Account_No, alias_Name, wechat_ID, skype_ID, business_Reg_No,
                        default_Remarks, notification, reference_By, tIN_No, invoice_Grp, modified_By, insertedId, party_Exists));

            var _party_exists = (bool)party_Exists.Value;
            if (_party_exists)
                return ("_party_exists", 0);

            if (result > 0)
            {
                string record_Type = string.Empty;
                int _insertedId = (int)insertedId.Value;
                //if (CoreService.Enable_Trace_Records(_configuration))
                //{
                //    if (party_Master.Party_Id == 0)
                //        record_Type = "Insert";
                //    else
                //        record_Type = "Update";
                //    await Insert_Party_Master_Trace(party_Master, record_Type);
                //}
                return ("success", _insertedId);
            }
            return ("error", 0);
        }
        public async Task<Party_Master> Get_Party_Details(int party_Id)
        {
            var _partyId = party_Id > 0 ? new SqlParameter("@PartyId", party_Id) : new SqlParameter("@PartyId", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Party_Master
                            .FromSqlRaw(@"exec Party_Master_Detail_Select @PartyId", _partyId)
                            .AsEnumerable()
                            .FirstOrDefault());

            if (result != null)
            {
                if (party_Id > 0)
                {
                    result.Party_Contact_List = await Task.Run(() => _dbContext.Party_Contact
                                                .FromSqlRaw(@"exec Party_Contact_Select @Contact_Id, @PartyId", new SqlParameter("@Contact_Id", DBNull.Value), _partyId).ToListAsync());

                    result.Party_Bank_List = await Task.Run(() => _dbContext.Party_Bank
                                            .FromSqlRaw(@"exec Party_Bank_Select @Account_Id, @PartyId", new SqlParameter("@Account_Id", DBNull.Value), _partyId).ToListAsync());

                    result.Party_Shipping_List = await Task.Run(() => _dbContext.Party_Shipping
                                                .FromSqlRaw(@"exec Party_Shipping_Select @Ship_Id, @PartyId", new SqlParameter("@Ship_Id", DBNull.Value), _partyId).ToListAsync());

                    result.Party_Document_List = await Task.Run(() => _dbContext.Party_Document
                            .FromSqlRaw(@"exec Party_Document_Select @Document_Id, @PartyId", new SqlParameter("@Document_Id", DBNull.Value), _partyId).ToListAsync());
                    if (result.Party_Document_List != null && result.Party_Document_List.Count > 0)
                    {
                        foreach (var item in result.Party_Document_List)
                        {
                            item.Upload_Path = !string.IsNullOrEmpty(item.Upload_Path) ? _configuration["BaseUrl"] + CoreCommonFilePath.PartyDocumentsPath + item.Upload_Path : null;
                        }
                    }

                    result.Party_Assist_List = await Task.Run(() => _dbContext.Party_Assist
                            .FromSqlRaw(@"exec Party_Assist_Select @Assist_Id, @PartyId", new SqlParameter("@Assist_Id", DBNull.Value), _partyId).ToListAsync());

                    result.Party_Media_List = await Task.Run(() => _dbContext.Party_Media
                                            .FromSqlRaw(@"exec Party_Media_Select @PartyId", _partyId).ToListAsync());

                    result.Party_Print_Process_List = await Task.Run(() => _dbContext.Party_Print_Process
                                                .FromSqlRaw(@"exec Party_Print_Process_Select @PartyId", _partyId).ToListAsync());
                }
            }

            return result;
        }
        public async Task<int> Get_Party_Code()
        {
            var partyCodeParam = new SqlParameter("@Party_Code", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            await _dbContext.Database.ExecuteSqlRawAsync("exec Party_Code_Select @Party_Code OUTPUT", partyCodeParam);

            if (int.TryParse(partyCodeParam.Value?.ToString(), out int partyCode))
            {
                return partyCode;
            }
            else
            {
                return 0;
            }
        }
        public async Task<int> Party_Master_Change_Status(int party_Id, bool status)
        {
            var _party_Id = new SqlParameter("@Party_Id", party_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Party_Master_Update_Status @Party_Id, @Status", _party_Id, Status));
            return result;
        }
        #endregion

        #region Party API
        public async Task<int> Add_Update_Party_API(Party_Api party_Api, int modified_By)
        {
            var api_Id = new SqlParameter("@API_Id", party_Api.API_Id);
            var _party_Id = party_Api.Party_Id > 0 ? new SqlParameter("@Party_Id", party_Api.Party_Id) : new SqlParameter("@Party_Id", DBNull.Value);
            var api_Url = !string.IsNullOrEmpty(party_Api.API_URL) ? new SqlParameter("@API_URL", party_Api.API_URL) : new SqlParameter("@API_URL", DBNull.Value);
            var api_User = !string.IsNullOrEmpty(party_Api.API_User) ? new SqlParameter("@API_User", party_Api.API_User) : new SqlParameter("@API_User", DBNull.Value);
            var api_Password = !string.IsNullOrEmpty(party_Api.API_Password) ? new SqlParameter("@API_Password", party_Api.API_Password) : new SqlParameter("@API_Password", DBNull.Value);
            var api_Method = !string.IsNullOrEmpty(party_Api.API_Method) ? new SqlParameter("@API_Method", party_Api.API_Method) : new SqlParameter("@API_Method", DBNull.Value);
            var api_Response = !string.IsNullOrEmpty(party_Api.API_Response) ? new SqlParameter("@API_Response", party_Api.API_Response) : new SqlParameter("@API_Response", DBNull.Value);
            var api_Status = new SqlParameter("@API_Status", party_Api.API_Status ?? false);
            var api_Inverse = new SqlParameter("@Disc_Inverse", party_Api.Disc_Inverse ?? false);
            var auto_Ref_No = new SqlParameter("@Auto_Ref_No", party_Api.Auto_Ref_No ?? false);
            var repeateveryType = !string.IsNullOrEmpty(party_Api.RepeateveryType) ? new SqlParameter("@RepeateveryType", party_Api.RepeateveryType) : new SqlParameter("@RepeateveryType", DBNull.Value);
            var repeatevery = !string.IsNullOrEmpty(party_Api.Repeatevery) ? new SqlParameter("@Repeatevery", party_Api.Repeatevery) : new SqlParameter("@Repeatevery", DBNull.Value);
            var lab = new SqlParameter("@Lab", party_Api.Lab ?? false);
            var overseas = new SqlParameter("@Overseas", party_Api.Overseas ?? false);
            var stock_Url = !string.IsNullOrEmpty(party_Api.Stock_Url) ? new SqlParameter("@Stock_Url", party_Api.Stock_Url) : new SqlParameter("@Stock_Url", DBNull.Value);
            var user_Id = !string.IsNullOrEmpty(party_Api.User_Id) ? new SqlParameter("@User_Id", party_Api.User_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var user_Caption = !string.IsNullOrEmpty(party_Api.User_Caption) ? new SqlParameter("@User_Caption", party_Api.User_Caption) : new SqlParameter("@User_Caption", DBNull.Value);
            var password_Caption = !string.IsNullOrEmpty(party_Api.Password_Caption) ? new SqlParameter("@Password_Caption", party_Api.Password_Caption) : new SqlParameter("@Password_Caption", DBNull.Value);
            var action_Caption = !string.IsNullOrEmpty(party_Api.Action_Caption) ? new SqlParameter("@Action_Caption", party_Api.Action_Caption) : new SqlParameter("@Action_Caption", DBNull.Value);
            var action_Value = !string.IsNullOrEmpty(party_Api.Action_Value) ? new SqlParameter("@Action_Value", party_Api.Action_Value) : new SqlParameter("@Action_Value", DBNull.Value);
            var action_Caption_1 = !string.IsNullOrEmpty(party_Api.Action_Caption_1) ? new SqlParameter("@Action_Caption_1", party_Api.Action_Caption_1) : new SqlParameter("@Action_Caption_1", DBNull.Value);
            var action_Value_1 = !string.IsNullOrEmpty(party_Api.Action_Value_1) ? new SqlParameter("@Action_Value_1", party_Api.Action_Value_1) : new SqlParameter("@Action_Value_1", DBNull.Value);
            var action_Caption_2 = !string.IsNullOrEmpty(party_Api.Action_Caption_2) ? new SqlParameter("@Action_Caption_2", party_Api.Action_Caption_2) : new SqlParameter("@Action_Caption_2", DBNull.Value);
            var action_Value_2 = !string.IsNullOrEmpty(party_Api.Action_Value_2) ? new SqlParameter("@Action_Value_2", party_Api.Action_Value_2) : new SqlParameter("@Action_Value_2", DBNull.Value);
            var short_Code = !string.IsNullOrEmpty(party_Api.Short_Code) ? new SqlParameter("@Short_Code", party_Api.Short_Code) : new SqlParameter("@Short_Code", DBNull.Value);
            var stock_Api_Method = !string.IsNullOrEmpty(party_Api.Stock_Api_Method) ? new SqlParameter("@Stock_Api_Method", party_Api.Stock_Api_Method) : new SqlParameter("@Stock_Api_Method", DBNull.Value);
            var method_Type = !string.IsNullOrEmpty(party_Api.Method_Type) ? new SqlParameter("@Method_Type", party_Api.Method_Type) : new SqlParameter("@Method_Type", DBNull.Value);
            var is_Same_Id = new SqlParameter("@Is_Same_Id", party_Api.Is_Same_Id ?? false);
            var overseas_Same_Id = new SqlParameter("@Overseas_Same_Id", party_Api.Overseas_Same_Id ?? false);
            var _modified_By = new SqlParameter("@Modified_By", modified_By);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_Api_Insert_Update @API_Id, @Party_Id, @API_URL, @API_User, @API_Password, @API_Method, @API_Response, @API_Status,
                        @Disc_Inverse, @Auto_Ref_No, @RepeateveryType, @repeatevery, @Lab, @Overseas, @Stock_Url, @User_Id, @User_Caption, @Password_Caption, @Action_Caption, 
                        @Action_Value,@Action_Caption_1, @Action_Value_1, @Action_Caption_2, @Action_Value_2, @Short_Code, @Stock_Api_Method, @Method_Type, @Is_Same_Id, @Overseas_Same_Id, @Modified_By",
                        api_Id, _party_Id, api_Url, api_User, api_Password, api_Method, api_Response, api_Status, api_Inverse, auto_Ref_No, repeateveryType, repeatevery, lab, overseas,
                        stock_Url, user_Id, user_Caption, password_Caption, action_Caption, action_Value, action_Caption_1, action_Value_1, action_Caption_2, action_Value_2,
                        short_Code, stock_Api_Method, method_Type, is_Same_Id, overseas_Same_Id, _modified_By));

            return result;
        }
        public async Task<int> Delete_Party_API(int api_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Party_Api_Delete {api_Id}"));
        }
        public async Task<Party_Api> Get_Party_API(int api_Id, int party_Id)
        {
            var _api_Id = api_Id > 0 ? new SqlParameter("@API_Id", api_Id) : new SqlParameter("@API_Id", DBNull.Value);
            var _party_Id = party_Id > 0 ? new SqlParameter("@Party_Id", party_Id) : new SqlParameter("@Party_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Party_Api
                            .FromSqlRaw(@"exec Party_Api_Select @API_Id, @Party_Id", _api_Id, _party_Id)
                            .AsEnumerable()
                            .FirstOrDefault());

            return result;
        }
        public async Task<IList<Party_API_With_Column_Mapping>> Get_Party_API_With_Column_Mapping()
        {
            var _api_Id = new SqlParameter("@API_Id", DBNull.Value);
            var _party_Id = new SqlParameter("@Party_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Party_API_With_Column_Mapping
                            .FromSqlRaw(@"exec Party_Api_Col_Map_Val_Map_Select @API_Id, @Party_Id", _api_Id, _party_Id)
                            .ToList());
            if (result != null && result.Count > 0)
            {

                foreach (var item in result)
                {
                    item.Supplier_Column_Mapping_List = await Common_Funtion_To_Get_Supp_Col_Map(item.Party_Id ?? 0, item.Upload_Type);
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Common_Funtion_To_Get_Supp_Col_Map(int supp_Id, string upload_Type)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("Supplier_Column_Mapping_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Supp_Id", supp_Id));
                    command.Parameters.Add(new SqlParameter("@Map_Flag", "C"));
                    command.Parameters.Add(new SqlParameter("@Column_Type", upload_Type));

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
                await connection.CloseAsync();
            }

            return result;
        }
        #endregion

        #region Party FTP
        public async Task<int> Add_Update_Party_FTP(Party_FTP party_FTP, int modified_By)
        {
            var ftp_Id = new SqlParameter("@FTP_Id", party_FTP.FTP_Id);
            var _party_Id = party_FTP.Party_Id > 0 ? new SqlParameter("@Party_Id", party_FTP.Party_Id) : new SqlParameter("@Party_Id", DBNull.Value);
            var host = !string.IsNullOrEmpty(party_FTP.Host) ? new SqlParameter("@Host", party_FTP.Host) : new SqlParameter("@Host", DBNull.Value);
            var ftp_Port = party_FTP.Ftp_Port > 0 ? new SqlParameter("@Ftp_Port", party_FTP.Ftp_Port) : new SqlParameter("@Ftp_Port", DBNull.Value);
            var ftp_User = !string.IsNullOrEmpty(party_FTP.Ftp_User) ? new SqlParameter("@Ftp_User", party_FTP.Ftp_User) : new SqlParameter("@Ftp_User", DBNull.Value);
            var ftp_Pasword = !string.IsNullOrEmpty(party_FTP.Ftp_Password) ? new SqlParameter("@Ftp_Password", party_FTP.Ftp_Password) : new SqlParameter("@Ftp_Password", DBNull.Value);
            var ftp_File_Name = !string.IsNullOrEmpty(party_FTP.Ftp_File_Name) ? new SqlParameter("@Ftp_File_Name", party_FTP.Ftp_File_Name) : new SqlParameter("@Ftp_File_Name", DBNull.Value);
            var ftp_File_Type = !string.IsNullOrEmpty(party_FTP.Ftp_File_Type) ? new SqlParameter("@Ftp_File_Type", party_FTP.Ftp_File_Type) : new SqlParameter("@Ftp_File_Type", DBNull.Value);
            var api_Inverse = new SqlParameter("@Disc_Inverse", party_FTP.Disc_Inverse ?? false);
            var auto_Ref_No = new SqlParameter("@Auto_Ref_No", party_FTP.Auto_Ref_No ?? false);
            var repeateveryType = !string.IsNullOrEmpty(party_FTP.RepeateveryType) ? new SqlParameter("@RepeateveryType", party_FTP.RepeateveryType) : new SqlParameter("@RepeateveryType", DBNull.Value);
            var repeatevery = !string.IsNullOrEmpty(party_FTP.Repeatevery) ? new SqlParameter("@Repeatevery", party_FTP.Repeatevery) : new SqlParameter("@Repeatevery", DBNull.Value);
            var lab = new SqlParameter("@Lab", party_FTP.Lab ?? false);
            var overseas = new SqlParameter("@Overseas", party_FTP.Overseas ?? false);
            var secure_Ftp = new SqlParameter("@Secure_Ftp", party_FTP.Secure_Ftp ?? false);
            var short_Code = !string.IsNullOrEmpty(party_FTP.Short_Code) ? new SqlParameter("@Short_Code", party_FTP.Short_Code) : new SqlParameter("@Short_Code", DBNull.Value);
            var ftp_Status = new SqlParameter("@Ftp_Status", party_FTP.Ftp_Status ?? false);
            var is_Same_Id = new SqlParameter("@Is_Same_Id", party_FTP.Is_Same_Id ?? false);
            var overseas_Same_Id = new SqlParameter("@Overseas_Same_Id", party_FTP.Overseas_Same_Id ?? false);
            var _modified_By = new SqlParameter("@Modified_By", modified_By);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_FTP_Insert_Update @FTP_Id, @Party_Id, @Host, @Ftp_Port, @Ftp_User, @Ftp_Password, @Ftp_File_Name, @Ftp_File_Type,
                        @Disc_Inverse, @Auto_Ref_No, @RepeateveryType, @Repeatevery, @Lab, @Overseas, @Secure_Ftp, @Short_Code, @Ftp_Status, @Is_Same_Id, @Overseas_Same_Id, @Modified_By", ftp_Id, _party_Id, host, ftp_Port,
                        ftp_User, ftp_Pasword, ftp_File_Name, ftp_File_Type, api_Inverse, auto_Ref_No, repeateveryType, repeatevery, lab, overseas, secure_Ftp, short_Code, ftp_Status, is_Same_Id, overseas_Same_Id, _modified_By));

            return result;
        }
        public async Task<int> Delete_Party_FTP(int ftp_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Party_FTP_Delete {ftp_Id}"));
        }
        public async Task<Party_FTP> Get_Party_FTP(int ftp_Id, int party_Id)
        {
            var _ftp_Id = ftp_Id > 0 ? new SqlParameter("@FTP_Id", ftp_Id) : new SqlParameter("@FTP_Id", DBNull.Value);
            var _party_Id = party_Id > 0 ? new SqlParameter("@Party_Id", party_Id) : new SqlParameter("@Party_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Party_FTP
                            .FromSqlRaw(@"exec Party_FTP_Select @FTP_Id, @Party_Id", _ftp_Id, _party_Id)
                            .AsEnumerable()
                            .FirstOrDefault());

            return result;
        }
        #endregion

        #region Party File
        public async Task<int> Add_Update_Party_File(Party_File party_File, int modified_By)
        {
            var file_Id = new SqlParameter("@File_Id", party_File.File_Id);
            var _party_Id = party_File.Party_Id > 0 ? new SqlParameter("@Party_Id", party_File.Party_Id) : new SqlParameter("@Party_Id", DBNull.Value);
            var file_Location = !string.IsNullOrEmpty(party_File.File_Location) ? new SqlParameter("@File_Location", party_File.File_Location) : new SqlParameter("@File_Location", DBNull.Value);
            var short_Code = !string.IsNullOrEmpty(party_File.Short_Code) ? new SqlParameter("@Short_Code", party_File.Short_Code) : new SqlParameter("@Short_Code", DBNull.Value);
            var file_Status = new SqlParameter("@File_Status", party_File.File_Status ?? false);
            var lab = new SqlParameter("@Lab", party_File.Lab ?? false);
            var overseas = new SqlParameter("@Overseas", party_File.Overseas ?? false);
            var validity_Days = party_File.Validity_Days > 0 ? new SqlParameter("@Validity_Days", party_File.Validity_Days) : new SqlParameter("@Validity_Days", DBNull.Value);
            var sheet_Name = !string.IsNullOrEmpty(party_File.Sheet_Name) ? new SqlParameter("@Sheet_Name", party_File.Sheet_Name) : new SqlParameter("@Sheet_Name", DBNull.Value);
            var exclude = new SqlParameter("@Exclude", party_File.Exclude ?? false);
            var aPI_Flag = new SqlParameter("@API_Flag", party_File.API_Flag ?? false);
            var upload_Type = !string.IsNullOrEmpty(party_File.Upload_Type) ? new SqlParameter("@Upload_Type", party_File.Upload_Type) : new SqlParameter("@Upload_Type", DBNull.Value);
            var is_Same_Id = new SqlParameter("@Is_Same_Id", party_File.Is_Same_Id ?? false);
            var overseas_Same_Id = new SqlParameter("@Overseas_Same_Id", party_File.Overseas_Same_Id ?? false);
            var _modified_By = new SqlParameter("@Modified_By", modified_By);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_File_Insert_Update @File_Id, @Party_Id, @File_Location, @Short_Code, @File_Status, @Lab, @Overseas, @Validity_Days, @Sheet_Name,
                        @Exclude, @API_Flag, @Upload_Type, @Is_Same_Id, @Overseas_Same_Id, @Modified_By", file_Id, _party_Id, file_Location, short_Code, file_Status, lab, overseas, validity_Days, sheet_Name, exclude, aPI_Flag, upload_Type, is_Same_Id, overseas_Same_Id, _modified_By));

            return result;
        }
        public async Task<int> Delete_Party_File(int file_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Party_File_Delete {file_Id}"));
        }
        public async Task<Party_File> Get_Party_File(int file_Id, int party_Id)
        {
            var _file_Id = file_Id > 0 ? new SqlParameter("@File_Id", file_Id) : new SqlParameter("@File_Id", DBNull.Value);
            var _party_Id = party_Id > 0 ? new SqlParameter("@Party_Id", party_Id) : new SqlParameter("@Party_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Party_File
                            .FromSqlRaw(@"exec Party_File_Select @File_Id, @Party_Id", _file_Id, _party_Id)
                            .AsEnumerable()
                            .FirstOrDefault());

            return result;
        }
        #endregion

        #region Customer Party File
        public async Task<int> Add_Update_Customer_Party_File(Customer_Party_File party_File, int modified_By, string map_Flag)
        {
            var file_Id = new SqlParameter("@File_Id", party_File.File_Id);
            var _user_Pricing_Id = party_File.User_Pricing_Id > 0 ? new SqlParameter("@User_Pricing_Id", party_File.User_Pricing_Id) : new SqlParameter("@User_Pricing_Id", DBNull.Value);
            var file_Name = !string.IsNullOrEmpty(party_File.File_Name) ? new SqlParameter("@File_Name", party_File.File_Name) : new SqlParameter("@File_Name", DBNull.Value);
            var file_Type = !string.IsNullOrEmpty(party_File.File_Type) ? new SqlParameter("@File_Type", party_File.File_Type) : new SqlParameter("@File_Type", DBNull.Value);
            var iP = !string.IsNullOrEmpty(party_File.IP) ? new SqlParameter("@IP", party_File.IP) : new SqlParameter("@IP", DBNull.Value);
            var country = !string.IsNullOrEmpty(party_File.Country) ? new SqlParameter("@Country", party_File.Country) : new SqlParameter("@Country", DBNull.Value);
            var file_Status = new SqlParameter("@File_Status", party_File.File_Status ?? false);
            var _modified_By = new SqlParameter("@Modified_By", modified_By);
            var _map_Flag = !string.IsNullOrEmpty(map_Flag) ? new SqlParameter("@Map_Flag", map_Flag) : new SqlParameter("@Map_Flag", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Customer_Party_File_Insert_Update @File_Id, @User_Pricing_Id, @File_Name,@File_Type,@IP,@Country, @File_Status, @Modified_By, @Map_Flag",
                        file_Id, _user_Pricing_Id, file_Name, file_Type, iP, country, file_Status, _modified_By, _map_Flag));

            return result;
        }
        public async Task<int> Delete_Customer_Party_File(int file_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Customer_Party_File_Delete {file_Id}"));
        }
        public async Task<Customer_Party_File> Get_Customer_Party_File(int? file_Id, int party_Id, string map_Flag)
        {
            var _file_Id = file_Id > 0 ? new SqlParameter("@File_Id", file_Id) : new SqlParameter("@File_Id", DBNull.Value);
            var _party_Id = party_Id > 0 ? new SqlParameter("@Party_Id", party_Id) : new SqlParameter("@Party_Id", DBNull.Value);
            var _map_Flag = !string.IsNullOrEmpty(map_Flag) ? new SqlParameter("@Map_Flag", map_Flag) : new SqlParameter("@Map_Flag", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Customer_Party_File
                            .FromSqlRaw(@"exec [Customer_Party_File_Select] @File_Id, @Party_Id, @Map_Flag", _file_Id, _party_Id, _map_Flag)
                            .AsEnumerable()
                            .FirstOrDefault());

            return result;
        }
        #endregion

        #region Customer Party FTP
        public async Task<int> Add_Update_Customer_Party_FTP(Customer_Party_FTP party_FTP, int modified_By, string map_Flag)
        {
            var ftp_Id = new SqlParameter("@FTP_Id", party_FTP.FTP_Id);
            var _user_pricing_id = party_FTP.User_Pricing_Id > 0 ? new SqlParameter("@User_Pricing_Id", party_FTP.User_Pricing_Id) : new SqlParameter("@User_Pricing_Id", DBNull.Value);
            var host = !string.IsNullOrEmpty(party_FTP.Host) ? new SqlParameter("@Host", party_FTP.Host) : new SqlParameter("@Host", DBNull.Value);
            var ftp_Port = party_FTP.Ftp_Port > 0 ? new SqlParameter("@Ftp_Port", party_FTP.Ftp_Port) : new SqlParameter("@Ftp_Port", DBNull.Value);
            var ftp_User = !string.IsNullOrEmpty(party_FTP.Ftp_User) ? new SqlParameter("@Ftp_User", party_FTP.Ftp_User) : new SqlParameter("@Ftp_User", DBNull.Value);
            var ftp_Pasword = !string.IsNullOrEmpty(party_FTP.Ftp_Password) ? new SqlParameter("@Ftp_Password", party_FTP.Ftp_Password) : new SqlParameter("@Ftp_Password", DBNull.Value);
            var ftp_File_Name = !string.IsNullOrEmpty(party_FTP.Ftp_File_Name) ? new SqlParameter("@Ftp_File_Name", party_FTP.Ftp_File_Name) : new SqlParameter("@Ftp_File_Name", DBNull.Value);
            var ftp_File_Format = !string.IsNullOrEmpty(party_FTP.Ftp_File_Format) ? new SqlParameter("@Ftp_File_Format", party_FTP.Ftp_File_Format) : new SqlParameter("@Ftp_File_Format", DBNull.Value);
            var repeateveryType = !string.IsNullOrEmpty(party_FTP.RepeateveryType) ? new SqlParameter("@RepeateveryType", party_FTP.RepeateveryType) : new SqlParameter("@RepeateveryType", DBNull.Value);
            var repeatevery = !string.IsNullOrEmpty(party_FTP.Repeatevery) ? new SqlParameter("@Repeatevery", party_FTP.Repeatevery) : new SqlParameter("@Repeatevery", DBNull.Value);
            var _map_Flag = !string.IsNullOrEmpty(map_Flag) ? new SqlParameter("@Map_Flag", map_Flag) : new SqlParameter("@Map_Flag", DBNull.Value);
            var secure_Ftp = new SqlParameter("@Secure_Ftp", party_FTP.Secure_Ftp ?? false);
            var ftp_Status = new SqlParameter("@Ftp_Status", party_FTP.Ftp_Status ?? false);
            var _modified_By = new SqlParameter("@Modified_By", modified_By);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Customer_Party_FTP_Insert_Update @FTP_Id, @User_Pricing_Id, @Host, @Ftp_Port, @Ftp_User, @Ftp_Password, @Ftp_File_Name, @Ftp_File_Format,
                        @RepeateveryType, @Repeatevery, @Secure_Ftp, @Ftp_Status, @Modified_By, @Map_Flag",
                        ftp_Id, _user_pricing_id, host, ftp_Port, ftp_User, ftp_Pasword, ftp_File_Name, ftp_File_Format, repeateveryType, repeatevery, secure_Ftp, ftp_Status, _modified_By, _map_Flag));

            return result;
        }
        public async Task<int> Delete_Customer_Party_FTP(int ftp_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Customer_Party_FTP_Delete {ftp_Id}"));
        }
        public async Task<Customer_Party_FTP> Get_Customer_Party_FTP(int? ftp_Id, int user_Pricing_Id, string map_Flag)
        {
            var _ftp_Id = ftp_Id > 0 ? new SqlParameter("@FTP_Id", ftp_Id) : new SqlParameter("@FTP_Id", DBNull.Value);
            var _user_Pricing_Id = user_Pricing_Id > 0 ? new SqlParameter("@User_Pricing_Id", user_Pricing_Id) : new SqlParameter("@User_Pricing_Id", DBNull.Value);
            var _map_Flag = !string.IsNullOrEmpty(map_Flag) ? new SqlParameter("@Map_Flag", map_Flag) : new SqlParameter("@Map_Flag", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Customer_Party_FTP
                            .FromSqlRaw(@"exec Customer_Party_FTP_Select @FTP_Id, @User_Pricing_Id, @Map_Flag", _ftp_Id, _user_Pricing_Id, _map_Flag)
                            .AsEnumerable()
                            .FirstOrDefault());

            return result;
        }
        #endregion

        #region Customer Party API
        public async Task<int> Add_Update_Customer_Party_API(Customer_Party_Api party_API, int modified_By, string map_Flag)
        {
            var api_Id = new SqlParameter("@API_Id", party_API.API_Id);
            var _user_Pricing_Id = party_API.User_Pricing_Id > 0 ? new SqlParameter("@User_Pricing_Id", party_API.User_Pricing_Id) : new SqlParameter("@User_Pricing_Id", DBNull.Value);
            var format_Type = !string.IsNullOrEmpty(party_API.Format_Type) ? new SqlParameter("@Format_Type", party_API.Format_Type) : new SqlParameter("@Format_Type", DBNull.Value);
            var iP = !string.IsNullOrEmpty(party_API.IP) ? new SqlParameter("@IP", party_API.IP) : new SqlParameter("@IP", DBNull.Value);
            var country = !string.IsNullOrEmpty(party_API.Country) ? new SqlParameter("@Country", party_API.Country) : new SqlParameter("@Country", DBNull.Value);
            var api_Status = new SqlParameter("@API_Status", party_API.API_Status ?? false);
            var _modified_By = new SqlParameter("@Modified_By", modified_By);
            var _map_Flag = !string.IsNullOrEmpty(map_Flag) ? new SqlParameter("@Map_Flag", map_Flag) : new SqlParameter("@Map_Flag", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Customer_Party_API_Insert_Update @API_Id, @User_Pricing_Id, @Format_Type,@IP,@Country, @API_Status, @Modified_By, @Map_Flag",
                        api_Id, _user_Pricing_Id, format_Type, iP, country, api_Status, _modified_By, _map_Flag));

            return result;
        }
        public async Task<int> Delete_Customer_Part_API(int api_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Customer_Party_API_Delete {api_Id}"));
        }
        public async Task<Customer_Party_Api> Get_Customer_Party_API(int? api_Id, int user_pricing_id, string map_Flag)
        {
            var _api_Id = api_Id > 0 ? new SqlParameter("@API_Id", api_Id) : new SqlParameter("@API_Id", DBNull.Value);
            var _user_pricing_id = user_pricing_id > 0 ? new SqlParameter("@User_Pricing_Id", user_pricing_id) : new SqlParameter("@User_Pricing_Id", DBNull.Value);
            var _map_Flag = !string.IsNullOrEmpty(map_Flag) ? new SqlParameter("@Map_Flag", map_Flag) : new SqlParameter("@Map_Flag", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Customer_Party_Api
                            .FromSqlRaw(@"exec [Customer_Party_API_Select] @API_Id, @User_Pricing_Id, @Map_Flag", _api_Id, _user_pricing_id, _map_Flag)
                            .AsEnumerable()
                            .FirstOrDefault());

            return result;
        }
        #endregion

        #region Customer Column Caption

        public async Task<int> Add_Update_Customer_Column_Caption(DataTable dataTable, int modified_By)
        {
            var _modified_By = new SqlParameter("@User_Id", modified_By);
            var parameter = new SqlParameter("@Customer_Pricing_Column_Caption_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Customer_Pricing_Column_Caption_Table_Type",
                Value = dataTable
            };
            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Create_Update_Customer_Column_Caption  @Customer_Pricing_Column_Caption_Table_Type, @User_Id",
                        parameter, _modified_By));

            return result;
        }
        public async Task<List<Customer_Column_Caption>> Get_Customer_Pricing_Column_Caption(int? user_Pricing_Id, string? map_Flag, string? upload_Method)
        {
            var _user_Pricing_Id = user_Pricing_Id > 0 ? new SqlParameter("@User_Pricing_Id", user_Pricing_Id) : new SqlParameter("@Party_Id", DBNull.Value);
            var _map_Flag = !string.IsNullOrEmpty(map_Flag) ? new SqlParameter("@Map_Flag", map_Flag) : new SqlParameter("@Map_Flag", DBNull.Value);
            var _upload_Method = !string.IsNullOrEmpty(upload_Method) ? new SqlParameter("@Upload_Method", upload_Method) : new SqlParameter("@Upload_Method", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Customer_Column_Caption
                            .FromSqlRaw(@"exec Customer_Pricing_Column_Caption_Select @User_Pricing_Id, @Map_Flag, @Upload_Method", _user_Pricing_Id, _map_Flag, _upload_Method)
                            .AsEnumerable()
                            .ToList());

            return result;
        }

        #endregion

        public async Task<IList<Supplier_Details_List>> Get_Suplier_Detail_List(int party_Id)
        {
            var _party_Id = party_Id > 0 ? new SqlParameter("@Party_Id", party_Id) : new SqlParameter("@Party_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Supplier_Details_List
                            .FromSqlRaw(@"exec Get_Supplier_Details @Party_Id", _party_Id)
                            .ToListAsync());
            return result;
        }
        public async Task<IList<DropdownModel>> Get_Party_Suplier()
        {
            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Get_Party_Supplier")
                            .ToListAsync());
            return result;
        }
        public async Task<IList<DropdownModel>> Get_Party_Suplier_For_Manual_File()
        {
            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Get_Party_Supplier_For_Manual_File")
                            .ToListAsync());
            return result;
        }
        public async Task<IList<DropdownModel>> Get_Party_Type_Courier()
        {
            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Get_Party_Type_Courier")
                            .ToListAsync());
            return result;
        }
        public async Task<IList<DropdownModel>> Get_Party_Type_Customer()
        {
            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Get_Party_Type_Customer")
                            .ToListAsync());
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Party_Search_Select(string common_Search, int user_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Party_Master_Search_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(common_Search) ? new SqlParameter("@Common_Search", common_Search) : new SqlParameter("@Common_Search", DBNull.Value));
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
    }
}
