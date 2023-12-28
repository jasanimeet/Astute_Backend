using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static astute.Repository.SupplierService;

namespace astute.Repository
{
    public partial interface ISupplierService
    {
        Task<int> Insert_Update_Supplier_Value_Mapping(DataTable dataTable);
        Task<int> InsertSupplierValueMapping(Supplier_Value_Mapping supplier_Value);
        Task<int> UpdateSupplierValueMapping(Supplier_Value_Mapping supplier_Value);
        Task<int> DeleteSupplierValueMapping(int supId);
        Task<IList<Supplier_Value_Mapping>> Get_Supplier_Value_Mapping(int sup_Id, int col_Id);
        Task<int> Add_Update_Supplier_Column_Mapping(DataTable dataTable);
        Task<IList<Supplier_Column_Mapping>> Get_Supplier_Column_Mapping(int supp_Id, string map_Flag, string column_Type);
        Task<DataTable> Get_Supplier_Column_Mapping_In_Datatable(int supp_Id, string map_Flag, string column_Type);

        #region Value Config
        Task<int> Add_Update_Value_Config(Value_Config value_Config);
        Task<int> Delete_Value_Config(int valueMap_ID);
        Task<IList<Value_Config>> Get_Value_Config(int valueMap_ID);
        #endregion

        #region Supplier Pricing
        Task<List<Dictionary<string, object>>> Get_Supplier_Pricing_List();
        Task<List<Dictionary<string, object>>> Get_Sunrise_Pricing_List();
        Task<List<Dictionary<string, object>>> Get_Customer_Pricing_List();
        Task<List<Dictionary<string, object>>> Get_Supplier_Pricing(int supplier_Pricing_Id, int supplier_Id, string supplier_Filter_Type, string map_Flag, int sunrise_pricing_Id, int customer_pricing_Id);
        Task<(string, int)> Add_Update_Supplier_Pricing(Supplier_Pricing supplier_Pricing);
        Task<(string, int)> Delete_Supplier_Pricing(int supplier_Pricing_Id, int supplier_Id);
        Task<int> Delete_Supplier_Pricing_By_Supplier(int supplier_Id);
        Task<Common_Model> Get_Max_Sunrice_Pricing_Id();
        Task<int> Delete_Sunrise_Pricing(int sunrise_Pricing_Id);
        Task<int> Delete_Customer_Pricing(int customer_Pricing_Id);
        #endregion

        #region Supplier Pricing Key To Symbol
        Task<int> Add_Update_Supplier_Pricing_Key_To_Symbol(DataTable dataTable);
        Task<int> Delete_Supplier_Pricing_Key_To_Symbol(int supplier_Pricing_Id);
        #endregion

        #region Supplier Stock
        Task<(string, int)> Stock_Data_Insert_Update(Stock_Data_Master stock_Data_Master);
        Task<(string, int)> Stock_Data_Custom_Insert_Update(Stock_Data_Master_Schedular stock_Data_Master);
        Task<int> Stock_Data_Detail_Insert_Update(DataTable dataTable, int Stock_Data_Id);
        Task<List<Dictionary<string, object>>> Get_Stock_Data_Distinct_Column_Values(string column_Name, int supplier_Id);
        Task<IList<Stock_Data>> Get_Not_Uploaded_Stock_Data(int stock_data_Id);
        Task<int> Supplier_Stock_Insert_Update(int supplier_Id, int stock_Data_Id);
        #endregion

        #region Stock Number Generation
        Task<IList<Stock_Number_Generation>> Get_Stock_Number_Generation(int Id);
        Task<int> Add_Update_Stock_Number_Generation(DataTable dataTable);
        Task<int> Delete_Stock_Number_Generation(int Id);

        #endregion

        #region  Api/FTP/File Party Name
        Task<IList<DropdownModel>> Get_Api_Ftp_File_Party_Select(int party_Id, bool lab, bool overseas, bool is_Stock_Gen);
        #endregion

        #region Supplier Stock Error Log
        Task<List<Dictionary<string, object>>> Get_Supplier_Stock_Error_Log(string supplier_Ids, string upload_Type, string from_Date, string from_Time, string to_Date, string to_Time, bool is_Lab_Entry);
        Task<List<Dictionary<string, object>>> Get_Supplier_Stock_Error_Log_Detail(string supplier_Ids, string stock_Data_Ids, string upload_Type);
        #endregion

        #region Report
        Task<(string, int)> Create_Update_Report_Master(Report_Master report_Master);
        Task<int> Create_Update_Report_Detail(DataTable dataTable);
        Task<List<Dictionary<string, object>>> Get_Report_Name(int id);
        Task<List<Dictionary<string, object>>> Get_Report_Detail(int id);
        Task<List<Dictionary<string, object>>> Get_Report_Detail_Filter_Parameter(int id);
        Task<int> Create_Update_Report_User_Role(DataTable dataTable);
        #endregion
    }
}
