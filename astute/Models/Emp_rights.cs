using System;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Emp_rights
    {
        [Key]
        public Int16 Menu_Id { get; set; }
        public int Employee_Id { get; set; }
        public bool Insert_Allow { get; set; }
        public bool Update_Allow { get; set; }
        public bool Delete_Allow { get; set; }
        public bool Excel_Allow { get; set; }
        public bool Print_Allow { get; set; }
        public bool Query_Allow { get; set; }
    }
}
