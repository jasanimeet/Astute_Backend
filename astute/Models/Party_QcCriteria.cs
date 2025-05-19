using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Party_QcCriteria
    {
        [Key]
        public int QcCriteria_Id { get; set; }
        public int? Party_Id { get; set; }
        public string? Purchase { get; set; }
        public float? FromCts { get; set; }
        public float? ToCts { get; set; }
        public string? Presold { get; set; }
        public bool? Status { get; set; }
        public string? Contract { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
