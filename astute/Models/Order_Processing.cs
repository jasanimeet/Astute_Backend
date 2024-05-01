using Newtonsoft.Json;

namespace astute.Models
{
    public class Order_Processing
    {
        public int Id { get; set; }
        public int? User_Id { get; set; }
        public string? Customer_Name { get; set; }
        public string? Remarks { get; set; }
        public string? Status { get; set; }
        public object Order_Detail { get; set; }
    }   
    public class Order_Processing_Detail
    {
        public int Id { get; set; }
        public int Supp_Stock_Id { get; set; }

        [JsonProperty("COST DISC")]
        public object? Buyer_Disc { get; set; }

        [JsonProperty("COST AMOUNT")]
        public object? Buyer_Amt { get; set; }
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
    }
    public class Order_Stone_Process
    {
        public string? Order_Id { get; set; }
        public int? Order_No { get; set; }
        public string? Order_Status { get; set;}
        public string? QC_Request { get; set;}
        public string? Remarks { get; set; }
    }
    public class Order_Process_Detail
    {
        public int? Order_No { get; set; }
        public int? Sub_Order_Id { get; set; }
        public string? Company_Name { get; set; }
        public string? Is_Selected_Supp_Stock_Id { get; set; }
    }    
    public class Order_Processing_Reply_To_Assist
    {
        public int? Order_No { get; set; }
        public int? Sub_Order_Id { get; set; }
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
    public class Order_Excel_Model
    {
        public string Stock_Id { get; set; }
    }
    #endregion
}
