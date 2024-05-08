using System;
using System.Collections;

namespace astute.Models
{
    public class Account_Master
    {
        public int? Account_Id { get; set; }
        public string? Account_Name { get; set; }
        public string Group { get; set; }
        public string? Sub_Group { get; set; }
        public string? Main_Company { get; set; }
        public bool? Purchase_Expence { get; set; }
        public bool? Sales_Expence { get; set; }
        public bool? Is_Party { get; set; }
    }
}
