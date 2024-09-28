using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Party_Document
    {
        [Key]
        public int Document_Id { get; set; }
        public int? Party_Id { get; set; }
        public string? Party_Name { get; set; }
        public int? Document_Type { get; set;}
        public string? Document_Type_Value { get; set; }
        public string? Document_No { get; set; }
        [NotMapped]
        public IFormFile? Upload_Path_Name { get; set; }
        public string? Upload_Path { get; set; }
        public string? Valid_From { get; set; }
        public string? Valid_To { get; set; }
        public int? Kyc_Grade { get; set; }
        public string? Kyc_Grade_Name { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
