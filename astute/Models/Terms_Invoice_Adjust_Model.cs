using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;

namespace astute.Models
{
    public class Terms_Invoice_Adjust_Model
    {
        public int? Id { get; set; }
        public int? Trans_Id { get; set; }
        public int? Process_Id { get; set; }
        public int? Company_Id { get; set; }
        public int? Year_Id { get; set; }
        public int? Expense_Id { get; set; }
        public string Trans_Date { get; set; }
        public string Trans_Time { get; set; }
        public int? By_Account { get; set; }
        public string By_Type { get; set; }
        public int? To_Account { get; set; }
        public string To_Type { get; set; }
        public int? Currency_Id { get; set; }
        public float? Ex_Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal Amount_In_US { get; set; }
        public string Voucher_No { get;  set; }
        public string Remarks { get;  set; }
        public string Source_Party { get; set; }
        public string Third_Party { get; set; }
        public List<Terms_Invoice_Adjust> Terms_Invoice_Adjust { get; set; }
    }
}
