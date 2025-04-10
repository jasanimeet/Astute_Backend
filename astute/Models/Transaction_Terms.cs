using System.Text.Json.Serialization;

namespace astute.Models
{
    public partial class Transaction_Terms
    {
        [JsonPropertyName("Transaction_Terms_Id")]
        public int? Transaction_Terms_Id { get; set; }

        [JsonPropertyName("Terms_Id")]
        public int? Terms_Id { get; set; }

        [JsonPropertyName("Amount")]
        public float? Amount { get; set; }

        [JsonPropertyName("Trans_Id")]
        public int? Trans_Id { get; set; }
    }
}
