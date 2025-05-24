using System;
using System.Collections.Generic;

namespace astute.Models
{
    public partial class MenuMasterModel
    {
        public Int16 Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public string Caption { get; set; }
        public Int16 Parent_Id { get; set; }
        public string Menu_type { get; set; }
        public string Short_Key { get; set; }
        public Int16 Order_No { get; set; }
        public bool Status { get; set; }
        public string Module_Path { get; set; }
        public Menu_Rights_Model Menu_Rights { get; set; }
        public IList<MenuMasterModel> SubMenu { get; set; } = new List<MenuMasterModel>();
    }

    public partial class Menu_Rights_Model
    {
        public bool Insert_Allow { get; set; }
        public bool Update_Allow { get; set; }
        public bool Delete_Allow { get; set; }
        public bool Excel_Allow { get; set; }
        public bool Print_Allow { get; set; }
        public bool Query_Allow { get; set; }
    }
}
