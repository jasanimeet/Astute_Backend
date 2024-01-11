using System.Collections.Generic;

namespace astute.Models
{
    public class Report_Users_Role_Save_Layout_Model
    {
        public string User_Ids { get; set; }

        public IList<Report_Role_Save_Layout_Model> Report_Role_Save_Layout_Models { get; set; }

    }

    public class Report_Role_Save_Layout_Model
    {
        public int Id { get; set; }
    }
}
