using System.Collections.Generic;

namespace astute.Models
{
    public class Report_User_Roles_Model
    {
        public string User_Id { get; set; }

        public IList<Report_Roles> Report_Roles { get; set; }

    }

    public class Report_Roles
    {
        public int Rd_Id { get; set; }
        public string Display_Type { get; set; }
    }
}
