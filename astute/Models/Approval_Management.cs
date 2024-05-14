using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Approval_Management
    {
        public string? Ids { get; set; }
        public int? User_Id { get; set; }
        public bool? Is_Approved { get; set; }
        public bool? Is_Rejected { get; set; }
        public string? Remarks { get; set; }
    }
    public class Approval_Management_Status
    {
        public string? Ids { get; set; }
        public int? User_Id { get; set; }
        public string? Status { get; set; }
    }

    //public partial class Approval_Management_Create_Update
    //{
    //    [Key]
    //    public string? Supp_Stock_Id { get; set; }
    //    public string? Cart_Id { get; set; }
    //    public int? User_Id { get; set; }
    //    public string? Remarks { get; set; }
    //    public string? Status { get; set; }
    //}

    public partial class Approval_Management_Create_Update
    {   
        public int Id { get; set; }
        public int? User_Id { get; set; }
        public string? Remarks { get; set; }
        public string? Status { get; set; }
        public object Approval_Detail { get; set; }
    }

    public class Approval_Detail
    {
        public int? Id { get; set; }
        public int? Supp_Stock_Id { get; set; }
        [JsonProperty("Cart_Id")]
        public object? Cart_Id { get; set; }
        [JsonProperty("BUYER DISC")]
        public object? Buyer_Disc { get; set; }

        [JsonProperty("BUYER AMOUNT")]
        public object? Buyer_Amt { get; set; }
        [JsonProperty("EXPECTED FINAL DISC")]
        public object? Expected_Final_Disc { get; set; }

        [JsonProperty("EXPECTED FINAL AMT")]
        public object? Expected_Final_Amt { get; set; }

        [JsonProperty("CART OFFER DISC")]
        public object? Offer_Disc { get; set; }

        [JsonProperty("CART OFFER AMT")]
        public object? Offer_Amt { get; set; }

        [JsonProperty("OFFER DISC")]
        public object? Offer_Disc_1 { get; set; }

        [JsonProperty("OFFER AMT")]
        public object? Offer_Amt_1 { get; set; }

    }
}
