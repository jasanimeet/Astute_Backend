using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Customer_Detail
    {
        [Key]
        public int? User_Pricing_Id { get; set; }
        public string? Map_Flag { get; set; }
        public Customer_Party_Api Customer_Party_Api { get; set; }
        public Customer_Party_FTP Customer_Party_FTP { get; set; }
        public Customer_Party_File Customer_Party_File { get; set; }

    }
}
