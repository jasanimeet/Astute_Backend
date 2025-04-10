using System.Text.Json.Serialization;

namespace astute.Models
{
    public partial class Transaction_Expenses
    {
        [JsonPropertyName("Transaction_Expenses_Id")]
        public int? Transaction_Expenses_Id { get; set; }

        [JsonPropertyName("Expenses_Id")]
        public int? Expenses_Id { get; set; }

        [JsonPropertyName("Amount")]
        public float? Amount { get; set; }

        [JsonPropertyName("Trans_Id")]
        public int? Trans_Id { get; set; }

        [JsonPropertyName("Sign")]
        public string? Sign { get; set; }

        [JsonPropertyName("Percentage")]
        public float? Percentage { get; set; }
    }
}
