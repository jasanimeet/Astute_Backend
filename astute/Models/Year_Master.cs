using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Year_Master
    {
        [Key]
        public int Year_Id { get; set; }
        public string? Year { get; set; }
        public bool? Current_Status { get; set; }
        public bool? Status { get; set; }
        public string? From_Date { get; set; }
        public string? To_Date { get; set; }        
    }
}
