using Newtonsoft.Json;

namespace astute.Models
{
    public class Order_Processing_Final
    {
        [JsonProperty("Supp_Stock_Id")]
        public int SuppStockId { get; set; }
        [JsonProperty("COST DISC")]
        public string Cost_Disc { get; set; }
        [JsonProperty("COST AMOUNT")]
        public string Cost_Amount { get; set; }
        [JsonProperty("OFFER DISC")]
        public string Offer_Disc { get; set; }
        [JsonProperty("OFFER AMOUNT")]
        public string Offer_Amount { get; set; }
    }
}