using System.Collections.Generic;

namespace astute.Models
{
    public class Excel_Model
    {   
        public string? excel_Format { get; set; }
        public string? supplier_Ref_No { get; set; }
        public IList<Report_Filter_Parameter> Report_Filter_Parameter { get; set; } = new List<Report_Filter_Parameter>();
    }
}
