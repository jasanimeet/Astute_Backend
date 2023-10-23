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
        public int? Party_Id { get; set; }
        public string? Prefix { get; set; }
        public string? First_Name { get; set; }
        public string? Last_Name { get; set; }
        public string? Party_Name { get; set; }
        public int? Designation_Id { get; set; }
        public string? Designation { get; set; }
        public string? Phone_No { get; set; }
        public string? Mobile_No { get; set; }
        public string? Email { get; set; }
        [NotMapped]
        public string QueryFlag { get; set; }
    }
}
