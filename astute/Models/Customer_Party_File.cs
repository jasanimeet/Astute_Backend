using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public class Customer_Party_File
    {
        [Key]
        public int File_Id { get; set; }
        public int? Party_Id { get; set; }
        public string? File_Name{ get; set; }
        public string? File_Type { get; set; }
        public string? IP { get; set; }
        public string? Country { get; set; }
        public bool? File_Status { get; set; }
        public string? Map_Flag { get; set; }
        [NotMapped]
        public IList<Customer_Column_Caption> Customer_Column_Caption { get; set; } = new List<Customer_Column_Caption>();
    }
}
