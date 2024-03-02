using System.Collections.Generic;

namespace astute.Models
{
    public class Stock_Avalibility
    {
        public string stock_Id { get; set; }
        public int? iPgNo { get; set; }
        public int? iPgSize { get; set; }
        public IList<Report_Sorting> iSort { get; set; } = new List<Report_Sorting>();

    }
}
