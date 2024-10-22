using System.Collections.Generic;

namespace astute.Models
{
    public class Order_Excel_Email_Model
    {
        public string? To_Email { get; set; }
        public string? Majal_Excel_Exist { get; set; }
        public string? Stock_Id { get; set; }
        public string? Order_Id { get; set; }
        public int? Sub_Order_Id { get; set; }
        public bool? Send_From_Default { get; set; }
    }
}
