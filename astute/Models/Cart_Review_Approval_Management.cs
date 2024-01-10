using Microsoft.EntityFrameworkCore;

namespace astute.Models
{
    [Keyless]
    public partial class Cart_Review_Approval_Management
    {
        public string? Supp_Stock_Ids { get; set; }
        public int? User_Id { get; set; }
        public string? Upload_Type { get; set; }
    }
}
