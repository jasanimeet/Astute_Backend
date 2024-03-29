using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Customer_Detail
    {
        [Key]
        public int Party_Id { get; set; }
        public string? User_Id { get; set; }
        public Customer_Party_Api Customer_Party_Api { get; set; }
        public Customer_Party_FTP Customer_Party_FTP { get; set; }
        public Customer_Party_File Customer_Party_File { get; set; }

        public IList<Customer_Column_Caption> Customer_Column_Caption { get; set; } = new List<Customer_Column_Caption>();

    }
}
