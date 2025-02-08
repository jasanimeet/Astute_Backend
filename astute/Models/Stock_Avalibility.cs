using System.Collections.Generic;

namespace astute.Models
{
    public class Stock_Avalibility
    {
        public string stock_Id { get; set; }
        public string stock_Type { get; set; }
        public string supp_Stock_Id { get; set; }
        public int? party_Id { get; set; }
        public int? iPgNo { get; set; }
        public int? iPgSize { get; set; }
        public IList<Report_Sorting> iSort { get; set; } = new List<Report_Sorting>();
        public string? excel_Format { get; set; } 

    }

    public class Stock_Avalibility_Send_Email
    {
        public string? To_Email { get; set; }
        public string? Remarks { get; set; }
        public string stock_Id { get; set; }
        public string stock_Type { get; set; }
        public int? party_Id { get; set; }
        public int? iPgNo { get; set; }
        public int? iPgSize { get; set; }
        public IList<Report_Sorting> iSort { get; set; } = new List<Report_Sorting>();
        public bool? Send_From_Default { get; set; }
        public string? excel_Format { get; set; }

    }
    public class Stock_Avalibility_Values
    {
        public string Stock_Id { get; set; }
        public string Cost_Disc { get; set; }
        public string Cost_Amount { get; set; }
        public string Offer_Disc { get; set; }
        public string Offer_Amount { get; set; }
    }
}
