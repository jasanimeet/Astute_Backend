using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Party_Shipping
    {
        [Key]
        public int Ship_Id { get; set; }
        public int? Party_Id { get; set; }
        public string? Party_Name { get; set; }
        public string? Company_Name { get; set; }
        public string? Address_1 { get; set; }
        public string? Address_2 { get; set; }
        public string? Address_3 { get; set; }
        public int? City_Id { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Mobile_No { get; set; }
        public string? Mobile_No_Country_Code { get; set; }
        public string? Phone_No { get; set; }
        public string? Phone_No_Country_Code { get; set; }
        public string? Contact_Person { get; set; }
        public string? TIN_No { get; set; }
        public bool? Default_Address { get; set; }
        public bool? Status { get; set; }
        public bool? Is_Editable { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
