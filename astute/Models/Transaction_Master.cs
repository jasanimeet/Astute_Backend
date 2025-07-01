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

        [JsonPropertyName("Sub_Process")]
        public string? Sub_Process { get; set; }

        [JsonPropertyName("Entry_Type")]
        public string? Entry_Type { get; set; }

        [JsonPropertyName("Release_Days")]
        public float? Release_Days { get; set; }

        [JsonPropertyName("Release_Hours")]
        public float? Release_Hours { get; set; }

        [JsonPropertyName("Platform")]
        public string? Platform { get; set; }

        [JsonPropertyName("Country_Of_Origin")]
        public int? Country_Of_Origin { get; set; }

        [JsonPropertyName("Is_Consignment_Auto_Receive")]
        public bool? Is_Consignment_Auto_Receive { get; set; }

        [JsonPropertyName("Pre_Carrige_By")]
        public string? Pre_Carrige_By { get; set; }

        [JsonPropertyName("Sales_Invoice_No")]
        public string? Sales_Invoice_No { get; set; }

        [JsonPropertyName("Source_Customer_Id")]
        public int? Source_Customer_Id { get; set; }

        [JsonPropertyName("Invoice_Date")]
        public string? Invoice_Date { get; set; }
    }
}
