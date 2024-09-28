using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Pointer_Master
    {
        [Key]
        public int Pointer_Id { get; set; }
        public string? Pointer_Name { get; set; }
        public decimal? From_Cts { get; set; }
        public decimal? To_Cts { get; set; }
        public int? Pointer_Type { get; set; }
        public string? Pointer_Type_Value { get; set; }
        public Int16? Order_No { get; set; }
        public Int16? Sort_No { get; set; }
        public bool? Status { get; set; }
        [NotMapped]
        public IList<Pointer_Detail> Pointer_Detail_List { get; set; } = new List<Pointer_Detail>();
    }
}
