using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Holiday_Master
    {
        [Key]
        public int Holiday_Id { get; set; }
        public string? Date { get; set; }
        public string? Start_Time { get; set; }
        public string? End_Time { get; set;}
        public bool? Holiday_Flag { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
