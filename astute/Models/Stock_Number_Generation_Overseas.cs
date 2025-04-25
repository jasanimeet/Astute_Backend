using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Stock_Number_Generation_Overseas_List
    {
        public int User_Id { get; set; }
        public IList<Stock_Number_Generation_Overseas> stock_Number_Generations { get; set; } = new List<Stock_Number_Generation_Overseas>();
    }

    public partial class Stock_Number_Generation_Overseas
    {
        [Key]
        public int Id { get; set; }
        public string? Shape { get; set; }
        public string? Shape_Value { get; set; }
        public decimal? From_Cts { get; set; }
        public decimal? To_Cts { get; set; }
        public string? Front_Prefix { get; set; }
        public string? Back_Prefix { get; set; }
        public string? Front_Prefix_Alloted { get; set; }
        public string? Start_Format { get; set; }
        public int? Start_Number { get; set; }
        public int? End_Number { get; set; }
        public int? Current_No { get; set; }
        public string? Live_Prefix { get; set; }
        public int? Party_Id { get; set; }
        public string? Company_Name { get; set; }
        public string? Created_Date { get; set; }
        public string? Created_Time { get; set; }
        public string? Created_By { get; set; }
        public string? Update_Date { get; set; }
        public string? Updated_Time { get; set; }
        public string? Updated_By { get; set; }
    }
}