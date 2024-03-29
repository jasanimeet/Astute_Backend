using System.ComponentModel.DataAnnotations;

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
        public bool? File_Status { get; set; }
    }
}
