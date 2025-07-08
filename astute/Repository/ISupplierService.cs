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
        Task<List<Dictionary<string, object>>> Get_Supplier_Pricing(int supplier_Pricing_Id, int supplier_Id, string supplier_Filter_Type, string map_Flag, int sunrise_pricing_Id, int customer_pricing_Id, string? user_pricing_Id);
        Task<(string, int)> Add_Update_Supplier_Pricing(Supplier_Pricing supplier_Pricing);
        Task<int> Delete_Supplier_Pricing(int supplier_Pricing_Id, int supplier_Id);
        Task<int> Delete_Supplier_Pricing_By_Supplier(int supplier_Id);
        Task<Common_Model> Get_Max_Sunrice_Pricing_Id();
        Task<int> Delete_Sunrise_Pricing(int sunrise_Pricing_Id);
        Task<int> Delete_Customer_Pricing(int user_Pricing_Id);
        #endregion

        #region Supplier Pricing Key To Symbol
        Task<int> Add_Update_Supplier_Pricing_Key_To_Symbol(DataTable dataTable);
        Task<int> Delete_Supplier_Pricing_Key_To_Symbol(int supplier_Pricing_Id, string filter_Type);
        #endregion

        #region Supplier Stock
        Task<(string, int)> Stock_Data_Insert_Update(Stock_Data_Master stock_Data_Master);
        Task<(string, int)> Stock_Data_Custom_Insert_Update(Stock_Data_Master_Schedular stock_Data_Master);
        Task<int> Stock_Data_Detail_Insert_Update(DataTable dataTable, int Stock_Data_Id);
        Task<List<Dictionary<string, object>>> Get_Stock_Data_Distinct_Column_Values(string column_Name, int supplier_Id);
        Task<IList<Stock_Data>> Get_Not_Uploaded_Stock_Data(int stock_data_Id);
        Task<int> Supplier_Stock_Insert_Update(int supplier_Id, int stock_Data_Id);
        Task<string> Stock_Data_Supplier_Count_Select(int supplier_Id);
        Task<int> Supplier_Stock_Manual_File_Insert_Update(int supplier_Id, int stock_Data_Id, bool is_Overwrite, bool Priority);
        Task<int> Stock_Data_Shedular_Insert_Update(DataTable dataTable, int Stock_Data_Id);
        Task<DropdownModel> Get_Purchase_Order_Supplier(string supp_Ref_No);
        Task<int> Supplier_Stock_Start_End_Time_Update(Supplier_Stock_Update supplier_Stock_Update);
        Task<List<Dictionary<string, object>>> Get_Stock_Data_By_Rapaport_Increase_Decrease(string rap_increase, string rap_decrease);
        Task<int> Update_Stock_Data_By_Rapaport_Increase_Decrease(string rap_increase, string rap_decrease);
        Task<int> Supplier_Overseas_Stock_Insert_Update(int supplier_Id, int stock_Data_Id);
        Task<int> Supplier_Overseas_Stock_Manual_File_Insert_Update(int supplier_Id, int stock_Data_Id, bool is_Overwrite, bool Priority);
        #endregion

        #region Stock Number Generation
        Task<IList<Stock_Number_Generation>> Get_Stock_Number_Generation(int Id);
        Task<int> Add_Update_Stock_Number_Generation(DataTable dataTable);
        Task<int> Delete_Stock_Number_Generation(int Id);
        Task<int> Add_Update_Stock_Number_Generation_Raplicate(string ids);
        Task<IList<Stock_Number_Generation_Overseas>> Get_Stock_Number_Generation_Overseas(int Id);
        Task<int> Add_Update_Stock_Number_Generation_Overseas(DataTable dataTable);
        Task<int> Delete_Stock_Number_Generation_Overseas(int Id);
        Task<int> Add_Update_Stock_Number_Generation_Overseas_Raplicate(string ids);
        Task<List<Dictionary<string, object>>> Stock_Number_Generation_Replicate_Availability();
        #endregion

        #region  Api/FTP/File Party Name
        Task<IList<DropdownModel>> Get_Api_Ftp_File_Party_Select(int party_Id, bool lab, bool overseas, bool is_Stock_Gen);
        #endregion

        #region Supplier Stock Error Log
        Task<List<Dictionary<string, object>>> Get_Supplier_Stock_Error_Log(string supplier_Ids, string upload_Type, string from_Date, string from_Time, string to_Date, string to_Time, bool is_Last_Entry, string stock_Type, string supplierNo_CertNo);
        Task<List<Dictionary<string, object>>> Get_Supplier_Stock_Error_Log_Detail(string supplier_Ids, string stock_Data_Ids, string upload_Type, string supplierNo_CertNo, string stock_Type);
        Task<List<Dictionary<string, object>>> Get_Supplier_Stock_File_Error_Log(int supplier_Id, int stock_Data_Id);
        Task<DataTable> Get_Supplier_Stock_File_Error_Log_Detail(int supplier_Id, string upload_Type);
        Task<List<Dictionary<string, object>>> Get_Data_Transfer_Log(string from_Date, string to_Date);
        #endregion

        #region Report
        Task<(string, int)> Create_Update_Report_Master(Report_Master report_Master);
        Task<int> Create_Update_Report_Detail(DataTable dataTable);
        Task<List<Dictionary<string, object>>> Get_Report_Name(int id, int user_Id);
        Task<List<Dictionary<string, object>>> Get_Report_Detail(int id);
        Task<List<Dictionary<string, object>>> Get_Report_Detail_Filter_Parameter(int id, int user_Id);
        Task<int> Create_Update_Report_User_Role(DataTable dataTable, string? user_Type);
        Task<List<Dictionary<string, object>>> Get_Report_Users_Role(int id, int user_Id, string user_Type, bool? Is_Display);
        Task<List<Dictionary<string, object>>> Get_Report_Users_Role_Format_Type(int id, int user_Id);
        Task<int> Delete_Report_User_Role(int id, int user_Id, string format_Type);
        Task<(List<Dictionary<string, object>>, string, string, string, string, string, string, string, string, DataTable)> Get_Report_Search(int id, IList<Report_Filter_Parameter> report_Filter_Parameters, int iPgNo, int iPgSize, IList<Report_Sorting> iSort, string is_Selected_Supp_Stock_Id, string act_Mod_Id);
        Task<(List<Dictionary<string, object>>, string, string, string, string, string, string, string, string, string, string)> Get_Lab_Search_Report_Search(DataTable dataTable, int iPgNo, int iPgSize, IList<Report_Sorting> iSort, int? user_Id, string is_Selected_Supp_Stock_Id, string user_Format);
        Task<List<Dictionary<string, object>>> Get_Lab_Search_Distinct_Report_Search(DataTable dataTable);
        Task<(List<Dictionary<string, object>>, string, string, string, string, string, string, string, string, string, string, string)> Get_Lab_Search_Report_Search_Total(DataTable dataTable, int iPgNo, int iPgSize, IList<Report_Sorting> iSort);
        Task<List<Dictionary<string, object>>> Get_Stock_Avalibility_Report_Search(DataTable dataTable, string stock_Id, string stock_Type, string supp_Stock_Id, int iPgNo, int iPgSize, IList<Report_Sorting> iSort, int party_Id);
        Task<List<Dictionary<string, object>>> Get_Report_Column_Format(int user_Id, int report_Id, string format_Type);
        Task<string> Create_Update_Report_Search(Report_Search_Save report_Search_Save, int user_Id);
        Task<List<Dictionary<string, object>>> Get_Search_Save_Report_Search(int user_Id);
        Task<int> Delete_Report_Search(int id);
        Task<(string, int)> Create_Update_Report_Layout_Save(Report_Layout_Save report_Layout_Save);
        Task<int> Insert_Update_Report_Layout_Save_Detail(DataTable dataTable);
        Task<IList<Report_Layout_Save>> Get_Report_Layout_Save(int User_Id, int Rm_Id);
        Task<int> Update_Report_Layout_Save_Status(int id, int user_Id, int rm_Id);
        Task<int> Delete_Report_Layout_Save(int id);
        Task<DataTable> Get_Report_Search_Excel(int id, IList<Report_Filter_Parameter> report_Filter_Parameters);
        Task<DataTable> Get_Stock_Availability_Report_Excel(DataTable dataTable, string stock_Id, string stock_Type, int party_Id, string excel_Format);
        Task<IList<Report_Image_Video_Certificate>> Download_Image_Video_Certificate_Stock(string? Ids, string? document_Type);
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_Stock_Report(string? StockType, bool? Contract, bool? Upcoming, bool? Offer);
        Task<DataTable> Get_Purchase_Detail_Stock_Report_Excel(string Ids);
        #endregion

        #region GIA Lap Parameter
        Task<int> Insert_GIA_Certificate_Data(DataTable dataTable, int supplier_Id, string customer_Name);
        Task<List<Dictionary<string, object>>> Get_GIA_Certificate_Data(string report_Date);
        Task<List<Dictionary<string, object>>> Get_GIA_Certificate_Update_Data(DataTable dataTable, string supplier_Name, string customer_Name);
        Task<int> GIA_Certificate_Placed_Order(DataTable dataTable, int supplier_Id, string customer_Name);
        //Task<List<Dictionary<string, object>>> GIA_Lab_Parameter(string report_Date);
        #endregion

        #region Get Excel Formet Stock Result
        Task<DataTable> Get_Stock_In_Datatable(string supp_ref_no, string excel_Format, int user_Id);
        Task<DataTable> Get_Excel_Report_Search_New(IList<Report_Filter_Parameter> report_Filter_Parameters, string excel_Format, string supplier_Ref_No, int user_Id);
        Task<DataTable> Get_Excel_Report_Search(DataTable dt_Search, string excel_Format, string supplier_Ref_No, int user_Id);
        #endregion

        #region Order Processing New
        Task<List<Dictionary<string, object>>> Get_Order_Summary(int user_Id, Order_Processing_Summary order_Processing_Summary);
        Task<DataTable> Get_Order_Summary_Pre_Post_Excel(int user_Id, Order_Processing_Summary order_Processing_Summary);
        Task<int> Create_Stone_Order_Process(Order_Stone_Process order_Stone_Processing, int user_Id);
        Task<List<Dictionary<string, object>>> Get_Order_Detail(int user_Id, Order_Process_Detail order_Process_Detail);
        Task<List<Dictionary<string, object>>> Get_Order_Request_Status(Order_Process_Detail order_Process_Detail);
        Task<int> Delete_Order_Process(string order_No, int sub_Order_Id, int user_Id);
        Task<(string, int)> Delete_Entire_Order_Process(string order_No, int user_Id);
        Task<int> Accept_Request_Order_Process(Order_Process_Detail order_Process_Detail, int user_Id);
        Task<int> Delete_Order_Stones(string order_Id, int user_Id);
        Task<int> Order_Processing_Reply_To_Assist(DataTable dataTable, string order_No, int sub_Order_Id, int user_Id);
        Task<int> Order_Processing_Completed(DataTable dataTable, string order_No, int sub_Order_Id, int user_Id, string customer_Name);
        Task<int> Order_Procesing_Stone_Location_Solar(string order_No, string stock_ids);
        Task<(DataTable, bool)> Get_Order_Excel_Data(IList<Report_Filter_Parameter> report_Filter_Parameters, int user_Id, string order_Id, string sub_Order_Id);
        Task<(DataTable, bool)> Get_Order_Data_Excel(string stock_Id, int user_Id, string order_Id);
        Task<DataTable> Get_Order_Excel_Data_Mazal(IList<Report_Filter_Parameter> report_Filter_Parameters, string order_Id);
        Task<DataTable> Get_Order_Data_Mazal_Excel(string stock_Id, string order_Id);
        Task<List<Dictionary<string, object>>> Get_Order_Processing_Name_Status_Select(int sub_order_Id, string order_Id);
        Task<List<Dictionary<string, object>>> Get_Company_Name();
        Task<List<Dictionary<string, object>>> Get_Final_Order(Final_Order_Model final_Order_Model);
        Task<int> Order_Processing_Status_Update(Order_Processing_Status_Model order_Processing_Status_Model, int user_Id);
        Task<int> Final_Order_Processing_Create_Update(DataTable dataTable, int? user_Id);
        Task<int> Final_Order_Processing_Create_Update_Save(DataTable dataTable, int? user_Id);
        Task<List<Dictionary<string, object>>> Get_Lab_Entry_Summary(int user_Id, Order_Processing_Summary order_Processing_Summary);
        Task<List<Dictionary<string, object>>> Get_Lab_Entry_Detail(int trans_id);
        Task<List<Dictionary<string, object>>> Get_Lab_Entry_Is_Img_Cert(Lab_Entry_Is_Img_Cert_Model lab_Entry_Is_Img_Cert_Model);
        Task<List<Dictionary<string, object>>> Get_Order_Processing_Hold(int user_Id);
        Task<int> Insert_Update_Lab_Entry(DataTable masterDataTable, DataTable detailDataTable, int user_Id);
        Task<(int, bool)> Delete_Lab_Entry(int id);
        Task<List<Dictionary<string, object>>> Get_Lab_Entry_Report_Summary(int user_Id, Lab_Entry_Summary lab_Entry_Summary);
        Task<DataTable> Get_Lab_Entry_Report_Data(Report_Lab_Entry_Filter report_Lab_Entry_Filter);
        Task<DataTable> Get_Lab_Entry_Report_Data_Dynamic(Report_Lab_Entry_Filter report_Lab_Entry_Filter);
        Task<DataTable> Get_Lab_Entry_Report_Data_Pickup(Report_Lab_Entry_Filter report_Lab_Entry_Filter);
        Task<DataTable> Get_Lab_Entry_Auto_Order_Not_Placed_Overseas_Email();
        Task<List<Dictionary<string, object>>> Get_Lab_Entry_Detail_For_Shipment_Verification(int supplier_Id, string certificate_No);
        Task<List<Dictionary<string, object>>> Get_Unavailable_Lab_Entry_Detail_For_Shipment_Verification(string certificate_No);
        Task<List<Dictionary<string, object>>> Get_Lab_Entry_Detail_For_Shipment_Verification_By_Id(string Lab_Entry_Detail_Id);
        Task<List<Dictionary<string, object>>> Get_Purchase_Expenses_DropDown();
        Task<(int, bool)> Insert_Update_Purchase(DataTable masterDataTable, DataTable detailDataTable, DataTable termsDataTable, DataTable expensesDataTable, DataTable purchaseDetailLooseDataTable, int user_Id);
        Task<List<Dictionary<string, object>>> Get_Purchase_Master(Purchase_Master_Search_Model purchase_Master_Search_Model);
        Task<Dictionary<string, object>> Get_Purchase(int Trans_Id);
        Task<Dictionary<string, object>> Get_Purchase_Barcode_Print(int Trans_Id);
        Task<(int, bool)> Delete_Purchase(int Trans_Id, int User_Id);
        Task<List<Dictionary<string, object>>> Get_Lab_Entry_Report_Status_Summary(string Stock_Id);
        Task<List<Dictionary<string, object>>> Get_Lab_Entry_Report_Non_Status_Summary(string Stock_Id);
        Task<int> Lab_Entry_Report_Status_Update(DataTable statusDataTable, int user_Id);
        Task<DataTable> Get_Purchase_Detail_Excel(int Trans_Id);
        Task<DataTable> Get_Purchase_Detail_QC_Excel(string Id);
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_Contract(string certificate_No);
        Task<int> Purchase_Detail_Contract_Update(DataTable purchase_Detail_Contract_DataTable, int User_Id);
        Task<int> Purchase_Detail_Outward_Update(DataTable dataTable, int Trans_Id, int User_Id);
        Task<int> Purchase_Detail_QC_Update(DataTable dataTable, int Trans_Id, int User_Id);
        Task<int> Purchase_Confirm_Update(int Trans_Id, int User_Id);
        Task<List<Dictionary<string, object>>> Order_Process_Pending_FCM_Token();
        Task<int> Order_Process_Pending_FCM_Token_Update(string Order_No);
        Task<int> Update_Purchase_Master_File_Status(int Trans_Id, bool File_Status, int User_Id);
        Task<List<Dictionary<string, object>>> Get_Purchase_Pricing(int Trans_Id);
        Task<DataTable> Get_Purchase_Pricing_Excel(int Trans_Id);
        Task<int> Purchase_Detail_Pricing_Update(DataTable dataTable, int User_Id);
        Task<List<Dictionary<string, object>>> Get_Lab_Entry_Report_Status_Sunrise_Summary(string Sunrise_Stock_Id);
        Task<int> Lab_Entry_Report_Status_Sunrise_Update(DataTable dataTable);
        Task<List<Dictionary<string, object>>> Get_Fortune_Lab_Entry_Data();
        Task<List<Dictionary<string, object>>> Get_Purchase_Master_Pricing(Purchase_Master_Search_Model purchase_Master_Search_Model);
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_Pricing(int Trans_Id, int User_Id);
        Task<int> Purchase_Pricing_Update(DataTable dataTable, int User_Id);
        Task<int> Purchase_Pricing_With_Grade_Update(DataTable dataTable, int User_Id);
        Task<string> Update_Purchase_Master_Is_Upcoming_Approval(Purchase_Approval purchase_Approval, int User_Id);
        Task<string> Update_Purchase_Master_Is_Repricing_Approval(Purchase_Approval purchase_Approval, int User_Id);
        Task<DataTable> Get_Purchase_Detail_Pricing_Excel(int Trans_Id);
        Task<List<Dictionary<string, object>>> Get_Purchase_Media_Upload(Purchase_Media_Upload_Search_Model purchase_Media_Upload_Search_Model, DataTable dt);
        Task<int> Update_Purchase_Media_Upload(Purchase_Media_Upload_Model purchase_Media_Upload_Model);
        Task<int> Update_Purchase_Shipment_Receive(string Trans_Id);
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Qc(int Trans_Id, string Doc_Type);
        Task<List<Dictionary<string, object>>> Get_Purchase_Manual_Media_Upload(Purchase_Media_Upload_Search_Model purchase_Media_Upload_Search_Model, DataTable dt);
        #endregion

        #region Purchase QC Approval
        Task<List<Dictionary<string, object>>> Get_Purchase_QC_Approval(Purchase_Master_Search_Model purchase_Master_Search_Model, int user_Id);
        Task<DataTable> Get_Purchase_QC_Approval_Data_Dynamic(Report_Lab_Entry_Filter report_Lab_Entry_Filter);
        Task<DataTable> Get_Purchase_QC_Approval_Data(Report_Lab_Entry_Filter report_Lab_Entry_Filter);
        Task<int> Purchase_QC_Reply_Status_Update(DataTable dataTable, int User_Id);
        Task<int> Purchase_Detail_QC_Complete_Update(DataTable dataTable, int Trans_Id, int User_Id);
        Task<int> Purchase_Detail_QC_Close_Update(int Trans_Id, int User_Id);
        Task<List<Dictionary<string, object>>> Purchase_Detail_QC_Pending();
        #endregion

        #region Transaction
        Task<List<Dictionary<string, object>>> Get_Transaction_Master(Transaction_Master_Search_Model transaction_Master_Search_Model, int User_Id);
        Task<Dictionary<string, object>> Get_Transaction(int Trans_Id);
        Task<(int, bool)> Insert_Update_Transaction(DataTable masterDataTable, DataTable detailDataTable, DataTable termsDataTable, DataTable expensesDataTable, DataTable detailLooseDataTable, int user_Id);
        Task<(int, bool)> Transaction_Auto_Consignment_Receive_Insert_Update(DataTable masterDataTable, DataTable detailDataTable, DataTable termsDataTable, DataTable expensesDataTable, DataTable detailLooseDataTable, int user_Id);
        Task<int> Delete_Transaction(int Trans_Id, int User_Id);
        Task<DataTable> Get_Transaction_Excel(int Trans_Id);
        Task<DataTable> Get_Transaction_Report_Excel(string Ids);
        Task<int> Transaction_Auto_Release_Insert_Update();
        #endregion

        #region Purchase Return
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Purchase_Return(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Purchase_Return(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        #endregion

        #region Consignment Return
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Consignment_Return(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Consignment_Return(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        #endregion

        #region Purchase from Consignment
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Consignment_Purchase(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Consignment_Purchase(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        #endregion

        #region Hold
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Hold(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Hold(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        #endregion

        #region Release
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Release(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Release(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        #endregion

        #region Hold Customer DropDown
        Task<List<Dictionary<string, object>>> Get_Transaction_Hold_Customer_DropDown(int User_Id);
        #endregion

        #region Consignment Issue
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Consignment_Issue(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Consignment_Issue(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        #endregion

        #region Consignment Receive
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Consignment_Receive(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Consignment_Receive(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        #endregion

        #region Consignment Issue Customer DropDown
        Task<List<Dictionary<string, object>>> Get_Transaction_Consignment_Issue_Customer_DropDown(int User_Id);
        #endregion

        #region Internal Issue
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Internal_Issue(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Internal_Issue(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        #endregion

        #region Internal Receive
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Internal_Receive(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Internal_Receive(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        #endregion

        #region Internal Issue Customer DropDown
        Task<List<Dictionary<string, object>>> Get_Transaction_Internal_Issue_Customer_DropDown(int User_Id);
        #endregion

        #region Sales
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Sales(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Sales(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        #endregion

        #region Sales Return
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Sales_Return(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Sales_Return(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return);
        #endregion

        #region Sales Customer DropDown
        Task<List<Dictionary<string, object>>> Get_Transaction_Sales_Customer_DropDown(int? User_Id);
        #endregion

        #region Sales Invoice DropDown
        Task<List<Dictionary<string, object>>> Get_Transaction_Sales_Invoice_DropDown(int Customer_Id);
        #endregion

        #region Party Url Format
        Task<IList<Party_Url_Format>> Get_Party_Url_Format(int Id);
        Task<int> Create_Update_Party_Url_Format(DataTable dataTable);
        Task<int> Delete_Party_Url_Format(int id);
        #endregion

        #region Get Lastest Supplier Stock
        Task<DataTable> Get_Latest_Supplier_Stock_Excel_Download(int supplier_Id);
        #endregion

        #region Connect GIA Report Layout Save
        Task<IList<Report_Layout_Save>> Get_Connect_GIA_Report_Layout_Save(int User_Id, int Rm_Id);
        Task<List<Dictionary<string, object>>> Get_Connect_GIA_Report_Users_Role(int id, int user_Id);
        Task<(string, int)> Create_Update_Connect_GIA_Report_Layout_Save(Report_Layout_Save report_Layout_Save);
        Task<int> Insert_Update_Connect_GIA_Report_Layout_Save_Detail(DataTable dataTable);
        #endregion

        #region Purchase Detail Manual Discount
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_Manual_Discount(DataTable dataTable, int type);
        Task<int> Set_Purchase_Detail_Manual_Discount(DataTable dataTable, int User_Id);
        #endregion

        #region Quotation Master
        Task<Dictionary<string, object>> Get_Quotation_Master(int Quotation_Id);
        Task<List<Dictionary<string, object>>> Get_Quotation_Detail(int Quotation_Id, bool isSummary = false);
        Task<int> Set_Quotation_Master(DataTable dataTable, DataTable quotaion_Expense, Quotation_Master model, int User_Id);
        Task<List<Dictionary<string, object>>> Get_Quotation_Other_Detail(string Trans_Date);
        Task<IList<DropdownModel>> Get_Quotation_Remarks_List();
        Task<(IList<DropdownModel>, int)> Get_Quotation_Company_Bank_List(int Company_Id, int Currency_Id);
        Task<List<Dictionary<string, object>>> Get_Quotation_Expense_Detail(int Quotation_Id);
        #endregion

        #region Grade Master
        Task<(IList<Dictionary<string, object>>, int)> Get_Grade_Master(int Grade_Id);
        Task<Grade_Master> Get_Grade_Detail(int Grade_Id);
        Task<(string, int)> Set_Grade_Master(Grade_Master model, DataTable dataTable, int user_Id);
        Task<(string, int)> Delete_Grade_Master(int Grade_Id);
        #endregion

        #region QC Master / Detail
        Task<IList<QC_Master>> Get_QC_Master();
        Task<IList<QC_Detail>> Get_QC_Detail(int Grade_Id);
        Task<int> Create_Update_QC_Master(DataTable dataTable, int user_Id);
        Task<int> Create_Update_QC_Detail(DataTable dataTable, int user_Id);
        Task<(string, int)> Delete_QC_Master(int Grade_Id);
        #endregion

        #region QC Pricing Skip
        Task<List<Dictionary<string, object>>> Get_Purchase_Master_With_Pending_Upcoming_QC_Pricing(int? Trans_Id, string? CertOrStockId);
        Task<List<Dictionary<string, object>>> Get_Purchase_Detail_With_Pending_Upcoming_QC_Pricing(int Trans_Id);
        Task<DataTable> Get_Purchase_Detail_With_Pending_Upcoming_QC_Pricing_Excel(string Id, string ExcelType);
        #endregion

        #region Stone Trace Report
        Task<Dictionary<string, object>> Get_Stone_Trace_Master_Report(string id);
        Task<List<Dictionary<string, object>>> Get_Stone_Trace_Detail_Report(string id);
        #endregion
    }
}
