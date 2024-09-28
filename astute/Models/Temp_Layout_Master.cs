using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Temp_Layout_Master
    {
        [Key]
        public int Layout_Id { get; set; }
        public string? Layout_Name { get; set; }
        public Int16? Menu_Id { get; set; }
        public int? Employee_Id { get; set; }
        public bool? Status { get; set; }
        [NotMapped]
        public IList<Temp_Layout_Detail> Temp_Layout_Detail_List { get; set; }
    }
}
