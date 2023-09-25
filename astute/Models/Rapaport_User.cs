using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Rapaport_User
    {
        [Key]
        public string Rap_User { get; set; }
        public string Rap_Password { get; set; }
    }
}
