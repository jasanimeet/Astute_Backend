using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Country_Master
    {
        [Key]
        public int Country_Id { get; set; }
        public string Country { get; set; }
        public string? Isd_Code { get; set; }
        public Int16? Order_No { get; set; }
        public Int16? Sort_No { get; set; }
        public bool Status { get; set; }
        public string? Short_Code { get; set; }
        [NotMapped]
        public bool? IsForceInsert { get; set; }
    }
}
