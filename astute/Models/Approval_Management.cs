using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Approval_Management
    {
        [Key]
        public string? Supp_Stock_Id { get; set; }
        public string? Cart_Id { get; set; }
        public int? User_Id { get; set; }
        public string? Remarks { get; set; }
        public string? Status { get; set; }
    }
}
