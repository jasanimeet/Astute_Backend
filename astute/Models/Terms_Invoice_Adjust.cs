﻿namespace astute.Models
{
    public class Terms_Invoice_Adjust
    {
        public int Id { get; set; }
        public int Account_Trans_Detail_Id { get; set; }
        public int? Purchase_Master_Id { get; set; }
        public int? Transaction_Master_Id { get; set; }
        public int Currency_Id { get; set; }
        public float Ex_Rate { get; set; }
        public int Terms_Id { get; set; }
        public decimal Terms_Amount { get; set; }
        public decimal? Paid_Amount { get; set; }
        public bool? Is_Adjust { get; set; }
        public int? Adjust_Account_Id { get; set; }
        public decimal? Adjust_Amount { get; set; }
        public decimal? OS_Amount { get; set; }
        public int? Adjust_Account_Trans_Detail_Id { get; set; }
    }
}
