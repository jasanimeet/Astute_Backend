using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace astute.Models
{
    public partial class Company_Bank
    {
        [Key]
        public int Company_Bank_Id { get; set; }
        public int? Company_Id { get; set; }
        public string? Company_Name { get; set; }
        public int? Bank_Id { get; set; }
        public string? Bank_Name { get; set; }
        public string? Branch_Name { get; set; }
        public string? Currency { get; set; }
        public string? Currency_Name { get; set; }
        public int? Account_Type { get; set; }
        public string? Account_Type_Name { get; set; }
        public string? Account_No { get; set; }
        public string? Process_Id { get; set; }
        public string? Process_Names { get; set; }
        public bool? Status { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
