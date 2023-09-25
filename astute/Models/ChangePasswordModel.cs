namespace astute.Models
{
    public partial class ChangePasswordModel
    {
        public int EmoployeeId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
