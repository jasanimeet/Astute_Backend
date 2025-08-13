using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Lab_User_Master
    {
        [Key]
        public int Id { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public bool Active_Status { get; set; }
        public string? User_Type { get; set; }
        public bool Stock_View { get; set; }
        public bool Stock_Download { get; set; }
        public bool Order_Placed { get; set; }
        public bool Enable_Status { get; set; }
        public string? Last_Login_Date { get; set; }
        public string Query_Flag { get; set; }
        public string? User_For { get; set; }
        public string? Email { get; set; }
        public bool? Show_Amount { get; set; }
        public bool? Order_History { get; set; }
        public bool? Display_Own_Records { get; set; }
        public bool? Sub_User { get; set; }
        public int? Registration_Master_Id { get; set; }
    }

    public class Lab_User_Detail
    {
        public int Party_Id { get; set; }
        public IList<Lab_User_Master> Lab_User_Masters { get; set; } = new List<Lab_User_Master>();
    }
}
