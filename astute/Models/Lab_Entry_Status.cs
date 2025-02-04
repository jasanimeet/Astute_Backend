using Newtonsoft.Json;

namespace astute.Models
{
    public class Lab_Entry_Status
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }

        [JsonProperty("NEW STATUS")]
        public string? New_Status { get; set; }
    }
    public class Lab_Entry_Status_List 
    {
        public object? Lab_Entry_Status { get; set; }
    }
}
