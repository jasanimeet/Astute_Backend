using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace astute.Models
{
    public partial class Supplier_Pricing
    {
        [Key]
        public int Supplier_Pricing_Id { get; set; }
        public int? Supplier_Id { get; set; }
        public string? Shape { get; set; }
        public string? Cts { get; set; }
        public string? Color { get; set; }
        public string? Fancy_Color { get; set; }
        public string? Clarity { get; set; }
        public string? Cut { get; set; }
        public string? Polish { get; set; }
        public string? Symm { get; set; }
        public string? Fls_Intensity { get; set; }
        public string? Lab { get; set; }
        public string? Shade { get; set; }
        public string? Luster { get; set; }
        public string? Bgm { get; set; }
        public string? Culet { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; }
        public string? Good_Type { get; set; }
        public float? Length_From { get; set; }
        public float? Length_To { get; set; }
        public float? Width_From { get; set; }
        public float? Width_To { get; set; }
        public float? Depth_From { get; set; }
        public float? Depth_To { get; set; }
        public float? Depth_Per_From { get; set; }
        public float? Depth_Per_To { get; set; }
        public float? Table_Per_From { get; set; }
        public float? Table_Per_To { get; set; }
        public float? Crown_Angle_From { get; set; }
        public float? Crown_Angle_To { get; set; }
        public float? Crown_Height_From { get; set; }
        public float? Crown_Height_To { get; set; }
        public float? Pavilion_Angle_From { get; set; }
        public float? Pavilion_Angle_To { get; set; }
        public float? Pavilion_Height_From { get; set; }
        public float? Pavilion_Height_To { get; set; }
        public float? Girdle_Per_From { get; set; }
        public float? Girdle_Per_To { get; set; }
        public string? Table_Black { get; set; }
        public string? Side_Black { get; set; }
        public string? Table_White { get; set; }
        public string? Side_white { get; set; }
        public string? Key_To_Symbol { get; set; }
        public string? Comment { get; set; }
        public string? Cert_Type { get; set; }
        public string? Table_Open { get; set; }
        public string? Crown_Open { get; set; }
        public string? Pavilion_Open { get; set; }
        public string? Girdle_Open { get; set; }
        public float? Base_Disc_From { get; set; }
        public float? Base_Disc_To { get; set; }
        public float? Base_Amount_From { get; set; }
        public float? Base_Amount_To { get; set; }
        public string? Final_Disc_Per { get; set; }
        public string? Final_Amt { get; set; }
        public string? Supplier_Filter_Type { get; set; }
        public string? Calculation_Type { get; set; }
        public string? Sign { get; set; }
        public float? Value_1 { get; set; }
        public float? Value_2 { get; set; }
        public float? Value_3 { get; set; }
        public float? Value_4 { get; set; }
        public string? SP_Calculation_Type { get; set; }
        public string? SP_Sign { get; set; }
        public string? SP_Start_Date { get; set; }
        public string? SP_Start_Time { get; set; }
        public string? SP_End_Date { get; set; }
        public string? SP_End_Time { get; set; }
        public float? SP_Value_1 { get; set; }
        public float? SP_Value_2 { get; set; }
        public float? SP_Value_3 { get; set; }
        public float? SP_Value_4 { get; set; }
        public string? MS_Calculation_Type { get; set; }
        public string? MS_Sign { get; set; }
        public float? MS_Value_1 { get; set; }
        public float? MS_Value_2 { get; set; }
        public float? MS_Value_3 { get; set; }
        public float? MS_Value_4 { get; set; }
        public string? MS_SP_Calculation_Type { get; set; }
        public string? MS_SP_Sign { get; set; }
        public string? MS_SP_Start_Date { get; set; }
        public string? MS_SP_Start_Time { get; set; }
        public string? MS_SP_End_Date { get; set; }
        public string? MS_SP_End_Time { get; set; }
        public float? MS_SP_Value_1 { get; set; }
        public float? MS_SP_Value_2 { get; set; }
        public float? MS_SP_Value_3 { get; set; }
        public float? MS_SP_Value_4 { get; set; }
        public Supplier_Pricing_Key_To_Symbole supplier_Pricing_Key_To_Symbole { get; set; }
    }
}
