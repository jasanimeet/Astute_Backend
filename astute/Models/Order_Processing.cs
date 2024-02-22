using Newtonsoft.Json;

namespace astute.Models
{
    public class Order_Processing
    {
        public int? User_Id { get; set; }
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
        public object? Order_Cost_Disc { get; set; }

        [JsonProperty("COST AMOUNT")]
        public object? Order_Cost_Amt { get; set; }
    }
}
