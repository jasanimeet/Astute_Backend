using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}
