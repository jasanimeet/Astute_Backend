using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class PartyService : IPartyService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public PartyService(AstuteDbContext dbContext,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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
            var comp_Bank = party_Master.Comp_Bank > 0 ? new SqlParameter("@Comp_Bank", party_Master.Comp_Bank) : new SqlParameter("@Comp_Bank", DBNull.Value);
            var party_Name = new SqlParameter("@Party_Name", party_Master.Party_Name);
            var ship_PartyId = party_Master.Ship_PartyId > 0 ? new SqlParameter("@Ship_PartyId", party_Master.Ship_PartyId) : new SqlParameter("@Ship_PartyId", DBNull.Value);

            await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC Party_Master_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @Record_Type, @Party_Type, @Adress_1, @Adress_2, @Adress_3,
            @City_Id, @PinCode, @Mobile_1, @Mobile_2, @Phone_1, @Phone_2, @Fax, @Email_1, @Email_2, @Comp_Bank, @Party_Name, @Ship_PartyId", empId, ipaddress, date, time,
            record_Type, party_Type, party_Code, party_Address1, party_Address2, party_Address3, city_Id, pin_Code, mobile_No1, mobile_No2, phone_No1, phone_No2, fax,
            email_1, email_2, comp_Bank, party_Name, ship_PartyId));
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
        public async Task<IList<Party_Master>> GetParty(int party_Id)
        {
            var partyId = party_Id > 0 ? new SqlParameter("@PartyId", party_Id) : new SqlParameter("@PartyId", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Party_Master
                            .FromSqlRaw(@"exec Party_Master_Select @PartyId", partyId).ToListAsync());

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
            var mobile_No2 = !string.IsNullOrEmpty(party_Master.Mobile_2) ? new SqlParameter("@Mobile_2", party_Master.Mobile_2) : new SqlParameter("@Mobile_2", DBNull.Value);
            var phone_No1 = !string.IsNullOrEmpty(party_Master.Phone_1) ? new SqlParameter("@Phone_1", party_Master.Phone_1) : new SqlParameter("@Phone_1", DBNull.Value);
            var phone_No2 = !string.IsNullOrEmpty(party_Master.Phone_2) ? new SqlParameter("@Phone_2", party_Master.Phone_2) : new SqlParameter("@Phone_2", DBNull.Value);
            var fax = !string.IsNullOrEmpty(party_Master.Fax) ? new SqlParameter("@Fax", party_Master.Fax) : new SqlParameter("@Fax", DBNull.Value);
            var email_1 = !string.IsNullOrEmpty(party_Master.Email_1) ? new SqlParameter("@Email_1", party_Master.Email_1) : new SqlParameter("@Email_1", DBNull.Value);
            var email_2 = !string.IsNullOrEmpty(party_Master.Email_2) ? new SqlParameter("@Email_2", party_Master.Email_2) : new SqlParameter("@Email_2", DBNull.Value);
            var comp_Bank = party_Master.Comp_Bank > 0 ? new SqlParameter("@Comp_Bank", party_Master.Comp_Bank) : new SqlParameter("@Comp_Bank", DBNull.Value);
            var party_Name = new SqlParameter("@Party_Name", party_Master.Party_Name);
            var ship_PartyId = party_Master.Ship_PartyId > 0 ? new SqlParameter("@Ship_PartyId", party_Master.Ship_PartyId) : new SqlParameter("@Ship_PartyId", DBNull.Value);
            var insertedId = new SqlParameter("@InsertedId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_Master_Insert_Update @Party_Id, @Party_Type, @Party_Code, @Adress_1, @Adress_2, @Adress_3, @City_Id, @PinCode, @Mobile_1,
                        @Mobile_2, @Phone_1, @Phone_2, @Fax, @Email_1, @Email_2, @Comp_Bank, @Party_Name, @Ship_PartyId, @InsertedId OUT", party_Id, party_Type, party_Code, party_Address1, party_Address2,
                        party_Address3, city_Id, pin_Code, mobile_No1, mobile_No2, phone_No1, phone_No2, fax, email_1, email_2, comp_Bank, party_Name, ship_PartyId, insertedId));

            if (result > 0)
            {
                string record_Type = string.Empty;
                int _insertedId = (int)insertedId.Value;
                if (CoreService.Enable_Trace_Records(_configuration))
                {
                    if (party_Master.Party_Id == 0)
                        record_Type = "Insert";
                    else
                        record_Type = "Update";
                    await Insert_Party_Master_Trace(party_Master, record_Type);
                }
                return ("success", _insertedId);
            }
            return ("error", 0);
        }
        public async Task<Party_Master> Get_Party_Details(int party_Id)
        {
            var _partyId = party_Id > 0 ? new SqlParameter("@PartyId", party_Id) : new SqlParameter("@PartyId", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Party_Master
                            .FromSqlRaw(@"exec Party_Master_Select @PartyId", _partyId)
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
                }
            }

            return result;
        }
        #endregion

        #region Party API
        public async Task<int> Add_Update_Party_API(Party_Api party_Api)
        {   
            var api_Id = new SqlParameter("@API_Id", party_Api.API_Id);
            var _party_Id = party_Api.Party_Id > 0 ? new SqlParameter("@Party_Id", party_Api.Party_Id) : new SqlParameter("@Party_Id", DBNull.Value);
            var api_Url = !string.IsNullOrEmpty(party_Api.API_URL) ? new SqlParameter("@API_URL", party_Api.API_URL) : new SqlParameter("@API_URL", DBNull.Value);
            var api_User = !string.IsNullOrEmpty(party_Api.API_User) ? new SqlParameter("@API_User", party_Api.API_User) : new SqlParameter("@API_User", DBNull.Value);
            var api_Password = !string.IsNullOrEmpty(party_Api.API_Password) ? new SqlParameter("@API_Password", party_Api.API_Password) : new SqlParameter("@API_Password", DBNull.Value);
            var api_Method = !string.IsNullOrEmpty(party_Api.API_Method) ? new SqlParameter("@API_Method", party_Api.API_Method) : new SqlParameter("@API_Method", DBNull.Value);
            var api_Response = !string.IsNullOrEmpty(party_Api.API_Response) ? new SqlParameter("@API_Response", party_Api.API_Response) : new SqlParameter("@API_Response", DBNull.Value);
            var api_Status = new SqlParameter("@API_Status", party_Api.API_Status);
            var api_Inverse = new SqlParameter("@Disc_Inverse", party_Api.Disc_Inverse);
            var auto_Ref_No = new SqlParameter("@Auto_Ref_No", party_Api.Auto_Ref_No);
            var repeateveryType = !string.IsNullOrEmpty(party_Api.RepeateveryType) ? new SqlParameter("@RepeateveryType", party_Api.RepeateveryType) : new SqlParameter("@RepeateveryType", DBNull.Value);
            var repeatevery = !string.IsNullOrEmpty(party_Api.Repeatevery) ? new SqlParameter("@Repeatevery", party_Api.Repeatevery) : new SqlParameter("@Repeatevery", DBNull.Value);
            var lab = new SqlParameter("@Lab", party_Api.Lab);
            var overseas = new SqlParameter("@Overseas", party_Api.Overseas);
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

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_Api_Insert_Update @API_Id, @Party_Id, @API_URL, @API_User, @API_Password, @API_Method, @API_Response, @API_Status,
                        @Disc_Inverse, @Auto_Ref_No, @RepeateveryType, @repeatevery, @Lab, @Overseas, @Stock_Url, @User_Id, @User_Caption, @Password_Caption, @Action_Caption, @Action_Value,
                        @Action_Caption_1, @Action_Value_1, @Action_Caption_2, @Action_Value_2, @Short_Code, @Stock_Api_Method, @Method_Type", api_Id, _party_Id, api_Url, api_User, api_Password, api_Method, api_Response, api_Status, 
                        api_Inverse, auto_Ref_No, repeateveryType, repeatevery, lab, overseas, stock_Url, user_Id, user_Caption, password_Caption, action_Caption, action_Value, action_Caption_1, 
                        action_Value_1, action_Caption_2, action_Value_2, short_Code, stock_Api_Method, method_Type));

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
        #endregion

        #region Party FTP
        public async Task<int> Add_Update_Party_FTP(Party_FTP party_FTP)
        {   
            var ftp_Id = new SqlParameter("@FTP_Id", party_FTP.FTP_Id);
            var _party_Id = party_FTP.Party_Id > 0 ? new SqlParameter("@Party_Id", party_FTP.Party_Id) : new SqlParameter("@Party_Id", DBNull.Value);
            var host = !string.IsNullOrEmpty(party_FTP.Host) ? new SqlParameter("@Host", party_FTP.Host) : new SqlParameter("@Host", DBNull.Value);
            var ftp_Port = party_FTP.Ftp_Port > 0 ? new SqlParameter("@Ftp_Port", party_FTP.Ftp_Port) : new SqlParameter("@Ftp_Port", DBNull.Value);
            var ftp_User = !string.IsNullOrEmpty(party_FTP.Ftp_User) ? new SqlParameter("@Ftp_User", party_FTP.Ftp_User) : new SqlParameter("@Ftp_User", DBNull.Value);
            var ftp_Pasword = !string.IsNullOrEmpty(party_FTP.Ftp_Password) ? new SqlParameter("@Ftp_Password", party_FTP.Ftp_Password) : new SqlParameter("@Ftp_Password", DBNull.Value);
            var ftp_File_Name = !string.IsNullOrEmpty(party_FTP.Ftp_File_Name) ? new SqlParameter("@Ftp_File_Name", party_FTP.Ftp_File_Name) : new SqlParameter("@Ftp_File_Name", DBNull.Value);
            var ftp_File_Type = !string.IsNullOrEmpty(party_FTP.Ftp_File_Type) ? new SqlParameter("@Ftp_File_Type", party_FTP.Ftp_File_Type) : new SqlParameter("@Ftp_File_Type", DBNull.Value);
            var api_Inverse = new SqlParameter("@Disc_Inverse", party_FTP.Disc_Inverse);
            var auto_Ref_No = new SqlParameter("@Auto_Ref_No", party_FTP.Auto_Ref_No);
            var repeateveryType = !string.IsNullOrEmpty(party_FTP.RepeateveryType) ? new SqlParameter("@RepeateveryType", party_FTP.RepeateveryType) : new SqlParameter("@RepeateveryType", DBNull.Value);
            var repeatevery = !string.IsNullOrEmpty(party_FTP.Repeatevery) ? new SqlParameter("@Repeatevery", party_FTP.Repeatevery) : new SqlParameter("@Repeatevery", DBNull.Value);
            var lab = new SqlParameter("@Lab", party_FTP.Lab);
            var overseas = new SqlParameter("@Overseas", party_FTP.Overseas);
            var secure_Ftp = new SqlParameter("@Secure_Ftp", party_FTP.Secure_Ftp);
            var short_Code = !string.IsNullOrEmpty(party_FTP.Short_Code) ? new SqlParameter("@Short_Code", party_FTP.Short_Code) : new SqlParameter("@Short_Code", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_FTP_Insert_Update @FTP_Id, @Party_Id, @Host, @Ftp_Port, @Ftp_User, @Ftp_Password, @Ftp_File_Name, @Ftp_File_Type,
                        @Disc_Inverse, @Auto_Ref_No, @RepeateveryType, @Repeatevery, @Lab, @Overseas, @Secure_Ftp, @Short_Code", ftp_Id, _party_Id, host, ftp_Port, ftp_User, ftp_Pasword, ftp_File_Name, ftp_File_Type, 
                        api_Inverse, auto_Ref_No, repeateveryType, repeatevery, lab, overseas, secure_Ftp, short_Code));

            return result;
        }
        public async Task<int> Delete_Party_FTP(int ftp_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Party_FTP_Delete {ftp_Id}"));
        }
        public async Task<Party_FTP> Get_Party_FTP(int ftp_Id, int party_Id)
        {   
            var _ftp_Id = ftp_Id > 0 ? new SqlParameter("@FTP_Id", party_Id) : new SqlParameter("@FTP_Id", DBNull.Value);
            var _party_Id = party_Id > 0 ? new SqlParameter("@Party_Id", party_Id) : new SqlParameter("@Party_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Party_FTP
                            .FromSqlRaw(@"exec Party_FTP_Select @FTP_Id, @Party_Id", _ftp_Id, _party_Id)
                            .AsEnumerable()
                            .FirstOrDefault());

            return result;
        }
        #endregion

        #region Party File
        public async Task<int> Add_Update_Party_File(Party_File party_File)
        {   
            var file_Id = new SqlParameter("@File_Id", party_File.File_Id);
            var _party_Id = party_File.Party_Id > 0 ? new SqlParameter("@Party_Id", party_File.Party_Id) : new SqlParameter("@Party_Id", DBNull.Value);
            var file_Location = !string.IsNullOrEmpty(party_File.File_Location) ? new SqlParameter("@File_Location", party_File.File_Location) : new SqlParameter("@File_Location", DBNull.Value);
            var file_Type = !string.IsNullOrEmpty(party_File.File_Type) ? new SqlParameter("@File_Type", party_File.File_Type) : new SqlParameter("@File_Type", DBNull.Value);
            var offer_Name = !string.IsNullOrEmpty(party_File.Offer_Name) ? new SqlParameter("@Offer_Name", party_File.Offer_Name) : new SqlParameter("@Offer_Name", DBNull.Value);
            var api_Inverse = new SqlParameter("@Disc_Inverse", party_File.Disc_Inverse);
            var auto_Ref_No = new SqlParameter("@Auto_Ref_No", party_File.Auto_Ref_No);
            var file_Location_1 = !string.IsNullOrEmpty(party_File.File_Location_1) ? new SqlParameter("@File_Location_1", party_File.File_Location_1) : new SqlParameter("@File_Location_1", DBNull.Value);
            var file_Type_1 = !string.IsNullOrEmpty(party_File.File_Type_1) ? new SqlParameter("@File_Type_1", party_File.File_Type_1) : new SqlParameter("@File_Type_1", DBNull.Value);
            var file_Location_2 = !string.IsNullOrEmpty(party_File.File_Location_2) ? new SqlParameter("@File_Location_2", party_File.File_Location_2) : new SqlParameter("@File_Location_2", DBNull.Value);
            var file_Type_2 = !string.IsNullOrEmpty(party_File.File_Type_2) ? new SqlParameter("@File_Type_2", party_File.File_Type_2) : new SqlParameter("@File_Type_2", DBNull.Value);
            var repeateveryType = !string.IsNullOrEmpty(party_File.RepeateveryType) ? new SqlParameter("@RepeateveryType", party_File.RepeateveryType) : new SqlParameter("@RepeateveryType", DBNull.Value);
            var repeatevery = !string.IsNullOrEmpty(party_File.Repeatevery) ? new SqlParameter("@Repeatevery", party_File.Repeatevery) : new SqlParameter("@Repeatevery", DBNull.Value);
            var short_Code = !string.IsNullOrEmpty(party_File.Short_Code) ? new SqlParameter("@Short_Code", party_File.Short_Code) : new SqlParameter("@Short_Code", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_File_Insert_Update @File_Id, @Party_Id, @File_Location, @File_Type, @Offer_Name, @Disc_Inverse, @Auto_Ref_No,
                        @File_Location_1, @File_Type_1, @File_Location_2, @File_Type_2, @RepeateveryType, @Repeatevery, @Short_Code", file_Id, _party_Id, file_Location, file_Type, offer_Name, api_Inverse, auto_Ref_No,
                        file_Location_1, file_Type_1, file_Location_2, file_Type_2, repeateveryType, repeatevery, short_Code));

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
        public async Task<IList<DropdownModel>> Get_Party_Type_Courier()
        {
            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Get_Party_Type_Courier")
                            .ToListAsync());
            return result;
        }
        public async Task<int> Add_Update_Supplier_Column_Mapping(DataTable dataTable)
        {
            var parameter = new SqlParameter("@supplier_column", SqlDbType.Structured)
            {
                TypeName = "dbo.Supplier_Column_Mapping_Data_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Supplier_Column_Mapping_Insert_Update @supplier_column", parameter));
            return result;
        }
        public async Task<IList<Supplier_Column_Mapping>> Get_Supplier_Column_Mapping(int supp_Id)
        {
            var _supp_Id = new SqlParameter("@Supp_Id", supp_Id);

            var result = await Task.Run(() => _dbContext.Supplier_Column_Mapping
                            .FromSqlRaw(@"exec Supplier_Column_Mapping_Select @Supp_Id", _supp_Id)
                            .ToListAsync());
            return result;
        }
    }
}
