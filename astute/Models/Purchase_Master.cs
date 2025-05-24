using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace astute.Models
{
    public partial class Purchase_Master
    {
        [JsonPropertyName("Trans_Id")]
        public int? Trans_Id { get; set; }

        [JsonPropertyName("Trans_Date")]
        public string? Trans_Date { get; set; }

        [JsonPropertyName("Trans_Time")]
        public TimeSpan? Trans_Time { get; set; }

        [JsonPropertyName("Year_Id")]
        public int? Year_Id { get; set; }

        [JsonPropertyName("Trans_Type")]
        public string? Trans_Type { get; set; }

        [JsonPropertyName("Doc_Type")]
        public string? Doc_Type { get; set; }

        [JsonPropertyName("Supplier_Doc_Date")]
        public string? Supplier_Doc_Date { get; set; }

        [JsonPropertyName("Supplier_Doc_Time")]
        public TimeSpan? Supplier_Doc_Time { get; set; }

        [JsonPropertyName("Supplier_Invoice_No")]
        public string? Supplier_Invoice_No { get; set; }

        [JsonPropertyName("Internal_Invoice_No")]
        public string? Internal_Invoice_No { get; set; }

        [JsonPropertyName("Supplier_Id")]
        public int? Supplier_Id { get; set; }

        [JsonPropertyName("Company_Id")]
        public int? Company_Id { get; set; }

        [JsonPropertyName("Currency_Id")]
        public int? Currency_Id { get; set; }

        [JsonPropertyName("Exchange_Id")]
        public int? Exchange_Id { get; set; }

        [JsonPropertyName("Ex_Rate")]
        public float? Ex_Rate { get; set; }

        [JsonPropertyName("Process_Id")]
        public string? Process_Id { get; set; }

        [JsonPropertyName("Shipment_Type")]
        public string? Shipment_Type { get; set; }

        [JsonPropertyName("ETA_Days")]
        public int? ETA_Days { get; set; }

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

        [JsonProperty("Invoice_Amount")]
        public string? Invoice_Amount { get; set; }
    }
}