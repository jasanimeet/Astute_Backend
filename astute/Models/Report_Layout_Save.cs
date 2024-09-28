using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public class Report_Layout_Save
    {
        [Key]
        public int Id { get; set; }
        public int? User_Id { get; set; }
        public int? Rm_Id { get; set; }
        public string? Name { get; set; }
        public bool? Status { get; set; }
        [NotMapped]
        public IList<Report_Layout_Save_Detail> Report_Layout_Save_Detail_List { get; set; }
    }
}
