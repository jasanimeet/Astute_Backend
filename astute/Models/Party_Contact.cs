using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace astute.Models
{
    public partial class Party_Contact
    {
        [Key]
        public int Contact_Id { get; set; }
        public string? Contact_Name { get; set; }
        public int? Party_Id { get; set; }
        public string? Party_Name { get; set; }
        public string? Sex { get; set; }
        public int? Designation_Id { get; set; }
        public string? Designation { get; set; }
        public string? Mobile_No { get; set; }
        public string? Email { get; set; }
        public string? Birth_Date { get; set; }
        [NotMapped]
        public string QueryFlag { get; set; }
    }
}
