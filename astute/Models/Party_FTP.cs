using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Party_FTP
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
        public bool? Auto_Ref_No { get; set; }
        public string? RepeateveryType { get; set; }
        public string? Repeatevery { get; set; }
        public bool? Lab { get; set; }
        public bool? Overseas { get; set; }
        public bool? Secure_Ftp { get; set; }
        public string? Short_Code { get; set; }
        public bool? Ftp_Status { get; set; }
        public bool? Is_Same_Id { get; set; }
        public bool? Overseas_Same_Id { get; set; }
    }
}
