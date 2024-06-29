using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Shape_Value
    {
        [Key]
        public string Cat_Name { get; set; }
        public int Cat_Id { get; set; }
    }
}
