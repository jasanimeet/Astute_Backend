using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Supplier_Pricing
    {
        [Key]
        public int Supplier_Pricing_Id { get; set; }
        public int? Supplier_Id { get; set; }
        public int? Sunrise_Pricing_Id { get; set; }
        public int? Customer_Pricing_Id { get; set; }
        public string? User_Pricing_Id { get; set; }
        public string? Map_Flag { get; set; }
        public bool? Stock_Lab { get; set; }
        public bool? Stock_Overseas { get; set; }
        public bool? Stock_Buyer { get; set; }
        public bool? Stock_Saler { get; set; }
        public bool? Stock_Defualt { get; set; }
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
        public string? Cert_Type { get; set; }
        public string? Table_Open { get; set; }
        public string? Crown_Open { get; set; }
        public string? Pavilion_Open { get; set; }
        public string? Girdle_Open { get; set; }
        public float? Base_Disc_From { get; set; }
        public float? Base_Disc_To { get; set; }
        public float? Base_Amount_From { get; set; }
        public float? Base_Amount_To { get; set; }
        public float? Final_Disc_From { get; set; }
        public float? Final_Disc_To { get; set; }
        public float? Final_Amount_From { get; set; }
        public float? Final_Amount_To { get; set; }
        public string? Company { get; set; }
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
        public bool? SP_Toggle_Bar { get; set; }
        public bool? MS_SP_Toggle_Bar { get; set; }
        public int? Modified_By { get; set; }
        public string? C_Length { get; set; }
        public string? C_Width { get; set; }
        public string? Cost_Disc { get; set; }
        public string? Cost_Amount { get; set; }
        public bool? Default_Price { get; set; }
        public bool? Cost_Price_Flag { get; set; }
        public bool? Final_Price_Flag { get; set; }
        public bool? Is_All_Bgm { get; set; }
        public bool? Is_All_Clarity { get; set; }
        public bool? Is_All_Color { get; set; }
        public bool? Is_All_Culet { get; set; }
        public bool? Is_All_Cut { get; set; }
        public bool? Is_All_Fls_Intensity { get; set; }
        public bool? Is_All_Good_Type { get; set; }
        public bool? Is_All_Location { get; set; }
        public bool? Is_All_Lab { get; set; }
        public bool? Is_All_Luster { get; set; }
        public bool? Is_All_Polish { get; set; }
        public bool? Is_All_Shade { get; set; }
        public bool? Is_All_Shape { get; set; }
        public bool? Is_All_Symm { get; set; }
        public bool? Is_All_Status { get; set; }
        public bool? Is_All_Cert_Type { get; set; }
        public bool? Is_All_Fancy_Color { get; set; }
        public bool? Is_All_Girdle_Open { get; set; }
        public bool? Is_All_Table_Open { get; set; }
        public bool? Is_All_Table_Black { get; set; }
        public bool? Is_All_Table_White { get; set; }
        public bool? Is_All_Side_Black { get; set; }
        public bool? Is_All_Side_white { get; set; }
        public bool? Is_All_Pavilion_Open { get; set; }
        public bool? Is_All_Crown_Open { get; set; }
        public bool? Is_All_Company { get; set; }
        public bool? Is_API_FTP_URL { get; set; }
        [NotMapped]
        public string? Query_Flag { get; set; }
        [NotMapped]
        public IList<Supplier_Pricing_Key_To_Symbol> Key_To_Symbol { get; set; } = new List<Supplier_Pricing_Key_To_Symbol>();
        public IList<Supplier_Pricing_Key_To_Symbol> Lab_Comments { get; set; } = new List<Supplier_Pricing_Key_To_Symbol>();
    }
}
