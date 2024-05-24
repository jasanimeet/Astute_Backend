using System.Collections.Generic;

namespace astute.Models
{
    public class Account_Trans_Master
    {
        public int account_Trans_Id { get; set; }
        public string trans_Type { get; set; }
        public string? invoice_No { get; set; }
        public int currency_Id { get; set; }
        public int company_Id { get; set; }
        public int year_Id { get; set; }
        public int account_Id { get; set; }
        public decimal rate { get; set; }
        public List<Account_Trans_Detail_Master> account_Trans_Detail { get; set; }

}

    public class Account_Trans_Detail_Master
    {
        public int account_Trans_Detail_Id { get; set; }
        public int by_Account { get; set; }
        public string by_Type { get; set; }
        public int to_Account { get; set; }
        public string to_Type { get; set; }
        public decimal amount { get; set; }
        public string? narration { get; set; }

    }
}
