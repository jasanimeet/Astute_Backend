using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace astute.Models
{
    public class Party_Master_Replica
    {
        [Key]
        public int Party_Id { get; set; }
        public string? Party_Type { get; set; }
        public string? Party_Type_Value { get; set; }
        public string? Party_Code { get; set; }
        [NotMapped]
        public string? Adress_1 { get; set; }
        [NotMapped]
        public string? Adress_2 { get; set; }
        [NotMapped]
        public string? Adress_3 { get; set; }
        public string? Adress { get; set; }
        public int? City_Id { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Country_Code { get; set; }
        public string? Contact_Person { get; set; }
        public string? PinCode { get; set; }
        public string? Mobile_1 { get; set; }
        public string? Mobile_1_Country_Code { get; set; }
        public string? Mobile_2 { get; set; }
        public string? Mobile_2_Country_Code { get; set; }
        public string? Phone_1 { get; set; }
        public string? Phone_1_Country_Code { get; set; }
        public string? Phone_2 { get; set; }
        public string? Phone_2_Country_Code { get; set; }
        public string? Fax { get; set; }
        public string? Fax_Country_Code { get; set; }
        public string? Email_1 { get; set; }
        public string? Email_2 { get; set; }
        public string? Party_Name { get; set; }
        public int? Ship_PartyId { get; set; }
        public string? Ship_Party_Name { get; set; }
        public int? Final_Customer_Id { get; set; }
        public string? Website { get; set; }
        public string? Cust_Freight_Account_No { get; set; }
        public string? Alias_Name { get; set; }
        public string? Wechat_ID { get; set; }
        public string? Skype_ID { get; set; }
        public string? Business_Reg_No { get; set; }
        public int? Default_Remarks { get; set; }
        public string? Default_Remarks_Value { get; set; }
        public string? Notification { get; set; }
        public string? Reference_By { get; set; }
        public string? TIN_No { get; set; }
        public string? Invoice_Grp { get; set; }
        public string? Created_Date { get; set; }
        public string? Created_Time { get; set; }
        public string? Created_By { get; set; }
        public string? Updated_Date { get; set; }
        public string? Updated_Time { get; set; }
        public string? Updated_By { get; set; }
        public int? Modified_By { get; set; }
        public bool? Status { get; set; }
        public string? LOOSE_Diamond_Type_Value { get; set; }
        public string? LOOSE_Assist_Value_1 { get; set; }
        public float? LOOSE_Per_1 { get; set; }
        public string? LOOSE_Assist_Value_2 { get; set; }
        public float? LOOSE_Per_2 { get; set; }
        public DateTime? LOOSE_Date { get; set; }
        public string? CERTIFIED_Diamond_Type_Value { get; set; }
        public string? CERTIFIED_Assist_Value_1 { get; set; }
        public float? CERTIFIED_Per_1 { get; set; }
        public string? CERTIFIED_Assist_Value_2 { get; set; }
        public float? CERTIFIED_Per_2 { get; set; }
        public string? CERTIFIED_Viewing_Rights_To_Values { get; set; }
        public DateTime? CERTIFIED_Date { get; set; }
    }
}
