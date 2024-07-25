using System.Collections.Generic;

namespace astute.Models
{
    public class Account_Trans_Master
    {
        public int account_Trans_Id { get; set; }
        public string mod_Type { get; set; }
        public string? invoice_No { get; set; }
        public int currency { get; set; }
        public int company { get; set; }
        public int year{ get; set; }
        public int account { get; set; }
        public string type { get; set; }
        public decimal rate { get; set; }
        public List<Account_Trans_Detail_Master> account_Trans_Detail { get; set; }

    }

    public class Account_Trans_Detail_Master
    {
        public string voucherNo { get; set; }
        public int account { get; set; }
        public string type { get; set; }
        public decimal amount { get; set; }
        public string? narration { get; set; }
    }

    public class Account_Trans_Purchase_Master
    {
        public int account_Trans_Id { get; set; }
        public string mod_Type { get; set; }
        public string? invoice_No { get; set; }
        public int currency { get; set; }
        public int company { get; set; }
        public int year { get; set; }
        public int account { get; set; }
        public string type { get; set; }
        public decimal rate { get; set; }
        public string remarks { get; set; }
        public string invoice_Date { get; set; }
        public string invoice_Time { get; set; }
        public int supplier_Id { get; set; }
        public List<Account_Trans_Purchase_Detail_Master> account_Trans_Detail { get; set; }
        public List<Terms_Trans_Det> terms_Trans_Dets { get; set; }
        public object expense_Trans_Dets { get; set; }
        public List<InwardDetail> inwardDetails { get; set; }
    }

    public class Account_Trans_Purchase_Detail_Master
    {
        public string voucherNo { get; set; }
        public int account { get; set; }
        public string type { get; set; }
        public decimal amount { get; set; }
        public string? narration { get; set; }
        public int Cat_Val_Id { get; set; }
        public int Parcel_Id { get; set; }
        public int? Pcs { get; set; }
        public decimal? Cts { get; set; }
        public string Remarks { get; set; }
        public decimal? Rate { get; set; }


    }
}
