using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Ac_Group_Master
    {
        [Key]
        public int Ac_Group_Id { get; set; }
        public string? Ac_Group_Name { get; set;}
        public string? Trans_Type { get; set;}
        public string? Basic_Group { get; set;}
        public bool? Status { get; set;}
        [NotMapped]
        public IList<Ac_Group_Detail> Ac_Group_Detail_List { get; set; } = new List<Ac_Group_Detail>();
    }
}
