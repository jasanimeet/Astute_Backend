using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Approval_Management
    {
        public string? Ids { get; set; }
        public int? User_Id { get; set; }
        public bool? Is_Approved { get; set; }
        public bool? Is_Rejected { get; set; }
        public string? Remarks { get; set; }
    }

    public partial class Approval_Management_Create_Update
    {
        [Key]
        public string? Supp_Stock_Id { get; set; }
        public string? Cart_Id { get; set; }
        public int? User_Id { get; set; }
        public string? Remarks { get; set; }
        public string? Status { get; set; }
    }
}
