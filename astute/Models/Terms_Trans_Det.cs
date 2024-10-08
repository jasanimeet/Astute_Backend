﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Terms_Trans_Det
    {
        [Key]
        public int Terms_Trans_Det_Id { get; set; }
        public int Terms_Id { get; set; }
        public decimal? amount { get; set; }
        public int? Seq_No { get; set; }
        public int? Trans_Id { get; set; }
        public string? Trans_Type { get; set; }
        [NotMapped]
        public bool IsForceInsert { get; set; }
    }
}
