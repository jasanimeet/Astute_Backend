using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class City_Master_Combo
    {
        [Key]
        public int City_Id { get; set; }
        public string? City { get; set; }
        public string? CSC { get; set; }
    }
}
