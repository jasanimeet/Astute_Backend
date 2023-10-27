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
        public string? File_Type { get; set; }
        public string? Offer_Name { get; set; }
        public bool? Disc_Inverse { get; set; }
        public bool? Auto_Ref_No { get; set; }
        public string? File_Location_1 { get; set; }
        public string? File_Type_1 { get; set; }
        public string? File_Location_2 { get; set; }
        public string? File_Type_2 { get; set; }
        public string? RepeateveryType { get; set; }
        public string? Repeatevery { get; set; }
        public string? Short_Code { get; set; }
        public bool? File_Status { get; set; }
    }
}
