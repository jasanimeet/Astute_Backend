﻿using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IPartyService
    {
        #region Utilitie
        Task Insert_Party_Contact_Trace(DataTable dataTable);
        Task Insert_Party_Bank_Trace(DataTable dataTable);
        Task Insert_Party_Document_Trace(DataTable dataTable);
        Task Insert_Party_Assist_Trace(DataTable dataTable);
        Task Insert_Party_Shipping_Trace(DataTable dataTable);
        Task Insert_Party_Media_Trace(DataTable dataTable);
        Task Insert_Party_Print_Trace(DataTable dataTable);
        #endregion

        #region Party Master        
        Task<(string, int)> DeleteParty(int party_Id);
        Task<List<Dictionary<string, object>>> GetParty(int party_Id, string party_Type);
        Task<List<Dictionary<string, object>>> GetPartyName(int party_Id);
        Task<IList<Party_Master_Replica>> GetPartyReplicateFromCache(int partyId, string partyType, int page_Size, int Page_No);
        Task<IList<Party_Master_Replica>> GetPartyFromCache(int partyId, string partyType, int company_Code);
        Task<IList<Party_Master_Replica>> GetParty_Raplicate(int party_Id, string party_Type, int company_Code);
        Task<IList<Party_Master_Replica>> GetParty_Raplicate_08052024(int party_Id, string party_Type, int page_Size, int Page_No);
        //Task<List<Dictionary<string, object>>> GetParty(int party_Id, string party_Type);
        Task<List<Dictionary<string, object>>> GetPartyCustomer(int party_Id, string party_Type);
        #endregion

        #region Party Contact
        Task<int> AddUpdatePartyContact(DataTable dataTable);
        Task<IList<DropdownModel>> Get_User_Name_From_Party_Contact(int party_Id);
        #endregion

        #region Party Bank
        Task<int> AddUpdatePartyBank(DataTable dataTable);
        Task<int> PartyBankChangeStatus(int account_Id, bool status);
        #endregion

        #region Party Shipping
        Task<int> AddUpdatePartyShipping(DataTable dataTable);
        Task<int> PartyShippingChangeStatus(int ship_Id, bool status);
        #endregion

        #region Party Document
        Task<int> AddUpdatePartyDocument(DataTable dataTable);
        #endregion

        #region Party Assist
        Task<int> AddUpdatePartyAssist(DataTable dataTable);
        #endregion

        #region Party Media
        Task<int> AddUpdatePartyMedia(DataTable dataTable);
        #endregion
        #region Party Print Process
        Task<int> Add_Update_Party_Print_Process(DataTable dataTable);
        #endregion

        #region Party QC Criteria
        Task<int> Add_Update_Party_QcCriteria(DataTable dataTable);
        #endregion

        #region Party Details
        Task<(string, int)> Add_Update_Party(Party_Master party_Master);
        Task<Party_Master> Get_Party_Details(int party_Id);
        Task<int> Get_Party_Code();
        Task<int> Party_Master_Change_Status(int party_Id, bool status);
        #endregion

        #region Party API
        Task<int> Add_Update_Party_API(Party_Api party_Api, int modified_By);
        Task<int> Delete_Party_API(int api_Id);
        Task<Party_Api> Get_Party_API(int api_Id, int party_Id);
        Task<IList<Party_API_With_Column_Mapping>> Get_Party_API_With_Column_Mapping();
        Task<List<Dictionary<string, object>>> Common_Funtion_To_Get_Supp_Col_Map(int supp_Id, string upload_Type);
        #endregion

        #region Party FTP
        Task<int> Add_Update_Party_FTP(Party_FTP party_FTP, int modified_By);
        Task<int> Delete_Party_FTP(int ftp_Id);
        Task<Party_FTP> Get_Party_FTP(int ftp_Id, int party_Id);
        #endregion

        #region Party File
        Task<int> Add_Update_Party_File(Party_File party_File, int modified_By);
        Task<int> Delete_Party_File(int file_Id);
        Task<Party_File> Get_Party_File(int file_Id, int party_Id);
        Task<Party_File_Detail> Get_Party_File_Detail(int party_Id);
        #endregion

        #region Customer Party File
        Task<int> Add_Update_Customer_Party_File(Customer_Party_File party_File, int modified_By, string map_Flag);
        Task<int> Delete_Customer_Party_File(int file_Id);
        Task<Customer_Party_File> Get_Customer_Party_File(int? file_Id, int party_Id, string map_Flag);
        #endregion

        #region Customer Party FTP
        Task<int> Add_Update_Customer_Party_FTP(Customer_Party_FTP party_FTP, int modified_By, string map_Flag);
        Task<int> Delete_Customer_Party_FTP(int ftp_Id);
        Task<Customer_Party_FTP> Get_Customer_Party_FTP(int? ftp_Id, int user_Pricing_Id, string map_Flag);
        #endregion

        #region Customer Party API
        Task<int> Add_Update_Customer_Party_API(Customer_Party_Api party_API, int modified_By, string map_Flag);
        Task<int> Delete_Customer_Part_API(int api_Id);
        Task<Customer_Party_Api> Get_Customer_Party_API(int? api_Id, int user_Pricing_Id, string map_Flag);
        #endregion

        #region Customer Column Caption
        Task<int> Add_Update_Customer_Column_Caption(DataTable dataTable, int UserId);
        Task<List<Customer_Column_Caption>> Get_Customer_Pricing_Column_Caption(int? user_Pricing_Id, string? map_Flag, string? upload_Method);
        #endregion
        #region Party

        Task<IList<Supplier_Details_List>> Get_Supplier_Detail_List(int party_Id);
        Task<IList<DropdownModel>> Get_Party_Suplier();
        Task<IList<DropdownModel>> Get_All_Party_Supplier();
        Task<IList<DropdownModel>> Get_Party_Url_Format_Supplier();
        Task<IList<DropdownModel>> Get_Party_Suplier_For_Manual_File();
        Task<IList<DropdownModel>> Get_Party_Type_Courier();
        Task<List<Dictionary<string, object>>> Get_Party_Type_Customer(int user_Id);
        Task<List<Dictionary<string, object>>> Get_Party_Search_Select(string common_Search, int user_Id);
        Task<IList<DropdownModel>> Get_Party_Type_Suplier();
        Task<(List<Dictionary<string, object>>, string)> Get_Notification(int? User_Id);
        Task<int> Notification_Master_Update_Read_By(int? Notification_Id, bool? Is_Read, int? User_Id);
        Task<(List<Dictionary<string, object>>, string)> Get_Notification_Menu(int? User_Id);
        Task<List<Dictionary<string, object>>> Get_Notification_Menu_QC_Reply_Pending(int? User_Id);
        Task<List<Dictionary<string, object>>> Get_Notification_Menu_Select(int? User_Id);
        Task<int> Set_Notification_Menu(DataTable dataTable);
        Task<IList<Supplier_Price_List>> Get_Supplier_Price_List();
        Task<int> Update_Supplier_Price_List(DataTable supplier_Price_Lists);
        Task<List<Dictionary<string, object>>> Get_Supplier_Special_Price_Validity(int party_Id);
        Task<int> Party_Api_Ftp_File_Price_Lock();
        Task<List<Dictionary<string, object>>> Party_Api_Ftp_File_Price_Lock_List();
        Task<int> Update_Supplier_Price_Lock_List(DataTable supplier_Price_Lists);
        #endregion

        #region Hold

        Task<IList<PartyMasterDrop>> Get_Party();

        Task<IList<DropdownModel>> Get_Party_Assist(int Party_Code);

        #endregion

        #region Send Mail for Supplier Upload Stock

        Task<DataTable> Get_Supplier_Stock_Upload_Status();

        #endregion

        #region Connect GIA Report Layout Save

        Task<List<Report_Layout_Save>> Get_Connect_GIA_Result_Layout(int User_Id, int Rm_Id);
        Task<List<Report_Layout_Save_Detail>> Get_Connect_GIA_Result_Column_Caption(int? user_Pricing_Id);

        #endregion

        #region Quotation Print Detail
        Task<List<Dictionary<string, object>>> Get_Quotation_BillParty_Detail(int party_Id);
        #endregion
        #region Process Margin Master
        Task<List<Dictionary<string, object>>> Process_Margin_Master();
        Task<int> Delete_Process_Margin_Master(int id);
        Task<(string, int)> Insert_Update_Process_Margin_Master(DataTable dataTable, int user_Id);
        #endregion

        #region Manual Url Transfer
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Manual_Url_Transfer(DataTable dataTable, int user_Id);
        Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Manual_Url_Transfer(DataTable dataTable);
        Task<int> Insert_Update_Manual_Url_Transfer(DataTable dataTable, int user_Id);
        #endregion

        #region RFID No
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_RFID_No(DataTable dataTable, int user_Id);
        Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_RFID_No(DataTable dataTable);
        Task<int> Update_Purchase_Detail_RFID_No(DataTable dataTable, int user_Id);
        #endregion
    }
}
