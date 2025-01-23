using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Employee_Fortune_Order_Master
    {
        [Key]
        public int Employee_Id { get; set; }
        public string User_Type { get; set; }
        public int Fortune_Id { get; set; }
        public string Assist_By { get; set; }
        public string Company_Email { get; set; }
        public string Company_Name { get; set; }

    }
}
