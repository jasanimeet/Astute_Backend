using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Ac_Group_Detail
    {
        [Key]
        public int Ac_Group_Det_Id { get; set; }
        public int? Ac_Group_Id { get; set;}
        public string? Ac_Group_Det_Name { get; set; }
        public string? Trans_Type { get; set; }
        public string? Basic_Group { get; set; }
        public int? Opp_Group_Det_Id { get; set; }
        public int? Parent_Group { get; set; }
        public bool? Status { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
