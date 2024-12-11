using Newtonsoft.Json;

namespace astute.Models
{
    public partial class Order_Processing_Complete_Fortune_Detail
    {

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Supp_Stock_Id")]
        public string? SuppStockId { get; set; }

        [JsonProperty("CERTIFICATE NO")]
        public string? CertificateNo { get; set; }

        [JsonProperty("STATUS")]
        public string? Status { get; set; }

        [JsonProperty("BUYER CODE")]
        public int? BuyerCode { get; set; }

        [JsonProperty("BASE AMOUNT")]
        public string? BaseAmount { get; set; }

        [JsonProperty("COST AMOUNT")]
        public string? CostAmount { get; set; }

        [JsonProperty("Supplier Code")]
        public string? Supplier_code { get; set; }

        [JsonProperty("SHADE")]
        public string? Shade { get; set; }

        [JsonProperty("MILKY")]
        public string? Milky { get; set; }

        [JsonProperty("TABLE OPEN")]
        public string? TableOpen { get; set; }

        [JsonProperty("CROWN OPEN")]
        public string? CrownOpen { get; set; }

        [JsonProperty("PAVILION OPEN")]
        public string? PavilionOpen { get; set; }

        [JsonProperty("GIRDLE OPEN")]
        public string? GirdleOpen { get; set; }
    }
}