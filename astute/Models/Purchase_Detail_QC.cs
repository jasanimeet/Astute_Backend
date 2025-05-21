using Newtonsoft.Json;

namespace astute.Models
{
    public class Purchase_Detail_QC
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }

        [JsonProperty("QC REMARKS")]
        public string? QC_Remarks { get; set; }

        [JsonProperty("SUNRISE GRADE")]
        public string? Sunrise_Grade { get; set; }

        [JsonProperty("SHADE_ID")]
        public int? Shade { get; set; }

        [JsonProperty("SHADE")]
        public string? Shade_C { get; set; }

        [JsonProperty("MILKY")]
        public string? Milky_C { get; set; }

        [JsonProperty("MILKY_ID")]
        public int? Milky { get; set; }

        [JsonProperty("Table_Black_Id")]
        public int? Table_Black { get; set; }

        [JsonProperty("Crown_Black_Id")]
        public int? Crown_Black { get; set; }

        [JsonProperty("Table_White_Id")]
        public int? Table_White { get; set; }

        [JsonProperty("Crown_White_Id")]
        public int? Crown_White { get; set; }

        [JsonProperty("TABLE WHITE")]
        public string? Table_White_C { get; set; }

        [JsonProperty("SIDE WHITE")]
        public string? Crown_White_C { get; set; }

        [JsonProperty("TABLE BLACK")]
        public string? Table_Black_C { get; set; }

        [JsonProperty("SIDE BLACK")]
        public string? Crown_Black_C { get; set; }

        [JsonProperty("Additional Remarks")]
        public string? Additional_Remarks { get; set; }
    }
}
