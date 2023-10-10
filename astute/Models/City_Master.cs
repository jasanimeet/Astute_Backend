using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class City_Master
    {
        [Key]
        public int City_Id { get; set; }
        public string? City { get; set; }
        public int? State_Id { get; set; }
        public string State { get; set; }
        public int? Country_id { get; set; }
        public string Country { get; set; }
        public int? Order_No { get; set; }
        public int? Sort_No { get;set; }
        public bool Status { get; set; }
        public string? Std_Code { get; set; }
        public int? iTotalRec { get; set; }
        [NotMapped]
        public bool IsForceInsert { get; set; }
    }
}
