using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class City_Master_Export
    {
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        [Key]
        public int? Order_No { get; set; }
        public bool? Status { get; set; }
        public string? Std_Code { get; set; }
    }
}
