using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Grade_Detail
    {
        [Key]
        public int Id { get; set; }
        public int Trans_Id { get; set; }
        public string? Lab { get; set; }
        public string? Shape { get; set; }
        public string? Color { get; set; }
        public string? Clarity { get; set; }
        public string? Cut { get; set; }
        public string? Polish { get; set; }
        public string? Symm { get; set; }
        public string? FLS { get; set; }
        public string? Shade { get; set; }
        public string? Milky { get; set; }
        public string? Include_Key_To_Symbol { get; set; }
        public decimal? Kts_Count { get; set; }
        public string? Exclude_Key_To_Symbol { get; set; }
        public string? Include_Comment { get; set; }
        public string? Exclude_Comment { get; set; }
        public decimal? From_Cts { get; set; }
        public decimal? To_Cts { get; set; }
        public decimal? From_Table { get; set; }
        public decimal? To_Table { get; set; }
        public decimal? From_Dept { get; set; }
        public decimal? To_Dept { get; set; }
        public decimal? From_CA { get; set; }
        public decimal? To_CA { get; set; }
        public decimal? From_PA { get; set; }
        public decimal? To_PA { get; set; }
        public decimal? From_Chgt { get; set; }
        public decimal? To_Chgt { get; set; }
        public decimal? From_Phgt { get; set; }
        public decimal? To_Phgt { get; set; }
        public decimal? From_Len { get; set; }
        public decimal? To_Len { get; set; }
        public decimal? From_Width { get; set; }
        public decimal? To_Width { get; set; }
        public string? Culet { get; set; }
        public decimal? From_Girdle { get; set; }
        public decimal? To_Girdle { get; set; }
        public decimal? From_Str_Ln { get; set; }
        public decimal? To_Str_Ln { get; set; }
        public decimal? From_Lower { get; set; }
        public decimal? To_Lower { get; set; }
        public string? Grade { get; set; }
        public int? Avg_Last_Month { get; set; }
        public string? Avg_Type { get; set; }
        public bool? Avg_Is_Pcs { get; set; }
        public bool? Avg_Is_Disc { get; set; }
        public bool? Avg_Is_Value { get; set; }

        public string? Lab_Name { get; set; }
        public string? Shape_Name { get; set; }
        public string? Color_Name { get; set; }
        public string? Clarity_Name { get; set; }
        public string? Cut_Name { get; set; }
        public string? Polish_Name { get; set; }
        public string? Symm_Name { get; set; }
        public string? FLS_Name { get; set; }
        public string? Shade_Name { get; set; }
        public string? Milky_Name { get; set; }
        public string? Include_Key_To_Symbol_Name { get; set; }
        public string? Exclude_Key_To_Symbol_Name { get; set; }
        public string? Include_Comment_Name { get; set; }
        public string? Exclude_Comment_Name { get; set; }
        public string? Culet_Name { get; set; }
    }
}
