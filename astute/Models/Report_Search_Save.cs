using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Report_Search_Save
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }        
        public string? Search_Value { get; set; }
    }
}
