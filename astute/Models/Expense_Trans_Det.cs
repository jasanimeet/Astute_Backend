using Newtonsoft.Json;
namespace astute.Models
{
    public class Expense_Trans_Det
    {
        public int Expense_Trans_Det_Id { get; set; }
        public int Account_Master_Id { get; set; }
        public string? Sign { get; set; }
        [JsonProperty("per")]
        public decimal? Percentage { get; set; }
        public decimal? amount { get; set; }
        [JsonProperty("amount $")]
        public decimal? amount_Dollar { get; set; }
    }
}
