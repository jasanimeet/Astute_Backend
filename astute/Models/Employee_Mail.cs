using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Employee_Mail
    {
        [Key]
        public int? Employee_Id { get; set; }
        public string? Email_id { get; set; }
        public string? Email_Password { get; set; }
        public string? SMTP_Server { get; set; }
        public string? SMTP_Server_Address { get; set; }
        public int? SMTP_Port { get; set; }
        public bool? Enable_SSL { get; set; }
        public bool? Is_Default { get; set; }
    }
}
