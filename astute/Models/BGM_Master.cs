using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class BGM_Master
    {
        [Key]
        public int Bgm_Id { get; set; }
        public string? BGM { get; set; }
        public int? Shade { get; set; }
        public string? Shade_Category { get; set; }
        public int? Milky { get; set; }
        public string? Milky_Category { get; set; }
        public Int16? Sort_No { get; set; }
        public Int16? Order_No { get; set; }
        public bool? Status { get; set; }
    }
}
