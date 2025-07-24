using Newtonsoft.Json;

namespace astute.Models
{
    public partial class Process_Margin_Master
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Process_Id")]
        public int Process_Id { get; set; }
        [JsonProperty("Assist_Person_Id")]
        public int Assist_Person_Id { get; set; }
        [JsonProperty("Shape_Group")]
        public string? Shape_Group { get; set; }
        [JsonProperty("From_Cts")]
        public float? From_Cts { get; set; }
        [JsonProperty("To_Cts")]
        public float? To_Cts { get; set; }
        [JsonProperty("Discount")]
        public float? Discount { get; set; }

        [JsonProperty("Sub_Process_Id")]
        public int? Sub_Process_Id { get; set; }
        [JsonProperty("From_Days")]
        public int? From_Days { get; set; }
        [JsonProperty("To_Days")]
        public int? To_Days { get; set; }
        [JsonProperty("Amount")]
        public float? Amount { get; set; }
    }
}
