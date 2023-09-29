using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Supplier_Details_List
    {
        [Key]
        public int Party_Id { get; set; }
        public string? Party_Name { get; set; }
        public string? Party_Type { get; set; }
        public int? API_Id { get; set; }
        public int? FTP_Id { get; set; }
        public int? File_Id { get; set; }
        public string? File_Type { get; set; }
    }
}
