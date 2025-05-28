using Newtonsoft.Json;

namespace astute.Models
{
    public class Purchase_Detail_QC_Reply
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }

        [JsonProperty("QC_Reply_Status")]
        public string? QC_Reply_Status { get; set; }
    }
}
