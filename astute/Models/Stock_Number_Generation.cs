using System.ComponentModel.DataAnnotations;

namespace astute.Models
{

    public partial class Stock_Number_Generation
    {
        [Key]
        public int Id { get; set; }
        public string? Exc_Party_Id { get; set; }
        public string? Party_Name { get; set; }
        public string? Pointer_Id { get; set; }
        public string? Pointer_Name { get; set; }
        public string? Shape { get; set; }
        public string? Shape_Value { get; set; }
        public string? Stock_Type { get; set; }
        public string? Front_Prefix { get; set; }
        public string? Back_Prefix { get; set; }
        public string? Front_Prefix_Alloted { get; set; }
        public string? Start_Format { get; set; }
        public string? End_Format { get; set; }
        public int? Start_Number { get; set; }
        public int? End_Number { get; set; }
        public string? Live_Prefix { get; set; }
        public int? Supplier_Id { get; set; }
        public string? Supplier { get; set; }
        public string? Created_Date { get; set; }
        public string? Created_Time { get; set; }
        public string? Update_Date { get; set; }
        public string? Updated_Time { get; set; }
    }
}