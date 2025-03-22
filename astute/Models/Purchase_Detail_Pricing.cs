using Newtonsoft.Json;

namespace astute.Models
{
    public class Purchase_Detail_Pricing
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }

        [JsonProperty("Buyer Final Disc")]
        public string? Buyer_Final_Disc { get; set; }

        [JsonProperty("Buyer Final Value")]
        public string? Buyer_Final_Value { get; set; }
    }
    public class Purchase_Detail_Pricing_List
    {
        public object? Purchase_Detail_Pricing { get; set; }
    }
}
