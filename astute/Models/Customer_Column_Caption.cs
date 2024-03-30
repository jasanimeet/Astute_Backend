using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public class Customer_Column_Caption
    {
        public int Col_Id { get; set; }
        [NotMapped]
        public string Display_Name { get; set; }
        public string Caption_Name { get; set; }
        public bool Status { get; set; }
        public string? Upload_Method { get; set; }

    }
}
