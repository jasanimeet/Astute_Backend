using System.Collections.Generic;

namespace astute.Models
{
    public partial class Stock_Email_Model
    {
        public string? Stock_Upload_Type { get; set; }
        public string? To_Email { get; set; }
        public string? Remarks { get; set; }
        public string? Supplier_Ref_No { get; set; }
        public IList<IList<Report_Multiple_Filter_Parameter>> Report_Filter_Parameter_List { get; set; } = new List<IList<Report_Multiple_Filter_Parameter>>();
        public bool? Send_From_Default { get; set; }
        public string? User_Format { get; set; }
    }

    public partial class Cart_Approval_Order_Email_Model
    {
        public string? To_Email { get; set; }
        public string? Remarks { get; set; }
        public int id { get; set; }
        public IList<Report_Filter_Parameter> Report_Filter_Parameter { get; set; } = new List<Report_Filter_Parameter>();
        public bool? Send_From_Default { get; set; }
    }
}
