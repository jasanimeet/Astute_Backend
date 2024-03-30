using astute.Models;
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
        Task<IList<Party_Master_Replica>> GetParty_Raplicate(int party_Id, string party_Type);
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
        #endregion

        #region Customer Party File
        Task<int> Add_Update_Customer_Party_File(Customer_Party_File party_File, int modified_By);
        Task<int> Delete_Customer_Party_File(int file_Id);
        Task<Party_File> Get_Customer_Party_File(int file_Id, int party_Id);
        #endregion

        #region Customer Party FTP
        Task<int> Add_Update_Customer_Party_FTP(Customer_Party_FTP party_FTP, int modified_By);
        Task<int> Delete_Customer_Party_FTP(int ftp_Id);
        Task<Customer_Party_FTP> Get_Customer_Party_FTP(int ftp_Id, int party_Id);
        #endregion

        #region Customer Column Caption
        Task<int> Add_Update_Customer_Column_Caption(DataTable dataTable, int UserId);
        Task<List<Dictionary<string, object>>> Get_Customer_Pricing_Column_Caption();
        #endregion
        Task<IList<Supplier_Details_List>> Get_Suplier_Detail_List(int party_Id);
        Task<IList<DropdownModel>> Get_Party_Suplier();
        Task<IList<DropdownModel>> Get_Party_Suplier_For_Manual_File();
        Task<IList<DropdownModel>> Get_Party_Type_Courier();
        Task<IList<DropdownModel>> Get_Party_Type_Customer();
        Task<List<Dictionary<string, object>>> Get_Party_Search_Select(string common_Search, int user_Id);
    }
}
