using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class State_Master
    {
        [Key]
        public int State_Id { get; set; }
        public string State { get; set; }
        [Column("Country_id")]
        public int Country_Id { get; set; }
        public string Country { get; set; }
        public int? Order_No { get; set; }
        public int? Sort_No { get; set; }
        public bool Status { get; set; }
        [NotMapped]
        public bool IsForceInsert { get; set; }
    }
}
