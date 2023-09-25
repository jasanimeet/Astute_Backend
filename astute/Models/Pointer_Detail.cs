using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Pointer_Detail
    {
        [Key]
        public int Pointer_Det_Id { get; set; }
        public int Pointer_id { get; set; }
        public decimal From_Cts { get; set; }
        public decimal To_Cts { get; set; }
        public Int16 Order_No { get; set; }
        public Int16 Sort_No { get; set; }
        public bool Status { get; set; }
        public string Sub_Pointer { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
