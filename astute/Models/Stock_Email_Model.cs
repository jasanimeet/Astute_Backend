using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Collections.Generic;

namespace astute.Models
{
    public partial class Stock_Email_Model
    {   
        public string? Stock_Upload_Type { get; set; }
        public string? To_Email { get; set; }
        public string? Remarks { get; set; }
        public IList<Report_Filter_Parameter> Report_Filter_Parameter { get; set; } = new List<Report_Filter_Parameter>();
    }
}
