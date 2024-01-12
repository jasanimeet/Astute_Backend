using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Report_Layout_Save
    {
        [Key]
        public int Id { get; set; }
        public int? User_Id { get; set; }
        public string? Name { get; set; }
        public string? Layout_Value { get; set; }
    }
}
