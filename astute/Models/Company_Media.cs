using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Company_Media
    {
        [Key]
        public int Company_Media_Id { get; set; }
        public int? Cat_Val_Id { get; set; }
        public string? Social_Media_Name { get; set; }
        public int? Company_Id { get; set; }
        public string? Company_Name { get; set; }        
        public string? Media_Detail { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
