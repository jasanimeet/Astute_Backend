using Newtonsoft.Json;

namespace astute.Models
{
    public class Notification_Menu
    {
        [JsonProperty("Notification_Id")]
        public int? Notification_Id { get; set; }

        [JsonProperty("Notification_Title")]
        public string? Notification_Title { get; set; }

        [JsonProperty("Employee")]
        public string? Employee { get; set; }
    }
    public class Notification_Menu_List
    {
        public object? Notification_Menu { get; set; }
    }
}
