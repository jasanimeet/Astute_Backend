using System.Text.Json.Serialization;

namespace astute.Models
{
    public partial class Lab_Entry_Master
    {
        [JsonPropertyName("Trans_Id")]
        public int? Trans_Id { get; set; }

        [JsonPropertyName("Order_No")]
        public string? Order_No { get; set; }

        [JsonPropertyName("Order_Date")]
        public string? Order_Date { get; set; }

        [JsonPropertyName("Order_Time")]
        public string? Order_Time { get; set; }

        [JsonPropertyName("Type")]
        public string? Type { get; set; }

        [JsonPropertyName("Supplier_Name")]
        public string? Supplier_Name { get; set; }

        [JsonPropertyName("Company_Name")]
        public string? Company_Name { get; set; }

        [JsonPropertyName("Party_Id")]
        public int? Party_Id { get; set; }

        [JsonPropertyName("Remarks")]
        public string? Remarks { get; set; }

        [JsonPropertyName("Order Pcs")]
        public int? Order_Pcs { get; set; }

        [JsonPropertyName("Order Carats")]
        public string? Order_Carats { get; set; }

        [JsonPropertyName("Amount")]
        public string? Amount { get; set; }

        [JsonPropertyName("Entry_Type")]
        public string? Entry_Type { get; set; }

        [JsonPropertyName("Assist_By")]
        public int? Assist_By { get; set; }

        [JsonPropertyName("Assist_By_Name")]
        public string? Assist_By_Name { get; set; }

        [JsonPropertyName("Entry_User")]
        public int? Entry_User { get; set; }

        [JsonPropertyName("Entry_User_Name")]
        public string? Entry_User_Name { get; set; }

        [JsonPropertyName("Updated_Date")]
        public string? Updated_Date { get; set; }

        [JsonPropertyName("Updated_Time")]
        public string? Updated_Time { get; set; }

        [JsonPropertyName("Updated_By")]
        public string? Updated_By { get; set; }

        [JsonPropertyName("Modified User")]
        public string? Modified_User { get; set; }

    }
}
