using Newtonsoft.Json;

namespace astute.Models
{
    public partial class Manual_Url_Transfer
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Sunrise_Stock_Id")]
        public string Sunrise_Stock_Id { get; set; }
        [JsonProperty("Image_URL")]
        public string Image_URL { get; set; }
        [JsonProperty("Video_URL")]
        public string Video_URL { get; set; }
    }
}
