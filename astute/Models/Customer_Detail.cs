using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Customer_Detail
    {
        [Key]
        public int Party_Id { get; set; }
        public string? User_Id { get; set; }
        public Customer_Party_Api_Model Customer_Party_Api { get; set; }
        public Customer_Party_FTP_Model Customer_Party_FTP { get; set; }
        public Customer_Party_File_Model Customer_Party_File { get; set; }

       

    }

    public class Customer_Party_Api_Model
    {
    }

    public class Customer_Party_FTP_Model
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
        public IList<Customer_Column_Caption> Customer_Column_Caption { get; set; } = new List<Customer_Column_Caption>();
    }

    public class Customer_Party_File_Model
    {
        [Key]
        public int File_Id { get; set; }
        public int? Party_Id { get; set; }
        public string? File_Name { get; set; }
        public string? File_Type { get; set; }
        public string? IP { get; set; }
        public bool? File_Status { get; set; }

        public IList<Customer_Column_Caption> Customer_Column_Caption { get; set; } = new List<Customer_Column_Caption>();
    }
}
