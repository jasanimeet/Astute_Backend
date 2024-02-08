using Org.BouncyCastle.Tls;
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
        public bool? API_Status { get; set; }
        public bool? Ftp_Status { get; set; }
        public bool? File_Status { get; set; }
        public string? Created_Date { get; set; }
        public string? Created_Time { get; set; }
        public string? Created_By { get; set; }
        public string? Updated_Date { get; set; }
        public string? Updated_Time { get; set; }
        public string? Updated_By { get; set; }
    }
}
