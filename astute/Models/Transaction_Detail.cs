using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace astute.Models
{
    public partial class Transaction_Detail
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }

        [JsonProperty("Trans_Id")]
        public int? Trans_Id { get; set; }

        [JsonProperty("Purchase_Detail_Id")]
        public int? Purchase_Detail_Id { get; set; }

        [JsonProperty("RAP RATE")]
        public string? Rap_Rate { get; set; }

        [JsonProperty("RAP AMOUNT")]
        public string? Rap_Amount { get; set; }

        [JsonProperty("ACTUAL COST DISC")]
        public string? Actual_Cost_Disc { get; set; }

        [JsonProperty("ACTUAL COST AMOUNT")]
        public string? Actual_Cost_Amt { get; set; }

        [JsonProperty("CONSIGNMENT COST DISC")]
        public string? Consignment_Cost_Disc { get; set; }

        [JsonProperty("CONSIGNMENT COST AMOUNT")]
        public string? Consignment_Cost_Amt { get; set; }
    }
}
