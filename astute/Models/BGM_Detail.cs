using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class BGM_Detail
    {
        [Key]
        public int Id { get; set; }
        public int? BGM_Id { get; set; }
        public string? BGM { get; set; }
        public int? Shade { get; set; }
        public string? Shade_Category { get; set; }
        public int? Milky { get; set; }
        public string? Milky_Category { get; set; }
        public bool? Status { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
