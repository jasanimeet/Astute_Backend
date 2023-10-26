using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Terms_Master
    {
        [Key]
        public int Terms_Id { get; set; }
        public string? Terms { get; set; }
        public Int16? Term_Days { get; set; }
        public Int16? Order_No { get; set; }
        public Int16? Sort_No { get; set; }
        public bool? Status { get; set; }
        [NotMapped]
        public bool IsForceInsert { get; set; }
    }
}
