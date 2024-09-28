using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    [Keyless]
    public class Party_API_With_Column_Mapping
    {
      
        public int? API_Id { get; set; }
        public int? FTP_Id { get; set; }
        public int? Party_Id { get; set; }
        public string? Upload_Type { get; set; }
        public string? API_URL { get; set; }
        public string? API_User { get; set; }
        public string? API_Password { get; set; }
        public string? API_Method { get; set; }
        public string? API_Response { get; set; }
        public bool? API_Status { get; set; }
        public bool? Disc_Inverse { get; set; }
        public bool? Auto_Ref_No { get; set; }
        public string? RepeateveryType { get; set; }
        public string? Repeatevery { get; set; }
        public bool? Lab { get; set; }
        public bool? Overseas { get; set; }
        public string? Stock_Url { get; set; }
        public string? User_Id { get; set; }
        public string? User_Caption { get; set; }
        public string? Password_Caption { get; set; }
        public string? Action_Caption { get; set; }
        public string? Action_Value { get; set; }
        public string? Action_Caption_1 { get; set; }
        public string? Action_Value_1 { get; set; }
        public string? Action_Caption_2 { get; set; }
        public string? Action_Value_2 { get; set; }
        public string? Short_Code { get; set; }
        public string? Stock_Api_Method { get; set; }
        public string? Method_Type { get; set; }
        public string? Host { get; set; }
        public int? Ftp_Port { get; set; }
        public string? Ftp_User { get; set; }
        public string? Ftp_Password { get; set; }
        public string? Ftp_File_Name { get; set; }
        public string? Ftp_File_Type { get; set; }
        public bool? Secure_Ftp { get; set; }
        [NotMapped]
        public IList<Dictionary<string, object>> Supplier_Column_Mapping_List { get; set; }
    }
}
