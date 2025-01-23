using System.Text.Json.Serialization;

namespace astute.Models
{
    public partial class Purchase_Terms
    {
        [JsonPropertyName("Purchase_Terms_Id")]
        public int? Purchase_Terms_Id { get; set; }

        [JsonPropertyName("Terms_Id")]
        public int? Terms_Id { get; set; }

        [JsonPropertyName("Amount")]
        public float? Amount { get; set; }

        [JsonPropertyName("Purchase_Trans_Id")]
        public int? Purchase_Trans_Id { get; set; }
    }
}
