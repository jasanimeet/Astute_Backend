using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Quotation_Master_Lab_Entry_Detail
    {
        [JsonProperty("Trans_Id")]
        public int? Trans_Id { get; set; }

        [JsonProperty("Order_No")]
        public string? Order_No { get; set; }

        [JsonProperty("Stock_Id")]
        public string? Stock_Id { get; set; }
    }
    public class Quotation_Expense_Detail
    {
        [JsonProperty("Expense_Id")]
        public int? Expense_Id { get; set; }

        [JsonProperty("Expense_Value")]
        public decimal? Expense_Value { get; set; }
    }
    public class Quotation_Master
    {
        [Required]
        public string Trans_Date { get; set; }
        public int? Bill_To_Id { get; set; }
        public int? Ship_To_Id { get; set; }
        public int? Terms_Id { get; set; }
        public int? Currency_Id { get; set; }
        public float? Ex_Rate { get; set; }
        public int? Bank_Id { get; set; }
        public string? Remark { get; set; }
        public int? Year_Id { get; set; }
        public int? Company_Id { get; set; }
        public bool Is_Summary { get; set; }
        public object? Quotation_Master_Lab_Entry_Detail { get; set; }
        public object? Quotation_Expense_Detail { get; set; }
    }
}
