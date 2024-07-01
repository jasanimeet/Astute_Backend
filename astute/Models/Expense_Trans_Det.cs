using Newtonsoft.Json;
namespace astute.Models
{
    public class Expense_Trans_Det
    {
        public int Expense_Trans_Det_Id { get; set; }
        public int Account_Master_Id { get; set; }
        public string Sign { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        [JsonProperty("Amount $")]
        public decimal Amount_Dollar { get; set; }
    }
}
