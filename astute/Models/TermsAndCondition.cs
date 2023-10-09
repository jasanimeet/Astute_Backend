using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class TermsAndCondition
    {
        [Key]
        public int Condition_Id { get; set; }
        public string? Conditions { get; set; }
        public int? Process_Id { get; set; }
        public string? Process_Name { get; set; }
        public string? Start_Date { get; set; }
        public Int16? Order_No { get; set; }
        public Int16? Sort_No { get; set; }
        public bool? Status { get; set; }
        [NotMapped]
        public bool? IsForceInsert { get; set; }
    }
}
