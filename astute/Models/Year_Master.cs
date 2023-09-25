using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Year_Master
    {
        [Key]
        public Int16 Year_Id { get; set; }
        public string Year { get; set; }
        public bool Current_Status { get; set; }
        public bool Status { get; set; }
        public DateTime From_Date { get; set; }
        public DateTime To_Date { get; set; }        
    }
}
