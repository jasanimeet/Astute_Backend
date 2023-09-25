using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Rapaport_Clarity_Value
    {
        [Key]
        public string Cat_Name { get; set; }
        public int Cat_Id { get; set; }
        public string Rapaport_Name { get; set; }
    }
}
