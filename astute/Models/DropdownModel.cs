using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class DropdownModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
