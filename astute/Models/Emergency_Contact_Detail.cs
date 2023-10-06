using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Emergency_Contact_Detail
    {
        [Key]
        public int Emergency_Contact_Detail_Id { get; set; }
        public int? Employee_Id { get; set; }
        public string? Name { get; set; }
        public string? Relation { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
