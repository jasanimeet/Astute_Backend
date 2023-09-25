using Microsoft.AspNetCore.Http;
using System;
using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Company_Document
    {
        [Key]
        public int Company_Document_Id { get; set; }
        public int? Company_Id { get; set; }
        public string? Company_Name { get; set; }
        public int? Cat_Val_Id { get; set; }
        public string? Cat_Name { get; set; }
        public string? Start_Date { get; set; }
        public string? Expiry_Date { get; set; }
        [NotMapped]
        public IFormFile? Upload_Path_Name { get; set; }
        public string? Upload_Path { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
