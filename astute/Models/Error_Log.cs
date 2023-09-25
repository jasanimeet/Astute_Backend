using System;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Error_Log
    {
        [Key]
        public int Id { get; set; }
        public string Error_Message { get; set; }
        public string Module_Name { get; set; }
        public DateTime Arise_Date { get; set; }
        public string Error_Trace { get; set; }
    }
}
