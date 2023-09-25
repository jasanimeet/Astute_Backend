using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Layout_Detail
    {
        [Key]
        public int LayoutDet_ID { get; set; }
        public int Layout_Id { get; set; }
        public string Individual_Layout_Id { get; set; }
        public string Individual_Layout_Name { get; set; }
        public bool Status { get; set; }
        [NotMapped]
        public IList<Layout_Column> Layout_Columns { get; set; }
    }
}
