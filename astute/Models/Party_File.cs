using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace astute.Models
{
    public partial class Party_File
    {
        [Key]
        public int File_Id { get; set; }
        public int? Party_Id { get; set; }
        public string? File_Location { get; set; }
        public string? Short_Code { get; set; }
        public bool? File_Status { get; set; }
        public bool? Lab { get; set; }
        public bool? Overseas { get; set; }
        public int? Validity_Days { get; set; }
        public string? Sheet_Name { get; set; }
        public bool? Exclude { get; set; }
        public bool? API_Flag { get; set; }
        public string? Upload_Type { get; set; }
        public bool? Is_Same_Id { get; set; }
        public bool? Overseas_Same_Id { get; set; }
        [NotMapped]
        public bool? Is_Overwrite { get; set; } = false;

    }
}
