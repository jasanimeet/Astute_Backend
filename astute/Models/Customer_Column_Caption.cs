using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    [Keyless]
    public class Customer_Column_Caption
    {
        public int Col_Id { get; set; }
        public string Display_Name { get; set; }
        public string Caption_Name { get; set; }
        public bool Status { get; set; }

    }
}
