using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class GIA_Certificate_Data
    {
        public GIA_Certificate_Data()
        {
            GIA_Certificate_Data_Details = new List<GIA_Certificate_Data_Detail>();
        }
        public int? Supplier_Id { get; set; }
        public string? Customer_Name { get; set; }
        public IList<GIA_Certificate_Data_Detail> GIA_Certificate_Data_Details { get; set; }
    }
    public partial class GIA_Certificate_Data_Detail
    {
        [Key]
        public int Id { get; set; }
        public string? Certificate_No { get; set; }
        public string? Certificate_Date { get; set; }
        public string? Shape { get; set; }
        public float? Cts { get; set; }
        public string? Color { get; set; }
        public string? Clarity { get; set; }
        public string? Cut { get; set; }
        public string? Polish { get; set; }
        public string? Symm { get; set; }
        public string? Fls_Intensity { get; set; }
        public string? Fls_Color { get; set; }
        public float? Length { get; set; }
        public float? Width { get; set; }
        public float? Depth { get; set; }
        public float? Depth_Per { get; set; }
        public float? Table_Per { get; set; }
        public string? Culet { get; set; }
        public string? Laser_Inscription { get; set; }
        public float? Crown_Angle { get; set; }
        public float? Crown_Height { get; set; }
        public float? Pavillion_Angle { get; set; }
        public float? Pavillion_Height { get; set; }
        public float? Girdle_Per { get; set; }
        public string? Girdle_Condition { get; set; }
        public float? LR_Half { get; set; }
        public float? Str_Ln { get; set; }
        public string? Certificate_Link { get; set; }
        public string? Key_to_Symbol { get; set; }
        public string? Lab_Comment { get; set; }
        public float? Rap_Rate { get; set; }
        public float? Rap_Amount { get; set; }
        public float? Supplier_Cost_Disc { get; set; }
        public float? Supplier_Cost_Amt { get; set; }
        public float? Offer_Disc { get; set; }
        public float? Offer_Amt { get; set; }
        public string? Pointer { get; set; }
        public string? Cut_Grade { get; set; }
        public string? Polish_Grade { get; set; }
        public string? Symm_Grade { get; set; }
        public string? Fluorescence { get; set; }
        public string? Shape_Code { get; set; }
        public string? Girdle_Condition_Code { get; set; }
        public string? Plotting_Diagram { get; set; }
        public string? Proportions_Diagram { get; set; }
        public string? Digital_Card { get; set; }
    }
    public partial class GIA_Certificate_Parameter_Model
    {
        public string? cert_no { get; set; }
        public string? report_Date { get; set; }
        public string? supplier_Name { get; set; }
        public string? customer_Name { get; set; }
    }
    public class GIA_Certificate_Excel_Model
    {
        public string? Certificate_No { get; set; }
        public string? Supplier_Cost_Disc { get; set; }
        public string? Supplier_Cost_Amt { get; set; }
        public string? Offer_Disc { get; set; }
        public string? Offer_Amt { get; set; }
    }
    public class GIA_Certificate_Place_Order
    {
        public int? Supplier_Id { get; set; }
        public string? Customer_Name { get; set; }
        public object? Order_Detail { get; set; }
    }

    public class GIA_Certificate_Place_Order_Detail
    {
        public object? Certificate_No { get; set; }
        public object? Supplier_Cost_Disc { get; set; }
        public object? Supplier_Cost_Amt { get; set; }
        public object? Offer_Disc { get; set; }
        public object? Offer_Amt { get; set; }
    }
}