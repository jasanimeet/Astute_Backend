using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Employee_Loader
    {
        [Key]
        public int Employee_Id { get; set; }
        public int Loader_Id { get; set; }
        public bool Status { get; set; }
    }
}
