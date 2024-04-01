using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public class Customer_Party_Api
    {
        [Key]
        public int? API_Id { get; set; }
        public int? Party_Id { get; set; }
        public string? Format_Type { get; set; }
        public string? IP { get; set; }
        public string? Country { get; set; }
        public string? Map_Flag { get; set; }
        public bool? API_Status { get; set; }
        [NotMapped]
        public IList<Customer_Column_Caption> Customer_Column_Caption { get; set; } = new List<Customer_Column_Caption>();

    }
}
