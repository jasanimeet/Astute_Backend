namespace astute.Models
{
    public partial class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? FCMToken_ANDROID { get; set; }
        public string? FCMToken_IPHONE { get; set; }
    }
}
