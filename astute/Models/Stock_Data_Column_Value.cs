using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Stock_Data_Column_Value
    {
        [Key]
        public string? Column_Value { get; set; }
    }
}
