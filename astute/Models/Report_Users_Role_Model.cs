using System.Collections.Generic;

namespace astute.Models
{
    public class Report_Users_Role_Model
    {
        public string User_Ids { get; set; }

        public IList<Report_Roles> Report_Roles { get; set; }

    }

    public class Report_Roles
    {
        public int Rd_Id { get; set; }
        public string? Display_Name { get; set; }
        public string? Display_Type { get; set; }
        public string? Order_By { get; set; }
        public int? Short_No { get; set; }
        public int? Width { get; set; }
        public string? Column_Format { get; set; }
        public string? Alignment { get; set; }
        public string? Fore_Colour { get; set; }
        public string? Back_Colour { get; set; }
        public bool? IsBold { get; set; }
    }
}
