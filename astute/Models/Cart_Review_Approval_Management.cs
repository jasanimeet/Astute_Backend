using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;

namespace astute.Models
{
    [Keyless]
    public partial class Cart_Model
    {
        public int? User_Id { get; set; }
        public string? Customer_Name { get; set; }
        public string? Remarks { get; set; }
        public int? Validity_Days { get; set; }
        public IList<Cart_Detail> Cart_Detail { get; set; } = new List<Cart_Detail>();

    }

    public partial class Cart_Detail
    {

        public int? Supp_Stock_Id { get; set; }
        public double? Current_Base_Disc { get; set; }
        public double? Current_Base_Amt { get; set; }
        public double? Current_Final_Disc { get; set; }
        public double? Current_Final_Amt { get; set; }
        public double? Current_Final_Disc_Max_Slab { get; set; }
        public double? Current_Final_Amt_Max_Slab { get; set; }
        public double? Buyer_Disc { get; set; }
        public double? Buyer_Amt { get; set; }
        public double? Buyer_Price_Per_Cts { get; set; }
        public double? Expected_Final_Disc { get; set; }
        public double? Expected_Final_Amt { get; set; }
        public string? Cart_Status { get; set; }
    }
}
