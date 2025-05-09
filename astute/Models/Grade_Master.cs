using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Grade_Master
    {
        [Key]
        public int Trans_Id { get; set; }
        [Required]
        public string Grade { get; set; }
        public string? Grade_Type { get; set; }
        public string? Created_By { get; set; }
        public string? Created_Date { get; set; }
        public string? Created_Time { get; set; }
        public string? Updated_By { get; set; }
        public string? Updated_Date { get; set; }
        public string? Updated_Time { get; set; }
        [NotMapped]
        public IList<Grade_Detail> Grade_Detail_List { get; set; } = new List<Grade_Detail>();
       
    }
}
