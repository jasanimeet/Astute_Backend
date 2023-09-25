using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Category_Master
    {
        [Key]
        public int Cat_Id { get; set; }
        public string Column_Name { get; set; }
        public string Display_Name { get; set; }
        public bool Status { get; set; }
        public int Col_Id { get; set; }
    }
}
