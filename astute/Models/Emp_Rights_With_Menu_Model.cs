using System;
using System.Collections.Generic;

namespace astute.Models
{
    public partial class Emp_Rights_With_Menu_Model
    {
        public int EmployeeId { get; set; }
    }

    public partial class Emp_Rights_List_Model
    {
        public Int16 Menu_Id { get; set; }
        public string MenuName { get; set; }
        public string Caption { get; set; }
        public RightsModel Rights_Model { get; set; }

        public IList<Emp_Rights_List_Model> Sub_Menu_Rights { get; set; } = new List<Emp_Rights_List_Model>();
    }

    public partial class RightsModel
    {
        public int Menu_Id { get; set; }
        public bool Insert_Allow { get; set; }
        public bool Update_Allow { get; set; }
        public bool Delete_Allow { get; set; }
        public bool Excel_Allow { get; set; }
        public bool Print_Allow { get; set; }
        public bool Query_Allow { get; set; }
    }
}
