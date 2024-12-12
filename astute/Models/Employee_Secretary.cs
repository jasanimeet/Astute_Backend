using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Employee_Secretary
    {
        [Key]
        public int Employee_Secretary_Id { get; set; } 
        public int Employee_Id { get; set; }
        public int Secretary_Id  { get; set; }
        public string Secretary_Name { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
