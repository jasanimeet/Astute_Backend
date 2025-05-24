using Newtonsoft.Json;

namespace astute.Models
{
    public class Purchase_Detail_Manual_Discount
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }
        [JsonProperty("Manual Discount Detail Id")]
        public int? Manual_Discount_Detail_Id { get; set; }
        [JsonProperty("Stock Id")]
        public string? Stock_Id { get; set; }

        [JsonProperty("Certificate No")]
        public string? Cert_No { get; set; }

        [JsonProperty("Supplier Ref No")]
        public string? Supplier_Ref_No { get; set; }
        //public string? Supplier_Name { get; set; }
        [JsonProperty("Prev. Disc")]
        public decimal? Prev_Disc { get; set; }
        [JsonProperty("New Disc")]
        public decimal? New_Disc { get; set; }
    }
    public class Purchase_Detail_Manual_Discount_List
    {
        public object? Purchase_Detail_Manual_Discount { get; set; }
    }
    public class Purchase_Detail_Manual_Discount_Model
    {
        public string Stock_Id { get; set; }
    }
}
