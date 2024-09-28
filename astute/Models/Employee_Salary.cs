using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Employee_Salary
    {
        [Key]
        public int Employee_Salary_Id { get; set; }
        public int? Employee_Id { get; set; }
        public int? Salary { get; set; }
        public string? Start_Date { get; set; }
        public string? Salary_Type { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
