using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string? Ftp_File_Format { get; set; }
        public string? RepeateveryType { get; set; }
        public string? Repeatevery { get; set; }
        public bool? Secure_Ftp { get; set; }
        public bool? Ftp_Status { get; set; }
        public string? Map_Flag { get; set; }

        [NotMapped]
        public IList<Customer_Column_Caption> Customer_Column_Caption { get; set; } = new List<Customer_Column_Caption>();
    }
}
