using System;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Menu_Mas
    {
        [Key]
        public Int16 Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public string Caption { get; set; }
        public Int16? Parent_Id { get; set;}
        public string Menu_type { get; set; }
        public string? Short_Key { get; set; }
        public Int16 Order_No { get; set; }
        public bool Status { get; set; }
        public string? Module_Path { get; set; }
    }
}
