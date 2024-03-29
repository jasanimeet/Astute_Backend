﻿using Newtonsoft.Json;

namespace astute.Models
{
    public class Order_Processing
    {
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
}
