using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Party_Bank
    {
        [Key]
        public int Account_Id { get; set; }
        public int? Party_Id { get; set; }
        public int? Bank_Id { get; set; }
        public string? Bank_Name { get; set; }
        public string? Branch_Address { get; set; }
        public string? Ifsc_Code { get; set; }
        public string? Correspondent_Bank { get; set; }
        public string? Correspondent_Ifsc_Code { get; set; }
        public string? Account_No { get; set; }
        public bool? Status { get; set; }
        public int? Account_Type { get; set; } 
        public string? Account_Type_Value { get; set; }
        public bool? Default_Bank { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
