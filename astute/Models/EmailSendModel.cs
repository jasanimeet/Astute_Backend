namespace astute.Models
{
    public partial class EmailSendModel
    {
        public int EmployeeId { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string EmailText { get; set; }
    }
}
