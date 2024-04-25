using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace astute.Models
{
    [Keyless]
    public partial class Cart_Model
    {
        public int Id { get; set; }
        public int? User_Id { get; set; }
        public string? Customer_Name { get; set; }
        public string? Remarks { get; set; }
        public int? Validity_Days { get; set; }
        public object Cart_Detail { get; set; }

    }

    public partial class Cart_Detail
    {
        public int? Id { get; set; }

        public int? Supp_Stock_Id { get; set; }

        [JsonProperty("CTS")]
        public object? Cts { get; set; }

        [JsonProperty("BASE DISC")]
        public object? Cart_Base_Disc { get; set; }

        [JsonProperty("BASE AMOUNT")]
        public object? Cart_Base_Amt { get; set; }

        [JsonProperty("COST DISC")]
        public object? Cart_Final_Disc { get; set; }

        [JsonProperty("COST AMOUNT")]
        public object? Cart_Final_Amt { get; set; }

        [JsonProperty("MAX SLAB BASE DISC")]
        public object? Cart_Final_Disc_Max_Slab { get; set; }

        [JsonProperty("MAX SLAB BASE AMOUNT")]
        public object? Cart_Final_Amt_Max_Slab { get; set; }
        [JsonProperty("OFFER DISC")]
        public object? Cart_Offer_Disc { get; set; }
        [JsonProperty("OFFER AMOUNT")]
        public object? Cart_Offer_Amt { get; set; }
        [JsonProperty("BUYER DISC")]
        public object? Buyer_Disc { get; set; }

        [JsonProperty("BUYER AMOUNT")]
        public object? Buyer_Amt { get; set; }

        [JsonProperty("BUYER PRICE PER CTS")]
        public object? Buyer_Price_Per_Cts { get; set; }

        [JsonProperty("EXPECTED FINAL DISC")]
        public object? Expected_Final_Disc { get; set; }

        [JsonProperty("EXPECTED FINAL AMT")]
        public object? Expected_Final_Amt { get; set; }
        public string? Cart_Status { get; set; }
    }
}
