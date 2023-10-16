using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace astute.Models
{
    public partial class Supplier_Pricing
    {
        [Key]
        public int Supplier_Pricing_Id { get; set; }
        public int Supplier_Id { get; set; }
        public string Shape { get; set; }
        public string Cts { get; set; }
        public string Color { get; set; }
        public string Clarity { get; set; }
        public string Cut { get; set; }
        public string Polish { get; set; }
        public string Symm { get; set; }
        public string Flour { get; set; }
        public string Lab { get; set; }
        public string Shade { get; set; }
        public string Luster { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Base_Disc_Per { get; set; }
        public string Base_Amt { get; set; }
        public string Final_Disc_Per { get; set; }
        public string Final_Amt { get; set; }
        public string Supplier_Filter_Type { get; set; }
        public string Calculation_Type { get; set; }
        public string Sign { get; set; }
        public float Value_1 { get; set; }
        public float Value_2 { get; set; }
        public float Value_3 { get; set; }
        public float Value_4 { get; set; }
        public string SP_Calculation_Type { get; set; }
        public string SP_Sign { get; set; }
        public string SP_Start_Date { get; set; }
        public string SP_Start_Time { get; set; }
        public string SP_End_Date { get; set; }
        public string SP_End_Time { get; set; }
        public float SP_Value_1 { get; set; }
        public float SP_Value_2 { get; set; }
        public float SP_Value_3 { get; set; }
        public float SP_Value_4 { get; set; }
        public string MS_Calculation_Type { get; set; }
        public string MS_Sign { get; set; }
        public float MS_Value_1 { get; set; }
        public float MS_Value_2 { get; set; }
        public float MS_Value_3 { get; set; }
        public float MS_Value_4 { get; set; }
        public string MS_SP_Calculation_Type { get; set; }
        public string MS_SP_Sign { get; set; }
        public string MS_SP_Start_Date { get; set; }
        public string MS_SP_Start_Time { get; set; }
        public string MS_SP_End_Date { get; set; }
        public string MS_SP_End_Time { get; set; }
        public float MS_SP_Value_1 { get; set; }
        public float MS_SP_Value_2 { get; set; }
        public float MS_SP_Value_3 { get; set; }
        public float MS_SP_Value_4 { get; set; }
    }
}
