using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Report_Master
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Sp_Name { get; set; }
        public bool? Status { get; set; }
    }
}
