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
}
