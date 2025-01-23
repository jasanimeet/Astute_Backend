using System.Text.Json.Serialization;

namespace astute.Models
{
    public class Purchase_Expenses
    {
        [JsonPropertyName("Purchase_Expenses_Id")]
        public int? Purchase_Expenses_Id { get; set; }

        [JsonPropertyName("Expenses_Id")]
        public int? Expenses_Id { get; set; }

        [JsonPropertyName("Amount")]
        public float? Amount { get; set; }

        [JsonPropertyName("Purchase_Trans_Id")]
        public int? Purchase_Trans_Id { get; set; }
    }
}
