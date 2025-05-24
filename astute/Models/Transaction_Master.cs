using System;
using System.Text.Json.Serialization;

namespace astute.Models
{
    public partial class Transaction_Master
    {
        [JsonPropertyName("Trans_Id")]
        public int? Trans_Id { get; set; }

        [JsonPropertyName("Trans_Date")]
        public string? Trans_Date { get; set; }

        [JsonPropertyName("Trans_Time")]
        public TimeSpan? Trans_Time { get; set; }

        [JsonPropertyName("Supplier_Id")]
        public int? Supplier_Id { get; set; }

        [JsonPropertyName("Company_Id")]
        public int? Company_Id { get; set; }

        [JsonPropertyName("Currency_Id")]
        public int? Currency_Id { get; set; }

        [JsonPropertyName("Ex_Rate")]
        public float? Ex_Rate { get; set; }

        [JsonPropertyName("Process_Id")]
        public string? Process_Id { get; set; }

        [JsonPropertyName("Year_Id")]
        public int? Year_Id { get; set; }

        [JsonPropertyName("Transaction_Invoice_No")]
        public string? Transaction_Invoice_No { get; set; }

        [JsonPropertyName("Contract")]
        public bool? Contract { get; set; }

        [JsonPropertyName("Created_By")]
        public int? Created_By { get; set; }

        [JsonPropertyName("Created_Date")]
        public DateTime? Created_Date { get; set; }

        [JsonPropertyName("Created_Time")]
        public TimeSpan? Created_Time { get; set; }

        [JsonPropertyName("Updated_Date")]
        public DateTime? Updated_Date { get; set; }

        [JsonPropertyName("Updated_Time")]
        public TimeSpan? Updated_Time { get; set; }

        [JsonPropertyName("Updated_By")]
        public int? Updated_By { get; set; }

        [JsonPropertyName("Remarks")]
        public string? Remarks { get; set; }

        [JsonPropertyName("Supplier_Invoice_No")]
        public string? Supplier_Invoice_No { get; set; }

        [JsonPropertyName("Customer_Id")]
        public int? Customer_Id { get; set; }
        
        [JsonPropertyName("Due_Date")]
        public string? Due_Date { get; set; }

        [JsonPropertyName("Confirm_Hold")]
        public bool? Confirm_Hold { get; set; }

        [JsonPropertyName("Reserver_Stock")]
        public bool? Reserver_Stock { get; set; }
    }
}
