using System;

namespace astute.Models
{
    public class InwardDetail
    {
        public int Id { get; set; }
        public string Stock_Id { get; set; }
        public string Cert_No { get; set; }
        public int Shape { get; set; }
        public int Color { get; set; }
        public int Clarity { get; set; }
        public float? Cts { get; set; }
        public float? Rap_Price { get; set; }
        public float? Rap_Amt { get; set; }
        public float? Cost_Disc { get; set; }
        public float? Cost_Amt { get; set; }
        public float? Offer_Disc { get; set; }
        public float? Offer_Amt { get; set; }
        public int Cut { get; set; }
        public int Polish { get; set; }
        public int Symm { get; set; }
        public int? Flour_Intensity { get; set; }
        public float? Length { get; set; }
        public float? Width { get; set; }
        public float? Depth { get; set; }
        public float Depth_Per { get; set; }
        public float Table_Per { get; set; }
        public float? Crown_Angle { get; set; }
        public float? Crown_Height { get; set; }
        public float? Pavillion_Angle { get; set; }
        public float? Pavillion_Height { get; set; }
        public int Lab { get; set; }
        public string Supplier_Ref_No { get; set; }
        public int? Girdle_Type { get; set; }
        public string Key_to_Symbol { get; set; }
        public int? Culet { get; set; }
        public string Lab_Comment { get; set; }
        public float? Str_Ln { get; set; }
        public float? LR_Half { get; set; }
        public float? Girdle_Per { get; set; }
        public int? Girdle_Condition { get; set; }
        public int? Table_White { get; set; }
        public int? Crown_White { get; set; }
        public int? Table_Black { get; set; }
        public int? Crown_Black { get; set; }
        public int? Shade { get; set; }
        public int? Luster { get; set; }
        public bool Pre_Sold { get; set; }
        public int? Buyer { get; set; }
        public int? Laser_Insc { get; set; }
        public DateTime? Cert_Date { get; set; }
        public int? Cert_Type { get; set; }
        public string Company_Id { get; set; }
        public int? Trans_Id { get; set; }
        public int? Seq_No { get; set; }
        public int? Year_Id { get; set; }
        public DateTime? Created_Date { get; set; }
        public TimeSpan? Created_Time { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Updated_Date { get; set; }
        public TimeSpan? Updated_Time { get; set; }
        public int? Updated_By { get; set; }
    }

}
