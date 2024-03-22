using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static astute.Repository.SupplierService;

namespace astute.Models
{
    public partial class AstuteDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        public AstuteDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_configuration.GetConnectionString("AstuteConnection"));
        }

        public DbSet<Category_Master> Category_Master { get; set; }
        public DbSet<Category_Value> Category_Value { get; set; }
        public DbSet<Supplier_Value_Mapping> Supplier_Value_Mapping { get; set; }
        public DbSet<Employee_Master> Employee_Master { get; set; }
        public DbSet<Employee_Document> Employee_Document { get; set; }
        public DbSet<Employee_Salary> Employee_Salary { get; set; }
        public DbSet<Country_Master> Country_Master { get; set; }
        public DbSet<State_Master> State_Master { get; set; }
        public DbSet<City_Master> City_Master { get; set; }
        public DbSet<Terms_Master> Terms_Master { get; set; }
        public DbSet<Company_Master> Company_Master { get; set; }
        public DbSet<Company_Document> Company_Document { get; set; }
        public DbSet<Company_Media> Company_Media { get; set; }
        public DbSet<Company_Bank> Company_Bank { get; set; }
        public DbSet<Year_Master> Year_Master { get; set; }
        public DbSet<Error_Log> Error_Log { get; set; }
        public DbSet<Quote_Mas_Model> Quote_Mas_Model { get; set; }
        public DbSet<Process_Master> Process_Master { get; set; }
        public DbSet<Currency_Master> Currency_Master { get; set; }
        public DbSet<Pointer_Master> Pointer_Master { get; set; }
        public DbSet<Pointer_Detail> Pointer_Detail { get; set; }
        public DbSet<BGM_Master> BGM_Master { get; set; }
        public DbSet<Bank_Master> Bank_Master { get; set; }
        public DbSet<Menu_Mas> Menu_Mas { get; set; }
        public DbSet<Invoice_Remarks> Invoice_Remarks { get; set; }
        public DbSet<Emp_rights> Emp_rights { get; set; }
        public DbSet<Holiday_Master> Holiday_Master { get; set; }
        public DbSet<Quote_Master> Quote_Master { get; set; }
        public DbSet<Employee_Mail> Employee_Mail { get; set; }
        public DbSet<Layout_Master> Layout_Master { get; set; }
        public DbSet<Layout_Detail> Layout_Detail { get; set; }
        public DbSet<Layout_Column> Layout_Column { get; set; }
        public DbSet<Rapaport_Master> Rapaport_Master { get; set; }
        public DbSet<Rapaport_Detail> Rapaport_Detail { get; set; }
        public DbSet<Rapaport_User> Rapaport_User { get; set; }
        public DbSet<Party_Master> Party_Master { get; set; }
        public DbSet<Party_Contact> Party_Contact { get; set; }
        public DbSet<TermsAndCondition> TermsAndCondition { get; set; }
        public DbSet<Party_Bank> Party_Bank { get; set; }
        public DbSet<Party_Shipping> Party_Shipping { get; set; }
        public DbSet<Party_Document> Party_Document { get; set; }
        public DbSet<Party_Assist> Party_Assist { get; set; }
        public DbSet<Rapaport_Clarity_Value> Rapaport_Clarity_Value { get; set; }
        public DbSet<Rapaport_Color_Value> Rapaport_Color_Value { get; set; }
        public DbSet<Rapaport_Date_Value> Rapaport_Date_Value { get; set; }
        public DbSet<Loader_Master> Loader_Master { get; set; }
        public DbSet<Employee_Loader> Employee_Loader { get; set; }
        public DbSet<RapaportPriceModel> RapaportPriceModel { get; set; }
        public DbSet<Party_Api> Party_Api { get; set; }
        public DbSet<Party_FTP> Party_FTP { get; set; }
        public DbSet<Party_File> Party_File { get; set; }
        public DbSet<Supplier_Details> Supplier_Details { get; set; }
        public DbSet<Supplier_Details_List> Supplier_Details_List { get; set; }
        public DbSet<DropdownModel> DropdownModel { get; set; }
        public DbSet<Supplier_Column_Mapping> Supplier_Column_Mapping { get; set; }
        public DbSet<Exchange_Rate_Master> Exchange_Rate_Master { get; set; }
        public DbSet<Bank_Dropdown_Model> Bank_Dropdown_Model { get; set; }
        public DbSet<Employee_JWT_Token> Employee_JWT_Token { get; set; }
        public DbSet<Emergency_Contact_Detail> Emergency_Contact_Detail { get; set; }
        public DbSet<Party_Media> Party_Media { get; set; }
        public DbSet<Value_Config> Value_Config { get; set; }
        public DbSet<Party_Print_Process> Party_Print_Process { get; set; }
        public DbSet<City_Master_Export> City_Master_Export { get; set; }
        public DbSet<City_Master_Combo> City_Master_Combo { get; set; }
        public DbSet<Stock_Data_Column_Value> Stock_Data_Column_Value { get; set; }
        public DbSet<Stock_Data_Master> Stock_Data_Master { get; set; }
        public DbSet<Stock_Data> Stock_Data { get; set; }
        public DbSet<Stock_Number_Generation> Stock_Number_Generation { get; set; }
        public DbSet<Supplier_Pricing_Key_To_Symbol> Supplier_Pricing_Key_To_Symbol { get; set; }
        public DbSet<Party_API_With_Column_Mapping> Party_API_With_Column_Mapping { get; set; }
        public DbSet<Temp_Layout_Master> Temp_Layout_Master { get; set; }
        public DbSet<Temp_Layout_Detail> Temp_Layout_Detail { get; set; }
        public DbSet<Common_Model> Common_Model { get; set; }
        public DbSet<Report_Master> Report_Master { get; set; }
        public DbSet<Report_Detail> Report_Detail { get; set; }
        public DbSet<Party_Master_Replica> Party_Master_Replica { get; set; }
        public DbSet<BGM_Detail> BGM_Detail { get; set; }
        public DbSet<Report_Layout_Save> Report_Layout_Save { get; set; }
        public DbSet<Report_Layout_Save_Detail> Report_Layout_Save_Detail { get; set; }
        public DbSet<Report_Image_Video_Certificate> Report_Image_Video_Certificate { get; set; }
        
    }
}
