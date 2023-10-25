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
            var mobile_No2 = !string.IsNullOrEmpty(party_Master.Mobile_2) ? new SqlParameter("@Mobile_2", party_Master.Mobile_2) : new SqlParameter("@Mobile_2", DBNull.Value);
            var phone_No1 = !string.IsNullOrEmpty(party_Master.Phone_1) ? new SqlParameter("@Phone_1", party_Master.Phone_1) : new SqlParameter("@Phone_1", DBNull.Value);
            var phone_No2 = !string.IsNullOrEmpty(party_Master.Phone_2) ? new SqlParameter("@Phone_2", party_Master.Phone_2) : new SqlParameter("@Phone_2", DBNull.Value);
            var fax = !string.IsNullOrEmpty(party_Master.Fax) ? new SqlParameter("@Fax", party_Master.Fax) : new SqlParameter("@Fax", DBNull.Value);
            var email_1 = !string.IsNullOrEmpty(party_Master.Email_1) ? new SqlParameter("@Email_1", party_Master.Email_1) : new SqlParameter("@Email_1", DBNull.Value);
            var email_2 = !string.IsNullOrEmpty(party_Master.Email_2) ? new SqlParameter("@Email_2", party_Master.Email_2) : new SqlParameter("@Email_2", DBNull.Value);
            var comp_Bank = party_Master.Comp_Bank > 0 ? new SqlParameter("@Comp_Bank", party_Master.Comp_Bank) : new SqlParameter("@Comp_Bank", DBNull.Value);
            var party_Name = new SqlParameter("@Party_Name", party_Master.Party_Name);
            var ship_PartyId = party_Master.Ship_PartyId > 0 ? new SqlParameter("@Ship_PartyId", party_Master.Ship_PartyId) : new SqlParameter("@Ship_PartyId", DBNull.Value);
            var final_Customer_Id = party_Master.Final_Customer_Id > 0 ? new SqlParameter("@Final_Customer_Id", party_Master.Final_Customer_Id) : new SqlParameter("@Final_Customer_Id", DBNull.Value);
            var website = !string.IsNullOrEmpty(party_Master.Website) ? new SqlParameter("@Website", party_Master.Website) : new SqlParameter("@Website", DBNull.Value);
            var bank_currency = !string.IsNullOrEmpty(party_Master.Bank_Currency) ? new SqlParameter("@Bank_Currency", party_Master.Bank_Currency) : new SqlParameter("@Bank_Currency", DBNull.Value);
            var payment_Terms = party_Master.Payment_Terms > 0 ? new SqlParameter("@Payment_Terms", party_Master.Payment_Terms) : new SqlParameter("@Payment_Terms", DBNull.Value);
            var cust_Freight_Account_No = !string.IsNullOrEmpty(party_Master.Cust_Freight_Account_No) ? new SqlParameter("@Cust_Freight_Account_No", party_Master.Cust_Freight_Account_No) : new SqlParameter("@Cust_Freight_Account_No", DBNull.Value);
            var alias_Name = !string.IsNullOrEmpty(party_Master.Alias_Name) ? new SqlParameter("@Alias_Name", party_Master.Alias_Name) : new SqlParameter("@Alias_Name", DBNull.Value);
            var wechat_ID = !string.IsNullOrEmpty(party_Master.Wechat_ID) ? new SqlParameter("@Wechat_ID", party_Master.Wechat_ID) : new SqlParameter("@Wechat_ID", DBNull.Value);
            var skype_ID = !string.IsNullOrEmpty(party_Master.Skype_ID) ? new SqlParameter("@Skype_ID", party_Master.Skype_ID) : new SqlParameter("@Skype_ID", DBNull.Value);
            var business_Reg_No = !string.IsNullOrEmpty(party_Master.Business_Reg_No) ? new SqlParameter("@Business_Reg_No", party_Master.Business_Reg_No) : new SqlParameter("@Business_Reg_No", DBNull.Value);
            var default_Remarks = party_Master.Default_Remarks > 0 ? new SqlParameter("@Default_Remarks", party_Master.Default_Remarks) : new SqlParameter("@Default_Remarks", DBNull.Value);
            var notification = !string.IsNullOrEmpty(party_Master.Notification) ? new SqlParameter("@Notification", party_Master.Notification) : new SqlParameter("@Notification", DBNull.Value);
            var reference_By = !string.IsNullOrEmpty(party_Master.Reference_By) ? new SqlParameter("@Reference_By", party_Master.Reference_By) : new SqlParameter("@Reference_By", DBNull.Value);
            var tIN_No = !string.IsNullOrEmpty(party_Master.TIN_No) ? new SqlParameter("@TIN_No", party_Master.TIN_No) : new SqlParameter("@TIN_No", DBNull.Value);
            var insertedId = new SqlParameter("@InsertedId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_Master_Insert_Update @Party_Id, @Party_Type, @Party_Code, @Adress_1, @Adress_2, @Adress_3, @City_Id, @PinCode, @Mobile_1,
                        @Mobile_2, @Phone_1, @Phone_2, @Fax, @Email_1, @Email_2, @Comp_Bank, @Party_Name, @Ship_PartyId, @Final_Customer_Id, @Website, @Bank_Currency, @Payment_Terms,
                        @Cust_Freight_Account_No, @Alias_Name, @Wechat_ID, @Skype_ID, @Business_Reg_No, @Default_Remarks, @Notification, @Reference_By, @TIN_No, @InsertedId OUT", party_Id,
                        party_Type, party_Code, party_Address1, party_Address2, party_Address3, city_Id, pin_Code, mobile_No1, mobile_No2, phone_No1, phone_No2, fax, email_1, email_2, 
                        comp_Bank, party_Name, ship_PartyId, final_Customer_Id, website, bank_currency, payment_Terms, cust_Freight_Account_No, alias_Name, wechat_ID, skype_ID, business_Reg_No,
                        notification, reference_By, tIN_No, insertedId));

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

                    result.Party_Media_List = await Task.Run(() => _dbContext.Party_Media
                                            .FromSqlRaw(@"exec Party_Media_Select @PartyId", _partyId).ToListAsync());
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
            var format = !string.IsNullOrEmpty(party_Api.Format) ? new SqlParameter("@Format", party_Api.Format) : new SqlParameter("@Format", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_Api_Insert_Update @API_Id, @Party_Id, @API_URL, @API_User, @API_Password, @API_Method, @API_Response, @API_Status,
                        @Disc_Inverse, @Auto_Ref_No, @RepeateveryType, @repeatevery, @Lab, @Overseas, @Stock_Url, @User_Id, @User_Caption, @Password_Caption, @Action_Caption, 
                        @Action_Value,@Action_Caption_1, @Action_Value_1, @Action_Caption_2, @Action_Value_2, @Short_Code, @Stock_Api_Method, @Method_Type, @Format", api_Id, 
                        _party_Id, api_Url, api_User, api_Password, api_Method, api_Response, api_Status, api_Inverse, auto_Ref_No, repeateveryType, repeatevery, lab, overseas, 
                        stock_Url, user_Id, user_Caption, password_Caption, action_Caption, action_Value, action_Caption_1, action_Value_1, action_Caption_2, action_Value_2, 
                        short_Code, stock_Api_Method, method_Type, format));

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
            var api_Inverse = new SqlParameter("@Disc_Inverse", party_FTP.Disc_Inverse ?? false);
            var auto_Ref_No = new SqlParameter("@Auto_Ref_No", party_FTP.Auto_Ref_No ?? false);
            var repeateveryType = !string.IsNullOrEmpty(party_FTP.RepeateveryType) ? new SqlParameter("@RepeateveryType", party_FTP.RepeateveryType) : new SqlParameter("@RepeateveryType", DBNull.Value);
            var repeatevery = !string.IsNullOrEmpty(party_FTP.Repeatevery) ? new SqlParameter("@Repeatevery", party_FTP.Repeatevery) : new SqlParameter("@Repeatevery", DBNull.Value);
            var lab = new SqlParameter("@Lab", party_FTP.Lab ?? false);
            var overseas = new SqlParameter("@Overseas", party_FTP.Overseas ?? false);
            var secure_Ftp = new SqlParameter("@Secure_Ftp", party_FTP.Secure_Ftp ?? false);
            var short_Code = !string.IsNullOrEmpty(party_FTP.Short_Code) ? new SqlParameter("@Short_Code", party_FTP.Short_Code) : new SqlParameter("@Short_Code", DBNull.Value);
            var format = !string.IsNullOrEmpty(party_FTP.Format) ? new SqlParameter("@Format", party_FTP.Format) : new SqlParameter("@Format", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_FTP_Insert_Update @FTP_Id, @Party_Id, @Host, @Ftp_Port, @Ftp_User, @Ftp_Password, @Ftp_File_Name, @Ftp_File_Type,
                        @Disc_Inverse, @Auto_Ref_No, @RepeateveryType, @Repeatevery, @Lab, @Overseas, @Secure_Ftp, @Short_Code, @Format", ftp_Id, _party_Id, host, ftp_Port, 
                        ftp_User, ftp_Pasword, ftp_File_Name, ftp_File_Type, api_Inverse, auto_Ref_No, repeateveryType, repeatevery, lab, overseas, secure_Ftp, short_Code, format));

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
        public async Task<IList<DropdownModel>> Get_Party_Type_Customer()
        {
            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Get_Party_Type_Customer")
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
        public async Task<IList<Supplier_Column_Mapping>> Get_Supplier_Column_Mapping(int supp_Id, string map_Flag)
        {
            var _supp_Id = supp_Id > 0 ? new SqlParameter("@Supp_Id", supp_Id) : new SqlParameter("@Supp_Id", DBNull.Value);
            var _map_Flag = !string.IsNullOrEmpty(map_Flag) ? new SqlParameter("@Map_Flag", map_Flag) : new SqlParameter("@Map_Flag", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Supplier_Column_Mapping
                            .FromSqlRaw(@"exec Supplier_Column_Mapping_Select @Supp_Id, @Map_Flag", _supp_Id, _map_Flag)
                            .ToListAsync());
            return result;
        }

        #region Supplier Pricing
        public async Task<List<Dictionary<string, object>>> Get_Supplier_Pricing(int supplier_Pricing_Id, int supplier_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Supplier_Pricing_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Supplier_Pricing_Id", supplier_Pricing_Id));
                    command.Parameters.Add(new SqlParameter("@Supplier_Id", supplier_Id));

                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    var dataTable =  ds.Tables[ds.Tables.Count - 1];

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
        public async Task<int> Add_Update_Supplier_Pricing(Supplier_Pricing supplier_Pricing)
        {   
            var supplier_Pricing_Id = new SqlParameter("@Supplier_Pricing_Id", supplier_Pricing.Supplier_Pricing_Id);
            var supplier_Id = supplier_Pricing.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Pricing.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
            var shape = !string.IsNullOrEmpty(supplier_Pricing.Shape) ? new SqlParameter("@Shape", supplier_Pricing.Shape) : new SqlParameter("@Shape", DBNull.Value);
            var cts = !string.IsNullOrEmpty(supplier_Pricing.Cts) ? new SqlParameter("@Cts", supplier_Pricing.Cts) : new SqlParameter("@Cts", DBNull.Value);
            var color = !string.IsNullOrEmpty(supplier_Pricing.Color) ? new SqlParameter("@Color", supplier_Pricing.Color) : new SqlParameter("@Color", DBNull.Value);
            var clarity = !string.IsNullOrEmpty(supplier_Pricing.Clarity) ? new SqlParameter("@Clarity", supplier_Pricing.Clarity) : new SqlParameter("@Clarity", DBNull.Value);
            var cut = !string.IsNullOrEmpty(supplier_Pricing.Cut) ? new SqlParameter("@Cut", supplier_Pricing.Cut) : new SqlParameter("@Cut", DBNull.Value);
            var polish = !string.IsNullOrEmpty(supplier_Pricing.Polish) ? new SqlParameter("@Polish", supplier_Pricing.Polish) : new SqlParameter("@Polish", DBNull.Value);
            var symm = !string.IsNullOrEmpty(supplier_Pricing.Symm) ? new SqlParameter("@Symm", supplier_Pricing.Symm) : new SqlParameter("@Symm", DBNull.Value);
            var flour = !string.IsNullOrEmpty(supplier_Pricing.Flour) ? new SqlParameter("@Flour", supplier_Pricing.Flour) : new SqlParameter("@Flour", DBNull.Value);
            var lab = !string.IsNullOrEmpty(supplier_Pricing.Lab) ? new SqlParameter("@Lab", supplier_Pricing.Lab) : new SqlParameter("@Lab", DBNull.Value);
            var shade = !string.IsNullOrEmpty(supplier_Pricing.Shade) ? new SqlParameter("@Shade", supplier_Pricing.Shade) : new SqlParameter("@Shade", DBNull.Value);
            var luster = !string.IsNullOrEmpty(supplier_Pricing.Luster) ? new SqlParameter("@Luster", supplier_Pricing.Luster) : new SqlParameter("@Luster", DBNull.Value);
            var location = !string.IsNullOrEmpty(supplier_Pricing.Location) ? new SqlParameter("@Location", supplier_Pricing.Location) : new SqlParameter("@Location", DBNull.Value);
            var status = !string.IsNullOrEmpty(supplier_Pricing.Status) ? new SqlParameter("@Status", supplier_Pricing.Status) : new SqlParameter("@Status", DBNull.Value);
            var base_Disc_Per = !string.IsNullOrEmpty(supplier_Pricing.Base_Disc_Per) ? new SqlParameter("@Base_Disc_Per", supplier_Pricing.Base_Disc_Per) : new SqlParameter("@Base_Disc_Per", DBNull.Value);
            var base_Amt = !string.IsNullOrEmpty(supplier_Pricing.Base_Amt) ? new SqlParameter("@Base_Amt", supplier_Pricing.Base_Amt) : new SqlParameter("@Base_Amt", DBNull.Value);
            var final_Disc_Per = !string.IsNullOrEmpty(supplier_Pricing.Final_Disc_Per) ? new SqlParameter("@Final_Disc_Per", supplier_Pricing.Final_Disc_Per) : new SqlParameter("@Final_Disc_Per", DBNull.Value);
            var final_Amt = !string.IsNullOrEmpty(supplier_Pricing.Final_Amt) ? new SqlParameter("@Final_Amt", supplier_Pricing.Final_Amt) : new SqlParameter("@Final_Amt", DBNull.Value);
            var supplier_Filter_Type = !string.IsNullOrEmpty(supplier_Pricing.Supplier_Filter_Type) ? new SqlParameter("@Supplier_Filter_Type", supplier_Pricing.Supplier_Filter_Type) : new SqlParameter("@Supplier_Filter_Type", DBNull.Value);
            var calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.Calculation_Type) ? new SqlParameter("@Calculation_Type", supplier_Pricing.Calculation_Type) : new SqlParameter("@Calculation_Type", DBNull.Value);
            var sign = !string.IsNullOrEmpty(supplier_Pricing.Sign) ? new SqlParameter("@Sign", supplier_Pricing.Sign) : new SqlParameter("@Sign", DBNull.Value);
            var value_1 = supplier_Pricing.Value_1 > 0 ? new SqlParameter("@Value_1", supplier_Pricing.Value_1) : new SqlParameter("@Value_1", DBNull.Value);
            var value_2 = supplier_Pricing.Value_2 > 0 ? new SqlParameter("@Value_2", supplier_Pricing.Value_2) : new SqlParameter("@Value_2", DBNull.Value);
            var value_3 = supplier_Pricing.Value_3 > 0 ? new SqlParameter("@Value_3", supplier_Pricing.Value_3) : new SqlParameter("@Value_3", DBNull.Value);
            var value_4 = supplier_Pricing.Value_4 > 0 ? new SqlParameter("@Value_4", supplier_Pricing.Value_4) : new SqlParameter("@Value_4", DBNull.Value);
            var sp_calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.SP_Calculation_Type) ? new SqlParameter("@SP_Calculation_Type", supplier_Pricing.SP_Calculation_Type) : new SqlParameter("@SP_Calculation_Type", DBNull.Value);
            var sp_sign = !string.IsNullOrEmpty(supplier_Pricing.SP_Sign) ? new SqlParameter("@SP_Sign", supplier_Pricing.SP_Sign) : new SqlParameter("@SP_Sign", DBNull.Value);
            var sp_start_date = !string.IsNullOrEmpty(supplier_Pricing.SP_Start_Date) ? new SqlParameter("@SP_Start_Date", supplier_Pricing.SP_Start_Date) : new SqlParameter("@SP_Start_Date", DBNull.Value);
            var sp_start_time = !string.IsNullOrEmpty(supplier_Pricing.SP_Start_Time) ? new SqlParameter("@SP_Start_Time", supplier_Pricing.SP_Start_Time) : new SqlParameter("@SP_Start_Time", DBNull.Value);
            var sp_end_date = !string.IsNullOrEmpty(supplier_Pricing.SP_End_Date) ? new SqlParameter("@SP_End_Date", supplier_Pricing.SP_End_Date) : new SqlParameter("@SP_End_Date", DBNull.Value);
            var sp_end_time = !string.IsNullOrEmpty(supplier_Pricing.SP_End_Time) ? new SqlParameter("@SP_End_Time", supplier_Pricing.SP_End_Time) : new SqlParameter("@SP_End_Time", DBNull.Value);
            var sp_value_1 = supplier_Pricing.SP_Value_1 > 0 ? new SqlParameter("@SP_Value_1", supplier_Pricing.SP_Value_1) : new SqlParameter("@SP_Value_1", DBNull.Value);
            var sp_value_2 = supplier_Pricing.SP_Value_2 > 0 ? new SqlParameter("@SP_Value_2", supplier_Pricing.SP_Value_2) : new SqlParameter("@SP_Value_2", DBNull.Value);
            var sp_value_3 = supplier_Pricing.SP_Value_3 > 0 ? new SqlParameter("@SP_Value_3", supplier_Pricing.SP_Value_3) : new SqlParameter("@SP_Value_3", DBNull.Value);
            var sp_value_4 = supplier_Pricing.SP_Value_4 > 0 ? new SqlParameter("@SP_Value_4", supplier_Pricing.SP_Value_4) : new SqlParameter("@SP_Value_4", DBNull.Value);
            var ms_calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.MS_Calculation_Type) ? new SqlParameter("@MS_Calculation_Type", supplier_Pricing.MS_Calculation_Type) : new SqlParameter("@MS_Calculation_Type", DBNull.Value);
            var ms_sign = !string.IsNullOrEmpty(supplier_Pricing.MS_Sign) ? new SqlParameter("@MS_Sign", supplier_Pricing.MS_Sign) : new SqlParameter("@MS_Sign", DBNull.Value);
            var ms_value_1 = supplier_Pricing.MS_Value_1 > 0 ? new SqlParameter("@MS_Value_1", supplier_Pricing.MS_Value_1) : new SqlParameter("@MS_Value_1", DBNull.Value);
            var ms_value_2 = supplier_Pricing.MS_Value_2 > 0 ? new SqlParameter("@MS_Value_2", supplier_Pricing.MS_Value_2) : new SqlParameter("@MS_Value_2", DBNull.Value);
            var ms_value_3 = supplier_Pricing.MS_Value_3 > 0 ? new SqlParameter("@MS_Value_3", supplier_Pricing.MS_Value_3) : new SqlParameter("@MS_Value_3", DBNull.Value);
            var ms_value_4 = supplier_Pricing.MS_Value_4 > 0 ? new SqlParameter("@MS_Value_4", supplier_Pricing.MS_Value_4) : new SqlParameter("@MS_Value_4", DBNull.Value);
            var ms_sp_calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Calculation_Type) ? new SqlParameter("@MS_SP_Calculation_Type", supplier_Pricing.MS_SP_Calculation_Type) : new SqlParameter("@MS_SP_Calculation_Type", DBNull.Value);
            var ms_sp_sign = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Sign) ? new SqlParameter("@SP_Sign", supplier_Pricing.MS_SP_Sign) : new SqlParameter("@SP_Sign", DBNull.Value);
            var ms_sp_start_date = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Start_Date) ? new SqlParameter("@MS_SP_Start_Date", supplier_Pricing.MS_SP_Start_Date) : new SqlParameter("@MS_SP_Start_Date", DBNull.Value);
            var ms_sp_start_time = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Start_Time) ? new SqlParameter("@MS_SP_Start_Time", supplier_Pricing.MS_SP_Start_Time) : new SqlParameter("@MS_SP_Start_Time", DBNull.Value);
            var ms_sp_end_date = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_End_Date) ? new SqlParameter("@MS_SP_End_Date", supplier_Pricing.MS_SP_End_Date) : new SqlParameter("@MS_SP_End_Date", DBNull.Value);
            var ms_sp_end_time = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_End_Time) ? new SqlParameter("@MS_SP_End_Time", supplier_Pricing.MS_SP_End_Time) : new SqlParameter("@MS_SP_End_Time", DBNull.Value);
            var ms_sp_value_1 = supplier_Pricing.MS_SP_Value_1 > 0 ? new SqlParameter("@MS_SP_Value_1", supplier_Pricing.MS_SP_Value_1) : new SqlParameter("@MS_SP_Value_1", DBNull.Value);
            var ms_sp_value_2 = supplier_Pricing.MS_SP_Value_2 > 0 ? new SqlParameter("@MS_SP_Value_2", supplier_Pricing.MS_SP_Value_2) : new SqlParameter("@MS_SP_Value_2", DBNull.Value);
            var ms_sp_value_3 = supplier_Pricing.MS_SP_Value_3 > 0 ? new SqlParameter("@MS_SP_Value_3", supplier_Pricing.MS_SP_Value_3) : new SqlParameter("@MS_SP_Value_3", DBNull.Value);
            var ms_sp_value_4 = supplier_Pricing.MS_SP_Value_4 > 0 ? new SqlParameter("@MS_SP_Value_4", supplier_Pricing.MS_SP_Value_4) : new SqlParameter("@MS_SP_Value_4", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Supplier_Pricing_Insert_Update @Supplier_Pricing_Id, @Supplier_Id, @Shape, @Cts, @Color, @Clarity, @Cut, @Polish, @Symm, @Flour, @Lab,
                        @Shade, @Luster, @Location, @Status, @Base_Disc_Per, @Base_Amt, @Final_Disc_Per, @Final_Amt, @Supplier_Filter_Type, @Calculation_Type, @Sign, @Value_1, @Value_2, @Value_3, @Value_4, @SP_Calculation_Type, @SP_Sign,
                        @SP_Start_Date, @SP_Start_Time, @SP_End_Date, @SP_End_Time, @SP_Value_1, @SP_Value_2, @SP_Value_3, @SP_Value_4, @MS_Calculation_Type, @MS_Sign, @MS_Value_1,
                        @MS_Value_2, @MS_Value_3, @MS_Value_4, @MS_SP_Calculation_Type, @MS_SP_Sign, @MS_SP_Start_Date, @MS_SP_Start_Time, @MS_SP_End_Date, @MS_SP_End_Time, @MS_SP_Value_1,
                        @MS_SP_Value_2, @MS_SP_Value_3, @MS_SP_Value_4", supplier_Pricing_Id, supplier_Id, shape, cts, color, clarity, cut, polish, symm, flour, lab, shade, luster,
                        location, status, base_Disc_Per, base_Amt, final_Disc_Per, final_Amt, supplier_Filter_Type, calculation_Type, sign, value_1, value_2, value_3, value_4,
                        sp_calculation_Type, sp_sign, sp_start_date, sp_start_time, sp_end_time, sp_end_time, sp_value_1, sp_value_2, sp_value_3, sp_value_4, ms_calculation_Type,
                        ms_sign, ms_value_1, ms_value_2, ms_value_3, ms_value_4, ms_sp_calculation_Type, ms_sp_sign, ms_sp_start_date, ms_sp_start_time, ms_sp_end_date, ms_sp_end_time, 
                        ms_sp_value_1, ms_sp_value_2, ms_sp_value_3, ms_sp_value_4));

            return result;
        }
        #endregion
    }
}
