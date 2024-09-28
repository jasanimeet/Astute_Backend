using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Emp_rights_Model
    {
        [Key]
        public int Employee_Id { get; set; }
        public IList<Emp_Right_Post_Model> Emp_Right_Post_Model { get; set; }
    }

    public partial class Emp_Right_Post_Model
    {
        [Key]
        public Int16 Menu_Id { get; set; }
        public bool Insert_Allow { get; set; }
        public bool Update_Allow { get; set; }
        public bool Delete_Allow { get; set; }
        public bool Excel_Allow { get; set; }
        public bool Print_Allow { get; set; }
        public bool Query_Allow { get; set; }
    }
}
