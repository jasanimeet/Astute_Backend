using System;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Bank_Master
    {
        [Key]
        public int Bank_Id { get; set; }
        public string? Bank_Name { get; set; }
        public string? Bank_Code { get; set; }
        public string? Branch_Name { get; set; }
        public string? Branch_Code { get; set; }
        public string? Branch_Address { get; set; }
        public string? Ifsc_Code { get; set; }
        public string? Account_No { get; set; }
        public string? Correspondent_Bank { get; set; }
        public string? Correspondent_Bank_Addres { get; set; }
        public string? Correspondent_Ifsc_Code { get; set; }
        public string? Correspondent_Bank_Account_No { get; set; }
        public Int16? Order_No { get; set; }
        public Int16? Sort_No { get; set; }
        public bool? Status { get; set; }
        public int? Currency_Id { get; set; }
        public string? Currency { get; set; }
        public string? Currency_Name { get; set; }
        public int? Account_Type { get; set; }
        public string? Account_Type_Value { get; set; }
    }
}
