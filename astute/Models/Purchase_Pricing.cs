using Newtonsoft.Json;

namespace astute.Models
{
    public class Purchase_Pricing
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }

        [JsonProperty("Final Disc")]
        public string? Final_Disc { get; set; }

        [JsonProperty("Final Value")]
        public string? Final_Value { get; set; }

        [JsonProperty("Is Repriced")]
        public bool? Is_Repriced { get; set; }
    }
    public class Purchase_Pricing_List
    {
        public object? Purchase_Pricing { get; set; }
    }
    public class Purchase_Pricing_With_Grade
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }
        [JsonProperty("Final Disc")]
        public string? Final_Disc { get; set; }
        [JsonProperty("Final Value")]
        public string? Final_Value { get; set; }
        [JsonProperty("Sunrise Grade")]
        public string? Sunrise_Grade { get; set; }
        [JsonProperty("Is Repriced")]
        public bool? Is_Repriced { get; set; }
        [JsonProperty("Is Offer")]
        public bool? Is_Offer { get; set; }
    }
    public class Purchase_Pricing_With_Grade_List
    {
        public object? Purchase_Pricing_With_Grade { get; set; }
    }
}
