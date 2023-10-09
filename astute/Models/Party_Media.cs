using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Party_Media
    {
        [Key]
        public int Party_Media_Id { get; set; }
        public int? Party_Id { get; set; }
        public int? Cat_val_Id { get; set; }
        public string? ID { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
