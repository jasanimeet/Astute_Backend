using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Party_Master
    {
        [Key]
        public int Party_Id { get; set; }
        public string? Party_Type { get; set; }
        public string? Party_Type_Value { get; set; }
        public string? Party_Code { get; set; }
        public string? Adress_1 { get; set; }
        public string? Adress_2 { get; set; }
        public string? Adress_3 { get; set; }
        public int? City_Id { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PinCode { get; set; }
        public string? Mobile_1 { get; set; }
        public string? Mobile_2 { get; set; }
        public string? Phone_1 { get; set; }
        public string? Phone_2 { get; set; }
        public string? Fax { get; set; }
        public string? Email_1 { get; set; }
        public string? Email_2 { get; set; }
        public int? Comp_Bank { get; set; }
        public string? Bank_Name { get; set; }
        public string? Branch_Name { get; set; }
        public string? Party_Name { get; set; }
        public int? Ship_PartyId { get; set; }
        public string? Ship_Party_Name { get; set; }
        public int? Final_Customer_Id { get; set; }
        public string? Website { get; set; }
        public string? Bank_Currency { get; set; }
        public int? Payment_Terms { get; set; }
        public string? Payment_Terms_Value { get; set; }
        public string? Cust_Freight_Account_No { get; set; }
        [NotMapped]
        public IList<Party_Contact> Party_Contact_List { get; set; } = new List<Party_Contact>();
        [NotMapped]
        public IList<Party_Assist> Party_Assist_List { get; set; } = new List<Party_Assist>();
        [NotMapped]
        public IList<Party_Bank> Party_Bank_List { get; set; } = new List<Party_Bank>();
        [NotMapped]
        public IList<Party_Document> Party_Document_List { get; set; } = new List<Party_Document>();
        [NotMapped]
        public IList<Party_Media> Party_Media_List { get; set; } = new List<Party_Media>();
        [NotMapped]
        public IList<Party_Shipping> Party_Shipping_List { get; set; } = new List<Party_Shipping>();
    }
}
