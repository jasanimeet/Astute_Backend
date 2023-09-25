using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Employee_Document
    {
        [Key]
        public int Employee_Document_Id { get; set; }
        public int? Employee_Id { get; set; }
        public int? Document_Type { get; set; }
        public string? Document_Type_Name { get; set; }
        public string? Document_Expiry_Date { get; set; }
        [NotMapped]
        public IFormFile? Document_Url_Name { get; set; }
        public string? Document_Url { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
