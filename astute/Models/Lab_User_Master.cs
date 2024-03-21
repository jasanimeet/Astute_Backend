using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Lab_User_Master
    {
        [Key]
        public int Id { get; set; }
        public int? Party_Id { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public bool Active_Status { get; set; }
        public string? User_Type { get; set; }
        public bool Stock_View {  get; set; }
        public bool Stock_Download { get; set; }
        public bool Order_Placed { get; set; }
        public bool Enable_Status { get; set; }
        public string? Last_Login_Date { get; set; }
        public string Query_Flag { get; set; }
    }
}
