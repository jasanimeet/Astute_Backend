using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class User_Registration
    {
        [Key]
        public int User_Id { get; set; }
        public string? Company_Name { get; set; }
        public string? Prefix { get; set; }
        public string? First_Name { get; set; }
        public string? Last_Name { get; set; }
        public string? Address { get; set; }
        public string? City_Id { get; set; }
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
        public int? Designation { get; set; }
        public string? Designation_Value { get; set; }
        public string? Business_Reg_No { get; set; }
        [NotMapped]
        public IFormFile? Business_Reg_Upload_File { get; set; }
        public string? Business_Reg_Upload { get; set; }
        [NotMapped]
        public IFormFile? Photo_Proof_Upload_File { get; set; }
        public string? Photo_Proof_Upload { get; set; }
        [NotMapped]
        public IFormFile? Address_Proof_Upload_File { get; set; }
        public string? Address_Proof_Upload { get; set; }
        public string? User_Name { get; set; }
        public string? Password { get; set; }
        public string? Status { get; set; }
    }
}
