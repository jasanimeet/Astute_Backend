using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Customer_Party_FTP
    {
        [Key]
        public int FTP_Id { get; set; }
        public int? Party_Id { get; set; }
        public string? Host { get; set; }
        public int? Ftp_Port { get; set; }
        public string? Ftp_User { get; set; }
        public string? Ftp_Password { get; set; }
        public string? Ftp_File_Name { get; set; }
        public string? Ftp_File_Type { get; set; }
        public bool? Disc_Inverse { get; set; }
        public string? RepeateveryType { get; set; }
        public string? Repeatevery { get; set; }
        public bool? Secure_Ftp { get; set; }
        public string? Short_Code { get; set; }
        public bool? Ftp_Status { get; set; }
    }
}
