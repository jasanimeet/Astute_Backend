using System.Collections.Generic;
using System;

namespace astute.Models
{
    public partial class MenuDownloadShareMasterModel
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
        public bool Excel_Allow { get; set; }
        public bool Print_Allow { get; set; }

        public Menu_Download_Share_Rights_Model Menu_Rights { get; set; }
        public IList<MenuDownloadShareMasterModel> SubMenu { get; set; } = new List<MenuDownloadShareMasterModel>();
    }
}
