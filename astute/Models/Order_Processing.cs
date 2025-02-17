using Newtonsoft.Json;

namespace astute.Models
{
    public class Order_Processing
    {
        public int Id { get; set; }
        public int? User_Id { get; set; }
        public int? Assist_By { get; set; }
        public string? Customer_Name { get; set; }
        public string? Remarks { get; set; }
        public string? Status { get; set; }
        public object Order_Detail { get; set; }
    }
    public class Order_Processing_Detail
    {
        public int Id { get; set; }
        public int Supp_Stock_Id { get; set; }

        [JsonProperty("BUYER DISC")]
        public object? Buyer_Disc { get; set; }

        [JsonProperty("BUYER AMOUNT")]
        public object? Buyer_Amt { get; set; }

        [JsonProperty("COST DISC")]
        public object? Cost_Disc { get; set; }

        [JsonProperty("COST AMOUNT")]
        public object? Cost_Amt { get; set; }
        [JsonProperty("BASE DISC")]
        public object? Base_Disc { get; set; }
        [JsonProperty("BASE AMOUNT")]
        public object? Base_Amt { get; set; }
        [JsonProperty("OFFER DISC")]
        public object? Offer_Disc { get; set; }
        [JsonProperty("OFFER AMOUNT")]
        public object? Offer_Amt { get; set; }
        [JsonProperty("STATUS")]
        public object? Status { get; set; }
        [JsonProperty("QC REMARK")]
        public object? QC_Remarks { get; set; }
    }
    public class Order_Processing_Inactive
    {
        public string? Ids { get; set; }
        public bool? Is_Inactive { get; set; }
        public int? User_Id { get; set; }
    }

    #region Order_Processing_New
    public class Order_Processing_Summary
    {
        public string? Order_Status { get; set; }
        public string? Stone_Status { get; set; }
        public string? From_Date { get; set; }
        public string? To_Date { get; set; }
        public string? Stock_Id { get; set; }
        public string? Act_Mod_Id { get; set; }
        public string? Module_Id { get; set; }
    }
    public class Order_Stone_Process
    {
        public string? Order_Id { get; set; }
        public string? Order_No { get; set; }
        public string? Order_Status { get; set; }
        public string? QC_Request { get; set; }
        public string? Remarks { get; set; }
    }
    public class Order_Process_Detail
    {
        public string? Order_No { get; set; }
        public int? Sub_Order_Id { get; set; }
        public string? Company_Name { get; set; }
        public string? Is_Selected_Supp_Stock_Id { get; set; }
    }
    public class Order_Processing_Reply_To_Assist
    {
        public string? Order_No { get; set; }
        public string? Request_For { get; set; }
        public int? Sub_Order_Id { get; set; }
        public string? Summary_QC_Remarks { get; set; }
        public object Order_Detail { get; set; }
    }
    public class Order_Processing_Reply_To_Assist_Detail
    {
        public int Id { get; set; }

        [JsonProperty("CURRENT COST DISC")]
        public object? Cost_Disc { get; set; }

        [JsonProperty("CURRENT COST AMOUNT")]
        public object? Cost_Amt { get; set; }

        [JsonProperty("STATUS")]
        public object? Status { get; set; }

        [JsonProperty("REMARKS")]
        public object? Remarks { get; set; }


    }

    public partial class Order_Processing_Complete_Detail
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Supp_Stock_Id")]
        public int? SuppStockId { get; set; }

        [JsonProperty("DNA")]
        public string? Dna { get; set; }

        [JsonProperty("IMAGE LINK")]
        public string? ImageLink { get; set; }

        [JsonProperty("VIDEO LINK")]
        public string? VideoLink { get; set; }

        [JsonProperty("LAB")]
        public string? Lab { get; set; }

        [JsonProperty("CERTIFICATE LINK")]
        public string? CertificateLink { get; set; }

        [JsonProperty("COMPANY")]
        public string? Company { get; set; }

        [JsonProperty("LOCATION")]
        public string? Location { get; set; }

        [JsonProperty("ORDER NO")]
        public string? OrderNo { get; set; }

        [JsonProperty("CUSTOMER")]
        public string? Customer { get; set; }

        [JsonProperty("BUYER")]
        public string? Buyer { get; set; }

        [JsonProperty("BUYER CODE")]
        public string? Buyer_Code { get; set; }

        [JsonProperty("QC REMARKS")]
        public string? QcRemarks { get; set; }

        [JsonProperty("STOCK ID")]
        public string? StockId { get; set; }

        [JsonProperty("SUPPLIER NO")]
        public string? SupplierNo { get; set; }

        [JsonProperty("CERTIFICATE NO")]
        public string? CertificateNo { get; set; }

        [JsonProperty("SHAPE")]
        public string? Shape { get; set; }

        [JsonProperty("POINTER")]
        public string? Pointer { get; set; }

        [JsonProperty("SUB POINTER")]
        public string? SubPointer { get; set; }

        [JsonProperty("COLOR")]
        public string? Color { get; set; }

        [JsonProperty("CLARITY")]
        public string? Clarity { get; set; }

        [JsonProperty("CTS")]
        public string? Cts { get; set; }

        [JsonProperty("RAP RATE")]
        public string? RapRate { get; set; }

        [JsonProperty("RAP AMOUNT")]
        public string? RapAmount { get; set; }

        [JsonProperty("BASE DISC")]
        public string? BaseDisc { get; set; }

        [JsonProperty("BASE AMOUNT")]
        public string? BaseAmount { get; set; }

        [JsonProperty("CURRENT COST DISC")]
        public string? CurrentCostDisc { get; set; }

        [JsonProperty("CURRENT COST AMOUNT")]
        public string? CurrentCostAmount { get; set; }

        [JsonProperty("BUYER DISC")]
        public string? BuyerDisc { get; set; }

        [JsonProperty("BUYER AMOUNT")]
        public string? BuyerAmount { get; set; }

        [JsonProperty("COST DISC")]
        public string? CostDisc { get; set; }

        [JsonProperty("COST AMOUNT")]
        public string? CostAmount { get; set; }

        [JsonProperty("OFFER DISC")]
        public string? OfferDisc { get; set; }

        [JsonProperty("OFFER AMOUNT")]
        public string? OfferAmount { get; set; }

        [JsonProperty("PROCESS STATUS")]
        public string? ProcessStatus { get; set; }

        [JsonProperty("STATUS")]
        public string? Status { get; set; }

        [JsonProperty("REMARKS")]
        public string? Remarks { get; set; }

        [JsonProperty("EXPECTED PROFIT PER")]
        public string? ExpectedProfitPer { get; set; }

        [JsonProperty("EXPECTED PROFIT AMT")]
        public string? ExpectedProfitAmt { get; set; }

        [JsonProperty("CUT")]
        public string? Cut { get; set; }

        [JsonProperty("POLISH")]
        public string? Polish { get; set; }

        [JsonProperty("SYMM")]
        public string? Symm { get; set; }

        [JsonProperty("FLS INTENSITY")]
        public string? FlsIntensity { get; set; }

        [JsonProperty("KEY TO SYMBOL")]
        public string? KeyToSymbol { get; set; }

        [JsonProperty("LENGTH")]
        public string? Length { get; set; }

        [JsonProperty("WIDTH")]
        public string? Width { get; set; }

        [JsonProperty("DEPTH")]
        public string? Depth { get; set; }

        [JsonProperty("DEPTH PER")]
        public string? DepthPer { get; set; }

        [JsonProperty("TABLE PER")]
        public string? TablePer { get; set; }

        [JsonProperty("CROWN ANGLE")]
        public string? CrownAngle { get; set; }

        [JsonProperty("CROWN HEIGHT")]
        public string? CrownHeight { get; set; }

        [JsonProperty("PAVILION ANGLE")]
        public string? PavilionAngle { get; set; }

        [JsonProperty("PAVILION HEIGHT")]
        public string? PavilionHeight { get; set; }

        [JsonProperty("LASER INSCRIPTION")]
        public string? LaserInscription { get; set; }

        [JsonProperty("GIRDLE PER")]
        public string? GirdlePer { get; set; }

        [JsonProperty("LUSTER")]
        public string? Luster { get; set; }

        [JsonProperty("TABLE WHITE")]
        public string? TableWhite { get; set; }

        [JsonProperty("SIDE WHITE")]
        public string? SideWhite { get; set; }

        [JsonProperty("TABLE BLACK")]
        public string? TableBlack { get; set; }

        [JsonProperty("SIDE BLACK")]
        public string? SideBlack { get; set; }

        [JsonProperty("CULET")]
        public string? Culet { get; set; }

        [JsonProperty("GIA COMMENTS")]
        public string? GiaComments { get; set; }

        [JsonProperty("GIA TYPE")]
        public string? GiaType { get; set; }

        [JsonProperty("TABLE OPEN")]
        public string? TableOpen { get; set; }

        [JsonProperty("CROWN OPEN")]
        public string? CrownOpen { get; set; }

        [JsonProperty("PAVILION OPEN")]
        public string? PavilionOpen { get; set; }

        [JsonProperty("GIRDLE OPEN")]
        public string? GirdleOpen { get; set; }

        [JsonProperty("SHADE")]
        public string? Shade { get; set; }

        [JsonProperty("MILKY")]
        public string? Milky { get; set; }

        [JsonProperty("SUPPLIER COMMENTS")]
        public string? SupplierComments { get; set; }

        [JsonProperty("MANAGEMENT STATUS")]
        public string? ManagementStatus { get; set; }

        [JsonProperty("BGM")]
        public string? Bgm { get; set; }


        [JsonProperty("Party Code")]
        public string? Party_code { get; set; }

        [JsonProperty("Supplier Code")]
        public string? Supplier_code { get; set; }

        [JsonProperty("Supplier_Id")]
        public string? Supplier_Id { get; set; }

        [JsonProperty("Supplier_Name")]
        public string? Supplier_Name { get; set; }

        [JsonProperty("Supplier_Short_Name")]
        public string? Supplier_Short_Name { get; set; }

        [JsonProperty("LAB_Id")]
        public string? LAB_Id { get; set; }

        [JsonProperty("Shape_Id")]
        public string? Shape_Id { get; set; }

        [JsonProperty("BGM_Id")]
        public string? BGM_Id { get; set; }

        [JsonProperty("Color_Id")]
        public string? Color_Id { get; set; }

        [JsonProperty("Clarity_Id")]
        public string? Clarity_Id { get; set; }

        [JsonProperty("Cut_Id")]
        public string? Cut_Id { get; set; }

        [JsonProperty("Polish_Id")]
        public string? Polish_Id { get; set; }

        [JsonProperty("Symm_Id")]
        public string? Symm_Id { get; set; }

        [JsonProperty("FLS_INTENSITY_Id")]
        public string? FLS_INTENSITY_Id { get; set; }

        [JsonProperty("Table_Black_Id")]
        public string? Table_Black_Id { get; set; }

        [JsonProperty("Crown_Black_Id")]
        public string? Crown_Black_Id { get; set; }

        [JsonProperty("Table_White_Id")]
        public string? Table_White_Id { get; set; }

        [JsonProperty("Crown_White_Id")]
        public string? Crown_White_Id { get; set; }

        [JsonProperty("Culet_Id")]
        public string? Culet_Id { get; set; }

        [JsonProperty("Table_Open_Id")]
        public string? Table_Open_Id { get; set; }

        [JsonProperty("Crown_Open_Id")]
        public string? Crown_Open_Id { get; set; }

        [JsonProperty("Pav_Open_Id")]
        public string? Pav_Open_Id { get; set; }

        [JsonProperty("Girdle_Open_Id")]
        public string? Girdle_Open_Id { get; set; }

        [JsonProperty("CERTIFICATE DATE")]
        public string? Cert_Date { get; set; }

        [JsonProperty("Cert_Type_Id")]
        public string? Cert_Type_Id { get; set; }

        [JsonProperty("LR HALF")]
        public string? LR_Half { get; set; }

        [JsonProperty("STAR LN")]
        public string? Str_Ln { get; set; }

        [JsonProperty("Fancy_Color_Id")]
        public string? Fancy_Color_Id { get; set; }

        [JsonProperty("Fancy_Intensity_Id")]
        public string? Fancy_Intensity_Id { get; set; }

        [JsonProperty("Fancy_Overtone_Id")]
        public string? Fancy_Overtone_Id { get; set; }

        [JsonProperty("Rough_Origin_Id")]
        public string? Rough_Origin_Id { get; set; }

        [JsonProperty("FANCY COLOR")]
        public string? Fancy_Color { get; set; }

        [JsonProperty("FANCY INTENSITY")]
        public string? Fancy_Intensity { get; set; }

        [JsonProperty("FANCY OVERTONE")]
        public string? Fancy_Overtone { get; set; }

        [JsonProperty("Rough_Origin")]
        public string? Rough_Origin { get; set; }

        [JsonProperty("Cert_Type_Link")]
        public string? Cert_Type_Link { get; set; }

        [JsonProperty("Laser_Insc_Id")]
        public string? Laser_Insc_Id { get; set; }

        [JsonProperty("Girdle_Condition_Id")]
        public string? Girdle_Condition_Id { get; set; }

        [JsonProperty("GIRDLE CONDITION")]
        public string? Girdle_Condition { get; set; }

        [JsonProperty("Certi_Flag")]
        public bool? Certi_Flag { get; set; }
    }
    public class Order_Excel_Model
    {
        public string Stock_Id { get; set; }
    }

    public class Final_Order_Model
    {
        public string? From_Date { get; set; }
        public string? To_Date { get; set; }
        public string? Stock_Type { get; set; }
        public string? Stock_Id { get; set; }
        public string? Is_Selected_Supp_Stock_Id { get; set; }
    }
    public class Order_Processing_Status_Model
    {
        public int? id { get; set; }
        public string? status { get; set; }
        public string? remarks { get; set; }
        public string? current_cost_amt { get; set; }
        public string? current_cost_disc { get; set; }
        public string? offer_amt { get; set; }
        public string? offer_disc { get; set; }
    }
    #endregion
}
