using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Party_Print_Process
    {
        [Key]
        public int Print_Process_Id { get; set; }
        public int? Party_Id { get; set; }
        public string? Start_Date { get; set; }
        public string? Process_Type { get; set; }
        public string? Process_Type_Value { get; set; }
        public int? Default_Printing_Type { get; set; }
        public string? Default_Printing_Type_Value { get; set; }
        public int? Default_Currency { get; set; }
        public string? Currency { get; set; }
        public int? Default_Bank { get; set; }
        public string? Bank_Name { get; set; }
        public int? Default_Payment_Terms { get; set; }
        public string? Terms { get; set; }
        public int? Default_Remarks { get; set; }
        public string? Remarks { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
