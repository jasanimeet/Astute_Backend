using System.Collections.Generic;

namespace astute.Models
{
    public class Import_Excel
    {
        public Import_Master Import_Master { get; set; }
        public List<Import_Detail> Import_Details { get; set; }
    }
}
