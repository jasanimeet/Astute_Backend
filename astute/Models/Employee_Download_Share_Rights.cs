using System;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Employee_Download_Share_Rights
    {
        [Key]
        public Int16 Menu_Id { get; set; }
        public int Employee_Id { get; set; }
        public bool D_Default_Excel { get; set; }
        public bool D_Custom_Excel { get; set; }
        public bool D_Image { get; set; }
        public bool D_Video { get; set; }
        public bool D_Certificate { get; set; }
        public bool S_Default_Excel { get; set; }
        public bool S_Custom_Excel { get; set; }
        public bool S_Image { get; set; }
        public bool S_Video { get; set; }
        public bool S_Certificate { get; set; }
    }
}
