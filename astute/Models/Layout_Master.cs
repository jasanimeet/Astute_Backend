using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Layout_Master
    {
        public Layout_Master()
        {
            Layout_Details = new List<Layout_Detail>();
         }
        [Key]
        public int Layout_Id { get; set; }
        public Int16 Menu_id { get; set; }
        public int Employee_id { get; set; }
        [NotMapped]
        public IList<Layout_Detail> Layout_Details { get; set; }
    }
}
